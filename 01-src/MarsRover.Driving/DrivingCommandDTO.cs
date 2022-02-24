namespace MarsRover.Driving
{
    public class DrivingCommandDTO
    {
        public CoordinatesDTO StartingPoint {get; set;}

        public string Direction {get; set;}

        public string Side {get; set;}
    }
}