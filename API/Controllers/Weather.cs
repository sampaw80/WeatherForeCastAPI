using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : BaseApiController
    {
        private readonly IOptions<AppOptions> _options;
        public WeatherController(IOptions<AppOptions> options)
        {
            _options = options;
        }

        [HttpGet("{city}")]
        [AllowAnonymous]
        public async Task<IActionResult> City(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=9fcb9207f55491aa4c10f55d65ee20eb&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                    return Ok(new
                    {
                        Temp = rawWeather.Main.Temp,
                        Summary = string.Join(",", rawWeather.Weather.Select(x => x.Main)),
                        City = rawWeather.Name
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }

            } 
        }

        public class AppOptions
        {
            public string OpenWeatherApiKey { get; set; }
        }
        public class OpenWeatherResponse
        {
            public string Name { get; set; }

            public IEnumerable<WeatherDescription> Weather { get; set; }

            public Main Main { get; set; }
        }

        public class WeatherDescription
        {
            public string Main { get; set; }
            public string Description { get; set; }
        }
        public class Main
        {
            public string Temp { get; set; }
        }
    }
}