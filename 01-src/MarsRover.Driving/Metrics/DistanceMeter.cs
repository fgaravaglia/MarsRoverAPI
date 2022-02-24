using System;
using MarsRover.Driving.Engines;

namespace MarsRover.Driving.Metrics
{
    
    /// <summary>
    /// Abstraction for meter
    /// </summary>
    internal abstract class DistanceMeter : IDistanceMeter
    {
        protected Coordinates.SystemsEnum SupportedSystem { get; private set; }

        protected DistanceMeter(Coordinates.SystemsEnum referenceSystem)
        {
            this.SupportedSystem = referenceSystem;
        }

        protected abstract double CalculateDistanceBetween(Coordinates startPoint, Coordinates targetPoint);

        public double Calculate(Coordinates startPoint, Coordinates targetPoint)
        {
            if (startPoint.System != SupportedSystem || targetPoint.System != SupportedSystem)
                throw new NotImplementedException($"Unable to manage not {SupportedSystem} Coordinates");

            return CalculateDistanceBetween(startPoint, targetPoint);
        }
    }

   
}