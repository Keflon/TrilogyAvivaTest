using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrilogyAvivaTest.Mvvm.ViewModels;
using TrilogyAvivaTest.Services.Logging;

namespace TrilogyAvivaTest.Mvvm.PageViewModels
{
    public class LogHistoryPageVm : BasePageVm
    {
        // Expose the protected logger to the UI.
        public ILogger Logger => base._logger;

        public LogHistoryPageVm(ILogger logger) : base(logger)
        {
            // This exists simply to relay the logger to the base class.
        }

        public void Init(string fromString)
        {
            _logger.LogLine("LogHistoryPageVm initialised by " + fromString);
        }
    }
}
