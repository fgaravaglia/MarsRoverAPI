using System;
using MarsRover.Driving.Settings;

namespace MarsRover.Driving
{
    public class SettingsService : ISettingsService
    {
        readonly IDriverSettingsRepository _Repository;

        public SettingsService(string folderPath)
        {
            this._Repository = new JsonDriverSettingsRepository(folderPath);
        }

        public DriverSettings Get()
        {
            var dto = this._Repository.Get();
            return new DriverSettings() 
            { 
                ReferenceSystem = dto.ReferenceSystem,
                DataPath = dto.DataPath
            };
        }

        public void Save(string referenceSystem)
        {
            if(string.IsNullOrEmpty(referenceSystem))
                throw new ArgumentNullException(nameof(referenceSystem));
                
            var dto = this._Repository.Get();
            dto.ReferenceSystem = referenceSystem;
            this._Repository.Save(dto);
        }
    }
}