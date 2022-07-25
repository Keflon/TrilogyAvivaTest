using FunctionZero.MvvmZero;
using System;
using TrilogyAvivaTest.Mvvm.Pages;
using TrilogyAvivaTest.Mvvm.PageViewModels;
using TrilogyAvivaTest.Services.Logging;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrilogyAvivaTest
{
    public partial class App : Application
    {
        private readonly ILogger _logger;

        public App(ILogger logger, IPageServiceZero pageService)
        {
            _logger = logger;

            InitializeComponent();

            // This app will use a standard navigation stack.
            MainPage = new NavigationPage();

            // Present the HomePage, bound to a HomePageVm.
            // Bonus points for getting the pop-culture reference here ...
            pageService.PushPageAsync<HomePage, HomePageVm>((vm) =>  vm.Init("MAIN SCREEN TURN ON"));
        }

        protected override void OnStart()
        {
            _logger.LogLine("OnStart");
        }

        protected override void OnSleep()
        {
            _logger.LogLine("OnSleep");
        }

        protected override void OnResume()
        {
            _logger.LogLine("OnResume");
        }
    }
}
