using System.Collections.Generic;

namespace MarsRover.Driving
{

    public class DrivingFeedbackDTO
    {
        public bool HasMoved {get; set;}

        public string ResultCode {get; set;}

        public string ResultMessage {get; set;}

        public CoordinatesDTO TargetPosition {get; set;}

        public CoordinatesDTO Obstacle {get; set;}
    }

    /// <summary>
    /// Business Service to interact with Domain from outside
    /// </summary>
    public interface IDrivingService
    {
        /// <summary>
        /// Moves the Rover witha  single command
        /// </summary>
        /// <param name="startingCoordinates"></param>
        /// <param name="direction">F for Forward, B for Backward</param>
        /// <param name="side">L for Left, R for Right</param>
        /// <returns>result of move</returns>
        DrivingFeedbackDTO MoveTo(CoordinatesDTO startingCoordinates, string direction,  string side);
        /// <summary>
        /// Moves the Rover with a list of command
        /// </summary>
        /// <param name="commands"></param>
        /// <returns>result of move</returns>
        DrivingFeedbackDTO Move(IEnumerable<DrivingCommandDTO> commands);
    }
}