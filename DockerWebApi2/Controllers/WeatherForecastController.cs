using Common.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace DockerWebApi2.Controllers;

[ApiController, Route("api/v{version:apiVersion}/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly List<string> Summaries = new List<string>
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet, Route("Weather"), ApiVersion("1.0"), ApiExplorerSettings(GroupName = "v1")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Count)]
        })
        .ToArray();
    }

    //Post add a new weather forecast summary
    [HttpPost, Route("Weather"), ApiVersion("1.0"), ApiExplorerSettings(GroupName = "v1")]
    public List<WeatherSummaryModel> Post(WeatherSummaryModel model)
    {
        Summaries.Add(model.Summary);

        return Summaries.ToList();
    }
}

