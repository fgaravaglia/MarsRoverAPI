using System;

namespace MarsRover.Driving.Metrics
{
    internal static class ReferenceConverterHelper
    {
        /// <summary>
        /// COnverts the Cartesian coordinates into spherical
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static CoordinatesDTO ConvertCartesianToSpherical(CoordinatesDTO point)
        {
            // spherical: (r, Theta, z)
            double z = point.positionZ.HasValue ? point.positionZ.Value : 0.0;
            double tanOfTheta = point.positionY / point.positionX;
            double r = Math.Sqrt(point.positionX * point.positionX) + (point.positionY * point.positionY);

            return new CoordinatesDTO()
            {
                positionX = r,
                positionY = Math.Atan(tanOfTheta),
                positionZ = z
            };
        }

         /// <summary>
        /// COnverts the Spherical coordinates into Cartesian
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static CoordinatesDTO ConvertSphericalToCartesian(CoordinatesDTO point)
        {
            // spherical: (r, Theta, z)
            double z = 1.0;
            double x = point.positionX * Math.Cos(point.positionY);
            double y = point.positionX * Math.Sin(point.positionY);

            return new CoordinatesDTO()
            {
                positionX = x,
                positionY = y,
                positionZ = z
            };
        }
    }
}