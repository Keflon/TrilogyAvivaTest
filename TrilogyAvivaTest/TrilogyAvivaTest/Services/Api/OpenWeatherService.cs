using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrilogyAvivaTest.Bootstrap;
using TrilogyAvivaTest.Models;
using TrilogyAvivaTest.Services.Rest;

namespace TrilogyAvivaTest.Services.Api
{
    public class OpenWeatherService
    {
        private readonly IRestService _restService;
        private readonly string _apiPath;

        public OpenWeatherService(IRestService restService, string apiPath)
        {
            _restService = restService;
            _apiPath = apiPath;
        }

        public async Task<(ResultStatus status, WeatherResponse payload, string rawResponse)> GetWeatherAsync(string city)
        {
            return await _restService.GetAsync<WeatherResponse>(_apiPath + $"?q={city}&APPID={ApiConstants.ApiKey}");
        }
    }
}