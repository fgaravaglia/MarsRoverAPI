using System.Collections.Generic;
using System.Linq;
using MarsRover.Driving.Engines;
using MarsRover.Driving.Persistence;

namespace MarsRover.Driving
{
    public class JsonDataService : IDataService
    {
        readonly JsonObstacleRepository _Repository;

        public JsonDataService(string folderPath)
        {
            this._Repository = new  JsonObstacleRepository(folderPath);
        }

        public void SetObstacles(IEnumerable<CoordinatesDTO> obstacles)
        {
            var obstacleList = new List<Obstacle>();
            obstacles.ToList().ForEach(x => 
            {
                obstacleList.Add(new Obstacle(){ Position = new Coordinates(x.positionX, x.positionY, x.positionZ)});
            });
            this._Repository.Save(obstacleList);
        }
    }
}