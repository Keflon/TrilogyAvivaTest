using FunctionZero.MvvmZero;
using System;
using System.Threading.Tasks;
using TrilogyAvivaTest.Bootstrap;
using TrilogyAvivaTest.Mvvm.Pages;
using TrilogyAvivaTest.Mvvm.PageViewModels;
using TrilogyAvivaTest.Services.Logging;
using TrilogyAvivaTest.Services.Persistence;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrilogyAvivaTest
{
    public partial class App : Application
    {
        private readonly ILogger _logger;

        public App(ILogger logger, IKeyStore keyStore, IPageServiceZero pageService)
        {
            _logger = logger;

            InitializeComponent();

            pageService.Init(this);

            // This app will use a standard navigation stack.

            if (Device.RuntimePlatform == Device.iOS)
                // iOS cannot initalise a NavigationPage without a root page provided up front.
                // Providing a HomePage prevents the OnOwnerPagePushed event being raised in HomePageVm
                // So I'm just jimmying in a dud page at the root and disabling the back-button in HomePage.xaml because this is just a demo.
                MainPage = new NavigationPage(new ContentPage() { Content=new Label { Text="Don't look behind the curtain!"} });
            else
                MainPage = new NavigationPage();

            // Present the HomePage, bound to a HomePageVm.
            pageService.PushPageAsync<HomePage, HomePageVm>(async (vm) => await InitialiseTheVm(vm, keyStore));
        }

        private async Task InitialiseTheVm(HomePageVm vm, IKeyStore keyStore)
        {
            // Determine how many times the app has been launched from a cold-start and inform the vm.
            var result = await keyStore.ReadStringAsync(Constants.RunCountKey);
            int runCount;
            if (result == null)
            {
                runCount = 1;
            }
            else
            {
                if (int.TryParse(result, out runCount) == false)
                    runCount = 1;
            }

            await keyStore.WriteStringAsync(Constants.RunCountKey, (runCount + 1).ToString());

            vm.Init(runCount);
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
