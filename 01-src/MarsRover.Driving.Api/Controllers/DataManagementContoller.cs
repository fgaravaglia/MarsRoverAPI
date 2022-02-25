using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MarsRover.Driving;

namespace MarsRover.Driving.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataManagementController : ControllerBase
    {
        readonly IDataService _Service;

        public DataManagementController()
        {
            this._Service = new JsonDataService(@"C:\Temp");
        }

         /// <summary>
        /// Sets the Obstacles for Mars Rover
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT  /DatManagement/Obstacles
        ///     [ {"positionX": 1.25, "positionY": 0.25, "positionZ": null} ]
        /// 
        /// </remarks>
        /// <returns>successo or not</returns>
        /// <response code="200">The operations run succesfully</response>
        /// <response code="500">If unexpected error occurred</response>
        [HttpPut("Obstacles", Name = "SaveObstacles")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public string SaveObstacles([FromBody] IEnumerable<CoordinatesDTO> obstacles)
        {
            this._Service.SetObstacles(obstacles);
            return "0";
        }

        #region Request - Response

        #endregion
    }
}