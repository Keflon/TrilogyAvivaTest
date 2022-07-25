using FunctionZero.CommandZero;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrilogyAvivaTest.Bootstrap;
using TrilogyAvivaTest.Models;
using TrilogyAvivaTest.Mvvm.ViewModels;
using TrilogyAvivaTest.Services.Api;
using TrilogyAvivaTest.Services.Logging;
using TrilogyAvivaTest.Services.Persistence;

namespace TrilogyAvivaTest.Mvvm.PageViewModels
{
    internal class HomePageVm : BasePageVm
    {
        private readonly IKeyStore _keyStore;
        private readonly OpenWeatherService _weatherService;
        private string _cityName;
        private bool _isCitySaved;
        private WeatherResponse _cityWeather;

        public int RunCount { get; private set; }

        public string CityName
        {
            get => _cityName;
            set => SetProperty(ref _cityName, value);
        }

        public bool IsCitySaved
        {
            get => _isCitySaved;
            set => SetProperty(ref _isCitySaved, value);
        }
        public WeatherResponse CityWeather
        {
            get => _cityWeather;
            set => SetProperty(ref _cityWeather, value);
        }

        public ICommand ResetRunCountCommand { get; }
        public ICommand GetCityWeatherCommand { get; }
        public ICommand SaveCityNameCommand { get; }
        public ICommand ResetCityNameCommand { get; }

        public HomePageVm(ILogger logger, IKeyStore keyStore, OpenWeatherService weatherService) : base(logger)
        {
            _keyStore = keyStore;
            _weatherService = weatherService;

            ResetRunCountCommand = new CommandBuilder().AddGuard(this).SetExecute(ResetRunCount).SetName("Reset").Build();
            GetCityWeatherCommand = new CommandBuilder().AddGuard(this).SetExecuteAsync(GetCityWeatherAsync).SetName("GO!").SetCanExecute(GetIsCityValid).AddObservedProperty(this, nameof(CityName)).Build();
            SaveCityNameCommand = new CommandBuilder().AddGuard(this).SetExecuteAsync(SaveCityAsync).SetName("Save").SetCanExecute(GetIsCityValid).AddObservedProperty(this, nameof(CityName)).Build();
            ResetCityNameCommand = new CommandBuilder().AddGuard(this).SetExecute(ResetCity).SetName("Reset").SetCanExecute(()=> IsCitySaved).AddObservedProperty(this, nameof(IsCitySaved)).Build();
        }

        private void ResetRunCount()
        {
             _keyStore.Delete(Constants.RunCountKey);
        } 
        private async Task GetCityWeatherAsync()
        {
            var result = await _weatherService.GetWeatherAsync(CityName);
            switch (result.status)
            {
                case Services.Rest.ResultStatus.Success:
                    break;
                case Services.Rest.ResultStatus.ConnectionFailed:
                    break;
                case Services.Rest.ResultStatus.Unauthorized:
                    break;
                case Services.Rest.ResultStatus.BadResponse:
                    break;
                case Services.Rest.ResultStatus.BadPayload:
                    break;
                case Services.Rest.ResultStatus.Other:
                    break;
            }
        }
        private bool GetIsCityValid()
        {
            // TODO: Validate the CityName property
            return true;
        }
        private async Task SaveCityAsync()
        {
            await _keyStore.WriteStringAsync(Constants.CityNameKey, CityName);
            IsCitySaved = true;
        }
        private void ResetCity()
        {
            _keyStore.Delete(Constants.CityNameKey);
            IsCitySaved = false;
        }



        internal void Init(int runCount)
        {
            _logger.LogLine($"Run count is {runCount}");
            RunCount = runCount;
        }

        public override async void OnOwnerPageAppearing()
        {
            base.OnOwnerPageAppearing();

            CityName = await _keyStore.ReadStringAsync(Constants.CityNameKey);
            IsCitySaved = !string.IsNullOrEmpty(CityName);
        }
    }
}
