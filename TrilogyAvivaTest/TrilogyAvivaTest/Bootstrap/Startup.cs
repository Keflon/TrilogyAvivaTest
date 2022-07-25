﻿using FunctionZero.MvvmZero;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;
using TrilogyAvivaTest.Mvvm.Pages;
using TrilogyAvivaTest.Mvvm.PageViewModels;
using TrilogyAvivaTest.Services.Logging;
using TrilogyAvivaTest.Services.Persistence;

namespace TrilogyAvivaTest.Bootstrap
{
    public class Startup
    {
        private Container _IoCC;
        public Startup()
        {
            // Create the IoC container that will contain all our configurable classes ...
            _IoCC = new Container();

            // Tell the IoC container about our App. instance.
            _IoCC.Register<App>(Lifestyle.Singleton);

            // Tell the IoC container what to do if asked for an IPageService
            _IoCC.Register<IPageServiceZero>(CreatePageService, Lifestyle.Singleton);

            // Tell the IoC container about our Pages.
            _IoCC.Register<HomePage>(Lifestyle.Singleton);

            // Tell the IoC container about our ViewModels.
            _IoCC.Register<HomePageVm>(Lifestyle.Singleton);

            // Tell the IoC container about our Services.
            _IoCC.Register<ILogger, DebugLogger>(Lifestyle.Singleton);
            _IoCC.Register<IKeyStore, KeyStore>(Lifestyle.Singleton);
        }

        private IPageServiceZero CreatePageService()
        {
            // This is how we create an instance of PageServiceZero.
            // The PageService needs to know how to get the current NavigationPage it is to interact with.
            // (If you have a FlyoutPage at the root, the navigationGetter should return the current Detail item)
            // It also needs to know how to get Page and ViewModel instances so we provide it with a factory
            // that uses the IoC container. We could easily provide any sort of factory, we don't need to use an IoC container.
            var pageService = new PageServiceZero(() => App.Current.MainPage.Navigation, (theType) => _IoCC.GetInstance(theType));
            return pageService;
        }

        public App GetApplicationInstance() => _IoCC.GetInstance<App>();
    }
}