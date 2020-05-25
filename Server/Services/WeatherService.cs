using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using MyWeather;

namespace Server
{
    public class WeatherService : MyWeather.WeatherService.WeatherServiceBase
    {
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(ILogger<WeatherService> logger)
        {
            _logger = logger;
        }

        public override Task<WeatherDataReply> RequestCurrentWeatherData(WeatherRequest request,
            ServerCallContext context)
        {
            return Task.FromResult(GetWeatherData(request));
        }

        private WeatherDataReply GetWeatherData(WeatherRequest request)
        {
            var rnd = new Random((int) DateTime.Now.Ticks);

            return new WeatherDataReply
            {

                Temperature = rnd.Next(10) + 65,
                Location = request.Location,
                Windspeed = rnd.Next(10),
                Winddirection = rnd.Next(360)

            };
        }

        public override Task<WeatherHistoricReply> RequestHistoricData(WeatherRequest request,
            ServerCallContext context)
        {
            var list = new List<WeatherDataReply>();
            Enumerable.Range(0, 10).ToList().ForEach(arg => list.Add(GetWeatherData(request)));

            var reply = new WeatherHistoricReply
            {
                Data = {list}
            };
            return Task.FromResult(reply);
        }
        
    }
}