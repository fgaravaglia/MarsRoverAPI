using System.ComponentModel.DataAnnotations;

namespace MarsRover.Driving.Api.Controllers
{
    public class DrivingResponse
    {
        [Required]
        public bool HasMoved {get; set;}

        [Required]
        public string ResultCode {get; set;}

        public string ResultMessage {get; set;}

        public CoordinatesDTO TargetPosition {get; set;}

        public CoordinatesDTO Obstacle {get; set;}
    }
    
}