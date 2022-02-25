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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;

namespace MarsRover.Driving.Api.Controllers
{
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi=true)]
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        [HttpGet("", Name = "GetError")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            // Get the details of the exception that occurred
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            string routeWhereExceptionOccurred = string.Empty;
            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                //Do something with the exception
                Console.WriteLine($"[ERR] [{routeWhereExceptionOccurred}] [{exceptionThatOccurred.Message}]");
                Console.WriteLine(exceptionThatOccurred.StackTrace);
            }
            return Problem(
                detail: "Error while processing posted data. Please contact your System Administrator",
                instance: routeWhereExceptionOccurred, //A reference that identifies the specific occurrence of the problem
                title: "An Unexpected Error occurred", //a short title, maybe ex.Message
                statusCode: StatusCodes.Status500InternalServerError//will always return code 500 if not explicitly set
                //type: "http://example.com/errors/error-123-details"  //a reference to more information
            );
        }
    }
}