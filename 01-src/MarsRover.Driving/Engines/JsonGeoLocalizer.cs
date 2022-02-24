using System;
using MarsRover.Driving.Persistence;

namespace MarsRover.Driving.Engines
{
    /// <summary>
    /// IMplementation of localizer based on json file
    /// </summary>
    internal class JsonGeoLocalizer : IGeoLocalizer
    {
        readonly JsonObjectRepository<Coordinates> _Repository;
        
        public JsonGeoLocalizer(string folderPath)
        {
            this._Repository = new JsonObjectRepository<Coordinates>(folderPath, "Localization.json");
        }

        public Coordinates GetPosition()
        {
            return this._Repository.Get();
        }

        public void SavePosition(Coordinates point)
        {
            this._Repository.Save(point);
        }
    }
}