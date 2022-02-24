using System.Text.Json.Serialization;

namespace MarsRover.Driving.Engines
{
    /// <summary>
    /// DTO to define coordinates
    /// </summary>
    internal class Coordinates
    {
        /// <summary>
        /// Enums for supporte reference Systems
        /// </summary>
        public enum SystemsEnum
        {
            Cartesian,
            Spherical
        }

        public double positionX { get; set; }

        public double positionY { get; set; }

        public double? positionZ { get; set; }

        public SystemsEnum System{ get ; set; }

        /// <summary>
        /// String representation of coordinates
        /// </summary>
        /// <value></value>
        public string AsString { get { 
            if(positionZ == null) 
                return $"[{positionX.ToString("0.##")},{positionY.ToString("0.##")}]";
            else
                return $"[{positionX.ToString()},{positionY.ToString()},{positionZ.Value.ToString()}]";
        } }
        
        [JsonConstructorAttribute]
        public Coordinates() : this(0,0,null){} 

        public Coordinates(double x, double y) : this(x, y, null)
        {
        }
        public Coordinates(double x, double y, double? z)
        {
            this.positionX = x;
            this.positionY = y;
            this.positionZ = z;
            this.System = Coordinates.SystemsEnum.Cartesian;
        }

        public CoordinatesDTO ToDto()
        {
            return new CoordinatesDTO()
            {
                positionX = this.positionX,
                positionY = this.positionY,
                positionZ = this.positionZ
            };
        }
    }
}