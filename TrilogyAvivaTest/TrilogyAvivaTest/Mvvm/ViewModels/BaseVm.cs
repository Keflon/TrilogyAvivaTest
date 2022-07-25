using FunctionZero.MvvmZero;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TrilogyAvivaTest.Services.Logging;

namespace TrilogyAvivaTest.Mvvm.ViewModels
{
    public abstract class BaseVm : MvvmZeroBaseVm
    {
        protected readonly ILogger _logger;

        // TODO: Specialise for all ViewModels (page and non-page) here if necessary

        public BaseVm(ILogger logger)
        {
            _logger = logger;
        }
    }
}
