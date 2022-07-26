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
    public class HomePageVm : BasePageVm
    {
        private readonly IKeyStore _keyStore;
        private readonly OpenWeatherService _weatherService;
        private readonly IPageServiceZero _pageService;
        private readonly IAlertService _alertService;
        private string _cityName;
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
        public string SavedCityName
        {
            get => _savedCityName;
            set => SetProperty(ref _savedCityName, value);
        }

        public string CityNamePlaceholder
        {
            get => _cityNamePlaceholder;
            set => SetProperty(ref _cityNamePlaceholder, value);
        }

        // Concrete type is preferred here so x:DataType works in xaml for the ~.Text property.
        public CommandZeroAsync ResetRunCountCommand { get; }
        public CommandZeroAsync GetCityWeatherCommand { get; }
        public CommandZeroAsync SaveCityNameCommand { get; }
        public CommandZeroAsync LoadCityNameCommand { get; }
        public CommandZeroAsync ResetCityNameCommand { get; }
        public CommandZeroAsync ShowLogCommand { get; }

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
            SavedCityName = string.Empty;

            ResetRunCountCommand = new CommandBuilder().AddGuard(this).SetExecute(ResetRunCount).SetName("Reset").Build();
            GetCityWeatherCommand = new CommandBuilder().AddGuard(this).SetExecuteAsync(GetCityWeatherAsync).SetName(GetGetCityWeatherCommandText).SetCanExecute(GetIsCityValid).AddObservedProperty(this, nameof(CityName)).Build();
            SaveCityNameCommand = new CommandBuilder().AddGuard(this).SetExecuteAsync(SaveCityAsync).SetName("Save").SetCanExecute(CanSaveCity).AddObservedProperty(this, nameof(CityName)).Build();
            LoadCityNameCommand = new CommandBuilder().AddGuard(this).SetExecute(()=>CityName = SavedCityName).SetName("Load").SetCanExecute(CanLoadCity).AddObservedProperty(this, nameof(CityName), nameof(SavedCityName)).Build();
            ResetCityNameCommand = new CommandBuilder().AddGuard(this).SetExecute(ResetCity).SetName("Reset").SetCanExecute(() => SavedCityName != string.Empty).AddObservedProperty(this, nameof(SavedCityName)).Build();
            ShowLogCommand = new CommandBuilder().AddGuard(this).SetExecuteAsync(ShowLogCommandExecuteAsync).SetName("Show Log").Build();
        }

        private async Task ShowLogCommandExecuteAsync()
        {
            await _pageService.PushPageAsync<LogHistoryPage, LogHistoryPageVm>((vm) =>  vm.Init("From HomePageVm"));
        }

        private string GetGetCityWeatherCommandText()
        {
            if (GetIsCityValid())
                return "Get Weather for " + CityName;

            return "Waiting ...";
        }

        public bool CanSaveCity()
        {
            return GetIsCityValid() && (CityName != SavedCityName);
        }

        private bool CanLoadCity()
        {
            return (SavedCityName!= string.Empty) &&(SavedCityName != CityName);
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
                    // result.payload.Name may hold a 'corrected' spelling for CityName
                    // If an incorrect spelling is 'saved', replace it with the correct spelling.
                    bool nameIsSaved = CityName == _savedCityName;
                    if(CityName != result.payload.Name)
                    {
                        CityName = result.payload.Name;
                        if(nameIsSaved)
                            await SaveCityAsync();
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
                    await _alertService.DisplayAlertAsync("Oops!", "Unknown location", "OK");
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
            if(await _keyStore.WriteStringAsync(Constants.CityNameKey, CityName))
                SavedCityName = CityName;
            else
                SavedCityName = string.Empty;
        }

        private void ResetCity()
        {
            _keyStore.Delete(Constants.CityNameKey);
            SavedCityName = string.Empty;
            CityName = string.Empty;
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
            SavedCityName = CityName;

            // TODO: Use an API call to get a nearby city.
            CityNamePlaceholder = "E.g. Liverpool";

            if (RunCount == 1)
                await _alertService.DisplayAlertAsync(
                    "First Run!", 
                    "Hello. This is the first time you have launched the app. Hope you like it :)", 
                    "Dismiss"
                    );
        }
    }
}
