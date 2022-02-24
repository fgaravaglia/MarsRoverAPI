using System;
using MarsRover.Driving.Engines;

namespace MarsRover.Driving.Metrics
{
/// <summary>
    /// INterface to hide complexity of reference system to calcualte distance
    /// </summary>
    internal interface IDistanceMeter
    {
        /// <summary>
        /// COmputes the distance between points
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        double Calculate(Coordinates startPoint, Coordinates targetPoint);

    }

}