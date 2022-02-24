namespace MarsRover.Driving.Settings
{
    internal interface IDriverSettingsRepository
    {
        DriverSettingsDTO Get();

        void Save(DriverSettingsDTO settings);
    }
}