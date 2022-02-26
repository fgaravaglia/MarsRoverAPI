using System.Collections.Generic;

namespace MarsRover.Driving
{

    /// <summary>
    /// Business Service to interact with Domain from outside
    /// </summary>
    public interface IDrivingService
    {
        /// <summary>
        /// Moves the Rover with a list of command
        /// </summary>
        /// <param name="commands">commands to execute</param>
        /// <returns>result of move</returns>
        DrivingFeedbackDTO Move(IEnumerable<DrivingCommandDTO> commands);
    }
}