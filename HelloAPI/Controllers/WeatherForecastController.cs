using HelloAPI.Data;

using Microsoft.AspNetCore.Mvc;

namespace HelloAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger, HelloContext context) : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult Get()
    {
        logger.LogInformation("GetWeatherForecast called");
        var test = context.Publications.ToList();
        return Ok();
    }
}
