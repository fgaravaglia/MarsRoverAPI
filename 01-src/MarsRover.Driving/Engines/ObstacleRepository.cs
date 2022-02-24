using System;
using System.Collections.Generic;
using MarsRover.Driving.Persistence;

namespace MarsRover.Driving.Engines
{
    /// <summary>
    /// Interface to design component behaviour for a arepository of obsacles
    /// </summary>
    internal interface IObstacleRepository
    {
        /// <summary>
        /// Gets the full list of obsatacles
        /// </summary>
        /// <returns></returns>
        IList<Obstacle> GetAll();
        /// <summary>
        /// Persist it
        /// </summary>
        /// <param name="list"></param>
        void Save(List<Obstacle> list);
    }
    
    /// <summary>
    /// In-Memory hard coded implementation of repository
    /// </summary>
    internal  class ObstacleRepository :  IObstacleRepository
    {
        public IList<Obstacle> GetAll()
        {
            List<Obstacle> results = new List<Obstacle>();
            results.Add(new Obstacle(){
                Position = new Coordinates(2.0, 1.0) 
            });
            return results;
        }

        public void Save(List<Obstacle> list)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// JSon file repository implementation
    /// </summary>
    internal  class JsonObstacleRepository :  IObstacleRepository
    {
        readonly JsonObjectRepository<List<Obstacle>> _Repository;
        
        public JsonObstacleRepository(string folderPath)
        {
            this._Repository = new JsonObjectRepository<List<Obstacle>>(folderPath, "Obstacles.json");
        }

        public IList<Obstacle> GetAll()
        {
            return this._Repository.Get();
        }

        public void Save(List<Obstacle> list)
        {
            this._Repository.Save(list);
        }
    }
}