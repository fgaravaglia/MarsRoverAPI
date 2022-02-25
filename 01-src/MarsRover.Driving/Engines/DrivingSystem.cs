using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Driving.Engines
{
    internal class DrivingSystem
    {
        #region Attributes
        readonly IRadarSystem _radar;
        readonly IGeoLocalizer _localizer;
        Guid _trxId = Guid.Empty;
        List<Obstacle> _obstacles;
        #endregion

        public DrivingSystem(IRadarSystem radar, IGeoLocalizer localizer)
        {
            this._radar = radar ?? throw new ArgumentNullException(nameof(radar));
            this._localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this._trxId = Guid.Empty;
        }

        public void SetActualPosition(double x, double y, double? z)
        {
            var position = new Coordinates(x, y, z);
            this._localizer.SavePosition(position);
            LogInfo(this._trxId, $"Actual Position: {position.AsString}");
        }

        /// <summary>
        /// Starts the engine
        /// </summary>
        /// <param name="startingPoint"></param>
        public void Start(Coordinates startingPoint)
        {
            this._trxId = Guid.NewGuid();
            if (startingPoint == null)
                throw new ArgumentNullException(nameof(startingPoint));

            SetActualPosition(startingPoint.positionX, startingPoint.positionY, startingPoint.positionZ);
        }

        /// <summary>
        /// Moves the rover to next point, following the input
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public DrivingFeedback MoveTo(string direction, string side)
        {
            if (String.IsNullOrEmpty(direction))
                throw new ArgumentNullException(nameof(direction));
            if (direction != "F" && direction != "B")
                throw new ArgumentException("Invalid Input: Available directions are F, B", nameof(direction));
            if (!String.IsNullOrEmpty(side) && (side != "L" && side != "R"))
                throw new ArgumentException("Invalid Input: Available side are L, R", nameof(side));
            
            // get actual position
            var actualPosition = this._localizer.GetPosition();
            if (actualPosition == null)
                throw new ApplicationException("Unable to move: actual position is not set");
            if (this._trxId == Guid.Empty)
                throw new ApplicationException("Engine Not started. Please invoke Start method");

            // set directions flow
            int directionFlow = direction == "F" ? +1 : -1;
            int sideFlow = 0;
            if (!String.IsNullOrEmpty(side))
            {
                switch (side)
                {
                    case "R":
                        sideFlow = 1;
                        break;
                    case "L":
                        sideFlow = -1;
                        break;
                    default:
                        throw new ArgumentException("Invalid Input: Available side are L, R", nameof(side));
                }
            }
            int stepAmount = 1;

            // Check for obstacles
            this._obstacles = this._radar.ScanArea(actualPosition, stepAmount).ToList();
            LogInfo(this._trxId, $" Found { this._obstacles.Count()} obstacles inside the radar radius");
            bool foundObstacles = this._obstacles.Count > 0;
            if (foundObstacles)
            {
                return new DrivingFeedback()
                {
                    HasMoved = false,
                    ResultCode = "ERR-100",
                    ResultMessage = "Unable to Move to target position: Obstacle has been found on the path",
                    TargetPosition = actualPosition,
                    Obstacle = this._obstacles.First().Position
                };
            }

            // Set the actual position 
            LogInfo(this._trxId, $" Moving to target position");
            double finalX = actualPosition.positionX + sideFlow * stepAmount;
            double finalY = actualPosition.positionY + directionFlow * stepAmount;
            double? finalZ = actualPosition.positionZ;
            SetActualPosition(finalX, finalY, finalZ);

            return new DrivingFeedback()
            {
                HasMoved = true,
                ResultCode = "OK",
                TargetPosition = new Coordinates(finalX, finalY, finalZ)
            };
        }

        /// <summary>
        /// Executes the commands in input to move the rovers along a sequence of steps
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public DrivingFeedback Move(IEnumerable<DrivingCommand> commands)
        {
            if (commands == null)
                throw new ArgumentNullException(nameof(commands));
            List<DrivingFeedback> feedbacks = new List<DrivingFeedback>();

            var cmdIndex =1;
            DrivingCommand cmd = commands.ToList().First();
            // get actual position
            Coordinates lastPosition = this._localizer.GetPosition();
            if (lastPosition == null)
                throw new ApplicationException("Unable to move: actual position is not set");
            DrivingFeedback cmdFeedback = this.MoveTo(cmd.Direction, cmd.Side);
            lastPosition = cmdFeedback.TargetPosition;
            while (cmdFeedback.HasMoved && cmdIndex < commands.Count())
            {
                try
                {
                    // get next cmd, then execute it
                    cmd = commands.ToList()[cmdIndex];
                    cmdFeedback = this.MoveTo(cmd.Direction, cmd.Side);
                    // check result
                    if (cmdFeedback.HasMoved)
                    {
                        LogInfo(this._trxId, $"Command {commands.ToList().IndexOf(cmd) + 1} succesfully processed. Move to next point"); 
                        cmdIndex++;
                    }
                    else
                    {
                        LogInfo(this._trxId, $"Command {commands.ToList().IndexOf(cmd) + 1} Failed");
                    }

                    // updat ethe current position
                    lastPosition = cmdFeedback.TargetPosition;
                }
                catch (Exception ex)
                {
                    LogException(this._trxId, ex);

                    var errorFeedback = DrivingFeedback.FromException(ex);
                    // update current status
                    cmdFeedback.HasMoved = errorFeedback.HasMoved;
                    cmdFeedback.ResultCode = errorFeedback.ResultCode;
                    cmdFeedback.ResultMessage = errorFeedback.ResultMessage;
                    cmdFeedback.Obstacle = null;
                }
            }
            // update position as a feedback
            cmdFeedback.TargetPosition = lastPosition;
            return cmdFeedback;
        }
        
        private void LogInfo(Guid trxId, string message)
        {
            Console.WriteLine($"[INFO] [{trxId.ToString()}] {message}");
        }
        private void LogException(Guid trxId, Exception ex)
        {
            Console.WriteLine($"[ERR] [{trxId.ToString()}] {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}