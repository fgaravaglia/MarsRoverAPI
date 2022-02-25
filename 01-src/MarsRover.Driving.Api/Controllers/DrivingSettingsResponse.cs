using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarsRover.Driving.Api.Controllers
{
    public class DrivingSettings
    {
        [Required]
        [DefaultValue("CARTESIANO")]
        public string Mode {get; set;}

        [Required]
        public string DataPath {get; set;}

        public DrivingSettings()
        {
            this.Mode = "CARTESIANO";
            this.DataPath = @"C:\Temp";
        }
    }

    public class DrivingSettingsResponse : DrivingSettings
    {
        public DrivingSettingsResponse() : base()
        {
        }
    }
}