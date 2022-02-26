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
    public class DrivingController : ControllerBase
    {
        readonly ISettingsService _SettingService;

        public DrivingController()
        {
            this._SettingService = new SettingsService(@"C:\Temp");

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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public DrivingSettingsResponse GetSettings()
        {
            var settings = this._SettingService.Get();
            var response = new DrivingSettingsResponse();
            response.Mode = settings.ReferenceSystem;
            response.DataPath = settings.DataPath;
            return response;
        }

        /// <summary>
        /// Gets the settings fo Driving Engine
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Driving/Settings
        ///     { "mode": "Cartesian" }
        /// 
        /// </remarks>
        /// <returns>Actual Settings of Driving Engine</returns>
        /// <response code="200">The operations run succesfully</response>
        /// <response code="500">If unexpected error occurred</response>
        [HttpPut("Settings", Name = "GetSettings")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public string GetSettings([FromBody] SaveDrivingSettingsRequest settings)
        {
            this._SettingService.Save(settings.Mode);
            return "0";
        }

        // 
        /// <summary>
        /// Moves the rovers forward/Backwards and Right/left, based on list of commands
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Driving/Move
        ///     {
        ///        "Commands": [ 
        ///            { "x": 0.0, "y": 0.0, "z": null, "direction": "F", "side": "R" }, 
        ///            {  "direction": "F", "side": "R" },
        ///            { "direction": "B", "side": "L" }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>the informations about actual position of rover, in cartesian or Spherical coorinates (it depends on configuration</returns>
        /// <response code="200">The operations run succesfully</response>
        /// <response code="500">If unexpected error occurred</response>
        [HttpPost("Move", Name = "MoveTo")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public DrivingResponse Post([FromBody] DrivingRoute request)
        {
            var settings = this._SettingService.Get();
            var service = new DrivingService(settings);

            // map Request to object
            var commands = new List<DrivingCommandDTO>();
            request.Commands.ForEach(x => {
                commands.Add(new DrivingCommandDTO()
                {
                     Direction = x.Direction,
                     Side = x.Side,
                     StartingPoint = new CoordinatesDTO(){ positionX = x.X, positionY = x.Y, positionZ = x.Z    }
                });
            });

            var feedback = service.Move(commands);

            // build the json
            return new DrivingResponse()
            {
                HasMoved = feedback.HasMoved,
                ResultCode = feedback.ResultCode,
                ResultMessage = feedback.ResultMessage,
                TargetPosition = feedback.TargetPosition,
                Obstacle = feedback.Obstacle
            };
        }
    }

    #region Request - Response
    public class DrivingRoute
    {
        public List<DrivingCommand> Commands {get; set;}
    }

    public class DrivingCommand
    {
        public double X{get; set;}

        public double Y{get; set;}

        public double? Z{get; set;}
        public string Direction {get; set;}

        public string Side {get; set;}
    }

    #endregion
}