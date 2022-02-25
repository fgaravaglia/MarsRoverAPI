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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public DrivingSettingsResponse GetSettings()
        {
            var settings = this._SettingService.Get();
            var response = new DrivingSettingsResponse();
            response.Mode = settings.ReferenceSystem;
            response.DataPath = settings.DataPath;
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

    public class DrivingSettingsResponse
    {
        [Required]
        [DefaultValue("CARTESIANO")]
        public string Mode {get; set;}

        [Required]
        public string DataPath {get; set;}

        public DrivingSettingsResponse()
        {
            this.Mode = "CARTESIANO";
            this.DataPath = @"C:\Temp";
        }
    }

    public class DrivingResponse
    {
        public bool HasMoved {get; set;}

        public string ResultCode {get; set;}

        public string ResultMessage {get; set;}

        public CoordinatesDTO TargetPosition {get; set;}

        public CoordinatesDTO Obstacle {get; set;}
    }

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
}