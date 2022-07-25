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

            // This app will use a standard navigation stack.
            MainPage = new NavigationPage();

            // Present the HomePage, bound to a HomePageVm.
            // Bonus points for getting the pop-culture reference here ...
            pageService.PushPageAsync<HomePage, HomePageVm>(async (vm)=> await InitialiseTheVm(vm, keyStore));
        }

        private async Task InitialiseTheVm(HomePageVm vm, IKeyStore keyStore)
        {
            // Determine how many times the app has been launched from a cold-start and inform the vm.
            var result = await keyStore.ReadStringAsync(Constants.RunCountKey);
            int runCount;
            if(result == null)
            {
                runCount = 0;
            }
            else
            {
                if (int.TryParse(result, out runCount) == false)
                    runCount = 0;
            }

            await keyStore.WriteStringAsync(Constants.RunCountKey, runCount.ToString());

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
