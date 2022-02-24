using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Driving.Metrics;

namespace MarsRover.Driving.Engines
{
    /// <summary>
    /// Abstraction to define the behaviour of a Radar system
    /// </summary>
    internal interface IRadarSystem
    {
        /// <summary>
        /// Scans the area to identify obstacles
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        IList<Obstacle> ScanArea(Coordinates position);

        /// <summary>
        /// Scans the area to identify obstacles
        /// </summary>
        /// <param name="position">actual position of rover</param>
        /// <param name="radius">radius of sphere to check starting from position</param>
        /// <returns></returns>
        IList<Obstacle> ScanArea(Coordinates position, double radius);

        /// <summary>
        /// sets the actual radius of radar
        /// </summary>
        /// <param name="radius"></param>
        void SetRadius(double radius);

    }

    internal class RadarSystem : IRadarSystem
    {
        #region Attributes
        double _radarRadius;
        readonly IObstacleRepository _obstacleRepo;
        readonly Dictionary<Coordinates.SystemsEnum, IDistanceMeter> _meters;
        #endregion

        public RadarSystem(IObstacleRepository obstacleRepository)
        {
            this._radarRadius = 5.0;
            this._obstacleRepo = obstacleRepository ?? throw new ArgumentNullException(nameof(obstacleRepository));
            this._meters = new Dictionary<Coordinates.SystemsEnum, IDistanceMeter>();
            this._meters.Add(Coordinates.SystemsEnum.Cartesian, new CartesialDistanceMeter());
            //this._meters.Add(Coordinates.SystemsEnum.Cartesian, new CartesialDistanceMeter());
        }

        public  void SetRadius(double radius)
        {
            if(radius < 1.0)
                throw new ArgumentException("Unable to set radius minor of 1.0", nameof(radius));
            this._radarRadius = radius;
        }

        public static IRadarSystem FromSettings(DriverSettings settings, IObstacleRepository obstacleRepository)
        {
            var radar =  new RadarSystem(obstacleRepository);
            radar.SetRadius(1.0);
            return radar;
        }

        public IList<Obstacle> ScanArea(Coordinates position)
        {
            return ScanArea(position, this._radarRadius);
        }

        public IList<Obstacle> ScanArea(Coordinates position, double radius)
        {
            if(position == null)
                throw new ArgumentNullException(nameof(position));
            if(!this._meters.ContainsKey(position.System))
                throw new ApplicationException($"Wrong coordinates: {position.System} not managed");

            var obstacles = this._obstacleRepo.GetAll().ToList();
            List<Obstacle> results = new List<Obstacle>();
            // select the right distance meter, based on reference system
            IDistanceMeter meter = _meters[position.System];
            foreach(var obs in obstacles)
            {
                // convert sperical coordinates to cartesian
                var targetPosition = obs.Position;
                if(obs.Position.System != position.System)
                {
                    // do the conversion
                    throw new NotImplementedException();
                }
                // calculate Distance from acutal position
                var distance = meter.Calculate(position, targetPosition);
                if( distance <=  radius)
                {
                    LogDebug($"Found Obstacle at {targetPosition.AsString}: R={distance} => Inside Radius");
                    results.Add(obs);
                }
                else
                {
                    LogDebug($"Found Obstacle at {targetPosition.AsString}: R={distance} => Outside Radius");
                }

            }
            return results.ToList();
        }

        private void LogDebug(string message)
        {
            Console.WriteLine("[DEBUG] " + message);
        }
    }
}