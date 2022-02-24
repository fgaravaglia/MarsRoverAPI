using System;
using MarsRover.Driving.Engines;

namespace MarsRover.Driving.Metrics
{
    /// <summary>
    /// Concrete implementation of Meter of Cartesian Reference
    /// </summary>
    internal class CartesialDistanceMeter : DistanceMeter
    {
        public CartesialDistanceMeter() : base(Coordinates.SystemsEnum.Cartesian)
        {

        }

        protected override double CalculateDistanceBetween(Coordinates startPoint, Coordinates targetPoint)
        {
            // calculate Distance from acutal position
            var squaredDistance = (startPoint.positionX - targetPoint.positionX) * (startPoint.positionX - targetPoint.positionX)
                                + (startPoint.positionY - targetPoint.positionY) * (startPoint.positionY - targetPoint.positionY);
            var distance = Math.Sqrt(squaredDistance);
            return distance;
        }

    }
}