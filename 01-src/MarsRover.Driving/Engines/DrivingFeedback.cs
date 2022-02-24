using System;

namespace MarsRover.Driving.Engines
{
    internal class DrivingFeedback
    {
        public bool HasMoved {get; set;}

        public string ResultCode {get; set;}

        public string ResultMessage {get; set;}

        public Coordinates TargetPosition {get; set;}

        public Coordinates Obstacle {get; set;}

        public DrivingFeedbackDTO ToDto()
        {
            return  new DrivingFeedbackDTO()
                {
                    HasMoved = this.HasMoved,
                    ResultCode = this.ResultCode,
                    ResultMessage = this.ResultMessage,
                    TargetPosition = this.TargetPosition != null ? this.TargetPosition.ToDto() : null,
                     Obstacle = this.Obstacle != null ? this.Obstacle.ToDto() : null,
                };
        }

        public static DrivingFeedback FromApplicationException(ApplicationException appEx)
        {
            return new DrivingFeedback()
                {
                    HasMoved = false,
                    ResultCode = "ERR",
                    ResultMessage = appEx.Message
                };
        }

        public static DrivingFeedback FromException(Exception ex)
        {
            if(ex is ApplicationException)
                return FromApplicationException((ApplicationException)ex);

            return new DrivingFeedback()
                {
                    HasMoved = false,
                    ResultCode = "ERR-001",
                    ResultMessage = "Unexpected Error: Please contact your System Administrator"
                };
        }
    }
}