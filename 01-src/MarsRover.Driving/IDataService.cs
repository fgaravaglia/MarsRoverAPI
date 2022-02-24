using System.Collections.Generic;

namespace MarsRover.Driving
{
    public interface IDataService
    {
        void SetObstacles(IEnumerable<CoordinatesDTO> obstacles);
    }
}