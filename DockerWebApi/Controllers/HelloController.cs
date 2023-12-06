using Common.Model.Models;
using DockerWebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DockerWebApi.Controllers
{
    [ApiController, Route("api/v{version:apiVersion}/[controller]")]
    public class HelloController : ControllerBase
    {
        private readonly ILogger<HelloController> _logger;

        public HelloController(ILogger<HelloController> logger)
        {
            _logger = logger;
        }

        [HttpGet, Route("DatabaseTest"), ApiVersion("1.0"), ApiExplorerSettings(GroupName = "v1")]
        public string HelloDatabase()
        {
            _logger.LogInformation("Hello Database Version 1");

            return HelloRepository.Connect();
        }

        [HttpGet, Route("DatabaseTest"), ApiVersion("2.0"), MapToApiVersion("2.0"), ApiExplorerSettings(GroupName = "v2")]
        public HelloDatabaseModel HelloDatabase2()
        {
            _logger.LogInformation("Hello Database Version 2");

            var helloData = new HelloDatabaseModel
            {
                TimeString = HelloRepository.Connect(),
                Message = "Hello Database Version 2"
            };

            return helloData;
        }
    }
}