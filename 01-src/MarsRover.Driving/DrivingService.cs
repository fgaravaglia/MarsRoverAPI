using System;
using System.Runtime.CompilerServices;
using MarsRover.Driving.Engines;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Driving.Metrics;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace MarsRover.Driving
{
    public class DrivingService : IDrivingService
    {
        readonly DriverSettings _settings;
        readonly IObstacleRepository _obstacleRepository;
        readonly IGeoLocalizer _localizer;

        public DrivingService(DriverSettings settings)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._obstacleRepository = new JsonObstacleRepository(settings.DataPath);
            this._localizer = new JsonGeoLocalizer(settings.DataPath);
        }

        public DrivingFeedbackDTO MoveTo(CoordinatesDTO startingCoordinates, string direction, string side)
        {
            try
            {
                if (String.IsNullOrEmpty(direction))
                    throw new ArgumentNullException(nameof(direction));
                if (String.IsNullOrEmpty(side))
                    throw new ArgumentNullException(nameof(side));
                if (startingCoordinates == null)
                    throw new ArgumentNullException(nameof(startingCoordinates));

                DrivingSystem drivingSystem = new DrivingSystem(RadarSystem.FromSettings(this._settings, _obstacleRepository), this._localizer);
                var coordinates = new Coordinates(startingCoordinates.positionX, startingCoordinates.positionY, startingCoordinates.positionZ);
                drivingSystem.Start(coordinates);
                var feedback = drivingSystem.MoveTo(direction, side).ToDto();

                // check configuration for coordinates;
                if (this._settings.ReferenceSystem == Coordinates.SystemsEnum.Spherical.ToString())
                    feedback.TargetPosition = ReferenceConverterHelper.ConvertCartesianToSpherical(feedback.TargetPosition);

                return feedback;
            }
            catch (ArgumentNullException argEx)
            {
                return new DrivingFeedbackDTO()
                {
                    HasMoved = false,
                    ResultCode = "ERR-002",
                    ResultMessage = "Request non valida: " + argEx.Message
                };
            }
            catch (ApplicationException appEx)
            {
                return DrivingFeedback.FromApplicationException(appEx).ToDto();
            }
            catch (Exception ex)
            {
                // logging
                Console.WriteLine("[ERR] " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                return DrivingFeedback.FromException(ex).ToDto();
            }
        }

        public DrivingFeedbackDTO Move(IEnumerable<DrivingCommandDTO> commands)
        {
            try
            {
                var commandDtos = commands.Select(x =>
                {
                    return new Engines.DrivingCommand()
                    {
                        Direction = x.Direction,
                        Side = x.Side,
                        StartingPoint = new Coordinates(x.StartingPoint.positionX, x.StartingPoint.positionY, x.StartingPoint.positionZ)
                    };
                }).ToList();

                // CHeck for input
                if (!commandDtos.Any())
                    throw new ArgumentException("List of commands cannot be null", nameof(commands));
                // Start the engine and move it
                DrivingSystem drivingSystem = new DrivingSystem(RadarSystem.FromSettings(this._settings, _obstacleRepository), this._localizer);
                drivingSystem.Start(commandDtos.First().StartingPoint);
                return drivingSystem.Move(commandDtos).ToDto();
            }
            catch (ArgumentNullException argEx)
            {
                return new DrivingFeedbackDTO()
                {
                    HasMoved = false,
                    ResultCode = "ERR-002",
                    ResultMessage = "Request non valida: " + argEx.Message
                };
            }
            catch (ApplicationException appEx)
            {
                return new DrivingFeedbackDTO()
                {
                    HasMoved = false,
                    ResultCode = "ERR",
                    ResultMessage = appEx.Message
                };
            }
            catch (Exception ex)
            {
                // logging
                Console.WriteLine("[ERR] " + ex.Message);
                Console.WriteLine(ex.StackTrace);

                return new DrivingFeedbackDTO()
                {
                    HasMoved = false,
                    ResultCode = "ERR-001",
                    ResultMessage = "Unexpected Error: Please contact your System Administrator"
                };
            }
        }

    }
}
