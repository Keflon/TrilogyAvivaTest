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
        public HomePageVm(ILogger logger) : base(logger)
        {
        }

        internal void Init(int runCount)
        {
            _logger.LogLine($"Run count is {runCount}");
        }
    }
}
