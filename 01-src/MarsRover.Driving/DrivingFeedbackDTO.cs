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

}