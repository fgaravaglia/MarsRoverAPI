using System;
using System.IO;
using System.Text.Json;
using MarsRover.Driving.Persistence;

namespace MarsRover.Driving.Settings
{
    internal class JsonDriverSettingsRepository : IDriverSettingsRepository
    {
        readonly JsonObjectRepository<DriverSettingsDTO> _Repository;

        public JsonDriverSettingsRepository(string folderPath)
        {
            this._Repository = new JsonObjectRepository<DriverSettingsDTO>(folderPath, "DriverSettings.json");
        }

        public DriverSettingsDTO Get()
        {
            return this._Repository.Get();
        }

        public void Save(DriverSettingsDTO settings)
        {          
           this._Repository.Save(settings);
        }
    }
}