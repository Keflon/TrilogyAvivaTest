using System;
using System.Collections.Generic;
using System.Text;
using TrilogyAvivaTest.Models;
using TrilogyAvivaTest.Mvvm.ViewModels;
using TrilogyAvivaTest.Services.Logging;

namespace TrilogyAvivaTest.Mvvm.PageViewModels
{
    public class CityWeatherPageVm : BasePageVm
    {
        private WeatherResponse _cityWeather;
        public WeatherResponse CityWeather
        {
            get => _cityWeather;
            set => SetProperty(ref _cityWeather, value);
        }

        public double AbsoluteZero => -273.15;

        public CityWeatherPageVm(ILogger logger) : base(logger)
        {

        }

        public void Init(WeatherResponse weather)
        {
            CityWeather = weather;
        }
    }
}
