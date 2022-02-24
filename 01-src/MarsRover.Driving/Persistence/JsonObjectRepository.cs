using System;
using System.IO;
using System.Text.Json;

namespace MarsRover.Driving.Persistence
{
    internal class JsonObjectRepository<T> where T : class
    {
        readonly string _FolderPtah;
        readonly string _FileName;

        public JsonObjectRepository(string folderPath, string fileName)
        {
            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentNullException(nameof(folderPath));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            this._FolderPtah = folderPath;
            this._FileName = fileName;
        }

        public T Get()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if(!File.Exists(Path.Combine(this._FolderPtah, this._FileName)))
                return Activator.CreateInstance<T>();
                
            string jsonString = File.ReadAllText(Path.Combine(this._FolderPtah, this._FileName));
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }

        public void Save(T settings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(Path.Combine(this._FolderPtah, this._FileName), jsonString);
        }
    }
}