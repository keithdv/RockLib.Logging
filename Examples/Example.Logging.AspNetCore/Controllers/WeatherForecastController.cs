using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RockLib.Logging.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using RockLib.Logging;

namespace Example.Logging.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, RockLib.Logging.ILogger rockLibLogger)
        {
            _logger = logger;
            RockLibLogger = rockLibLogger;
        }

        public RockLib.Logging.ILogger RockLibLogger { get; }

        [HttpGet]
        [InfoLog]
        public IEnumerable<WeatherForecast> Get()
        {
            // Now, you would never use these next to each other but this highlights the issue

            // Overriding the default logger using a custom Logging Provider in Program
            // I would expect these two loggers to use the same underlying RockLib.Logger.ILogger
            // and get the same correlation id
            // But they do not

            _logger.LogInformation("Microsoft Logger");
            RockLibLogger.Info("RockLib Logger");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
