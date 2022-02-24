using MarsRover.Driving.Engines;
using System.Text.Json.Serialization;

namespace MarsRover.Driving.Settings
{
    internal class DriverSettingsDTO
    {
        [JsonPropertyName("referenceSystem")]
        public string ReferenceSystem { get; set; }

        [JsonPropertyName("dataPath")]
        public string DataPath { get; set; }

        [JsonPropertyName("stepAmount")]
        public double StepAmount { get; set; }

        public DriverSettingsDTO()
        {
            this.ReferenceSystem = Coordinates.SystemsEnum.Cartesian.ToString();
            this.DataPath = @"C:\Temp";
            this.StepAmount = 1.0;
        }
    }
}