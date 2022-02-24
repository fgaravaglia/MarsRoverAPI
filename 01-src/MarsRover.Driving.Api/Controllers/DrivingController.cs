using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarsRover.Driving.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrivingController : ControllerBase
    {
        public DrivingController()
        {

        }

        // // GET: api/Driving
        // [HttpGet("mytarget/{id}", Name = "Get")]
        // public string Get(int id)
        // {
        //     return "OK";
        // }

        /// <summary>
        /// Gets the settings fo Driving Engine
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Driving/Settings
        ///
        /// </remarks>
        /// <returns>Actual Settings of Driving Engine</returns>
        /// <response code="200">The operations run succesfully</response>
        /// <response code="500">If unexpected error occurred</response>
        [HttpGet("Settings", Name = "GetSettings")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public DrivingSettingsResponse GetSettings()
        {
            var response = new DrivingSettingsResponse();
            return response;
        }

        // 
        /// <summary>
        /// Moves the rovers forward/Backwards and Right/left
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>Sample: \POST api/Driving/MoveTo</remarks>
        /// <returns>the informations about actual position of rover</returns>
        /// <response code="200">The operations run succesfully</response>
        /// <response code="500">If unexpected error occurred</response>
        [HttpPost("MoveTo", Name = "MoveTo")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public DrivingResponse Post([FromBody] DrivingRoute request)
        {
            // Logic to create new Employee
            return new DrivingResponse();
        }
    }

    public class DrivingSettingsResponse
    {
        [Required]
        [DefaultValue("CARTESIANO")]
        public string Mode {get; set;}

        public double MovingStepAmount {get; set;}

        public DrivingSettingsResponse()
        {
            this.Mode = "CARTESIANO";
            this.MovingStepAmount = 1.0;
        }
    }

    public class DrivingResponse
    {

    }

    public class DrivingRoute
    {

    }
}