using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrilogyAvivaTest.Mvvm.ViewModels;
using TrilogyAvivaTest.Services.Logging;

namespace TrilogyAvivaTest.Mvvm.PageViewModels
{
    internal class HomePageVm : BasePageVm
    {
        private readonly ILogger _logger;

        public HomePageVm(ILogger logger)
        {
            _logger = logger;
        }

        internal void Init(string initMessage)
        {
            _logger.LogLine(initMessage);
        }
    }
}
