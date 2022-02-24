using MarsRover.Driving.Engines;
using MarsRover.Driving.Settings;

namespace MarsRover.Driving
{
    /// <summary>
    /// Settings taht can be changed from outside of Domain
    /// </summary>
    public class DriverSettings
    {
        public string ReferenceSystem {get; set;}

        public string DataPath {get; set;}

        public DriverSettings()
        {
            this.ReferenceSystem = Coordinates.SystemsEnum.Cartesian.ToString();
        }
    }

    /// <summary>
    /// Interface to model behaviour for capablity to retrieve and update settings
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets the settings for Driver System
        /// </summary>
        /// <returns></returns>
        DriverSettings Get();
        /// <summary>
        /// Saves the settings
        /// </summary>
        /// <param name="referenceSystem">Coordinates Reference System</param>
        void Save(string referenceSystem);
    }
}