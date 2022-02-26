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
        /// <remarks> spherical: (r, Theta, Phi)
        /// simplification: 
        /// - assuming that we are in first quadrant: z, x, y > 0
        /// - x, y are pretty much already the right values (small movement across the planet)
        /// </remarks>
        public static CoordinatesDTO ConvertCartesianToSpherical(CoordinatesDTO point)
        {

            double oldZ = point.positionZ.HasValue ? point.positionZ.Value : 1.0;
            double r = Math.Sqrt(oldZ*oldZ + point.positionX*point.positionX + point.positionY*point.positionY);
            double theta = Math.Atan(point.positionY / point.positionX);
            double phi = Math.Acos(oldZ / r );

            return new CoordinatesDTO()
            {
                positionX = r,
                positionY = theta,
                positionZ = phi
            };
        }
        /// <summary>
        /// COnverts the Cartesian coordinates into Cylindrical
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static CoordinatesDTO ConvertCartesianToCylindrical(CoordinatesDTO point)
        {
            // reference: (r, Theta, z)
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
        public static CoordinatesDTO ConvertCylindricalToCartesian(CoordinatesDTO point)
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