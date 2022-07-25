using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TrilogyAvivaTest.Bootstrap;
using TrilogyAvivaTest.Models;
using TrilogyAvivaTest.Mvvm.Pages;
using TrilogyAvivaTest.Mvvm.ViewModels;
using TrilogyAvivaTest.Services.Alert;
using TrilogyAvivaTest.Services.Api;
using TrilogyAvivaTest.Services.Logging;
using TrilogyAvivaTest.Services.Persistence;

namespace TrilogyAvivaTest.Mvvm.PageViewModels
{
    internal class HomePageVm : BasePageVm
    {
        private readonly IKeyStore _keyStore;
        private readonly OpenWeatherService _weatherService;
        private readonly IPageServiceZero _pageService;
        private readonly IAlertService _alertService;
        private string _cityName;
        private bool _isCitySaved;
        private string _cityNamePlaceholder;
        private int _runCount;
        private string _savedCityName;

        public int RunCount
        {
            get => _runCount;
            set => SetProperty(ref _runCount, value);
        }

        public string CityName
        {
            get => _cityName;
            set => SetProperty(ref _cityName, value);
        }

        public string CityNamePlaceholder
        {
            get => _cityNamePlaceholder;
            set => SetProperty(ref _cityNamePlaceholder, value);
        }

        public bool IsCitySaved
        {
            get => _isCitySaved;
            set => SetProperty(ref _isCitySaved, value);
        }

        //public string WeatherDescription
        //{
        //    get => _weatherDescription;
        //    set => SetProperty(ref _weatherDescription, value);
        //}

        public CommandZeroAsync ResetRunCountCommand { get; }
        public CommandZeroAsync GetCityWeatherCommand { get; }
        public CommandZeroAsync SaveCityNameCommand { get; }
        public CommandZeroAsync ResetCityNameCommand { get; }

        public HomePageVm(
            ILogger logger,
            IKeyStore keyStore,
            OpenWeatherService weatherService,
            IPageServiceZero pageService,
            IAlertService alertService) : base(logger)
        {
            _keyStore = keyStore;
            _weatherService = weatherService;
            _pageService = pageService;
            _alertService = alertService;

            // CityName is retrieved from backing store in OnOwnerPagePushed.
            // Setting it here so it does not need to be null-checked.
            CityName = string.Empty;

            ResetRunCountCommand = new CommandBuilder().AddGuard(this).SetExecute(ResetRunCount).SetName("Reset").Build();
            GetCityWeatherCommand = new CommandBuilder().AddGuard(this).SetExecuteAsync(GetCityWeatherAsync).SetName(GetGoText).SetCanExecute(GetIsCityValid).AddObservedProperty(this, nameof(CityName)).Build();
            SaveCityNameCommand = new CommandBuilder().AddGuard(this).SetExecuteAsync(SaveCityAsync).SetName("Save").SetCanExecute(CanSaveCity).AddObservedProperty(this, nameof(CityName)).Build();
            ResetCityNameCommand = new CommandBuilder().AddGuard(this).SetExecute(ResetCity).SetName("Reset").SetCanExecute(() => IsCitySaved).AddObservedProperty(this, nameof(IsCitySaved)).Build();
        }

        private string GetGoText()
        {
            if (GetIsCityValid())
                return "Get Weather for " + CityName;

            return "Waiting ...";
        }

        private bool CanSaveCity()
        {
            return GetIsCityValid() && (CityName != _savedCityName);
        }

        private void ResetRunCount()
        {
            _keyStore.Delete(Constants.RunCountKey);
            RunCount = 0;
        }
        private async Task GetCityWeatherAsync()
        {
            var result = await _weatherService.GetWeatherAsync(CityName);
            switch (result.status)
            {
                case Services.Rest.ResultStatus.Success:
                    if (CityName == _savedCityName)
                    {
                        CityName = result.payload.Name;
                        if (IsCitySaved)
                            await SaveCityNameCommand?.ExecuteAsync(null);
                    }
                    await _pageService.PushPageAsync<CityWeatherPage, CityWeatherPageVm>((vm) => vm.Init(result.payload));
                    break;
                case Services.Rest.ResultStatus.ConnectionFailed:
                    await _alertService.DisplayAlertAsync("Something awful happened", "Please check your connection and try again", "OK");
                    break;
                case Services.Rest.ResultStatus.Unauthorized:
                    await _alertService.DisplayAlertAsync("Something awful happened", "Please check the API key and try again", "OK");
                    break;
                case Services.Rest.ResultStatus.BadResponse:
                    await _alertService.DisplayAlertAsync("Something awful happened", "The server may be having a wobble. Please try again later", "OK");
                    break;
                case Services.Rest.ResultStatus.BadPayload:
                    await _alertService.DisplayAlertAsync("Something awful happened", "The server may be having a wobble. Please try again later", "OK");
                    break;
                case Services.Rest.ResultStatus.Other:
                    await _alertService.DisplayAlertAsync("Something awful happened", "That's all we know. Maybe try again later?", "OK");
                    break;
            }
        }

        private bool GetIsCityValid()
        {
            // TODO: Validate the CityName properly!
            return CityName.Length > 3;
        }

        private async Task SaveCityAsync()
        {
            IsCitySaved = await _keyStore.WriteStringAsync(Constants.CityNameKey, CityName);
        }
        private void ResetCity()
        {
            _keyStore.Delete(Constants.CityNameKey);
            IsCitySaved = false;
            CityName = String.Empty;
        }

        internal void Init(int runCount)
        {
            _logger.LogLine($"Run count is {runCount}");
            RunCount = runCount;
        }

        public override async void OnOwnerPagePushed(bool isModal)
        {
            base.OnOwnerPagePushed(isModal);
            CityName = await _keyStore.ReadStringAsync(Constants.CityNameKey) ?? String.Empty;
            IsCitySaved = !string.IsNullOrEmpty(CityName);

            // TODO: Use an API call to get a nearby city.
            CityNamePlaceholder = "E.g. Liverpool";

            if (RunCount == 1)
                await _alertService.DisplayAlertAsync("First Run!", "Hello. This is the first time you have launched the app. Hope you like it :)", "Dismiss");
        }


        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsCitySaved))
            {
                if (IsCitySaved)
                    _savedCityName = CityName;
                else
                    _savedCityName = String.Empty;
            }
        }
    }
}
