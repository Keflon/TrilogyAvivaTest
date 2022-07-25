using FunctionZero.MvvmZero;
using System;
using System.Collections.Generic;
using System.Text;
using TrilogyAvivaTest.Services.Logging;

namespace TrilogyAvivaTest.Mvvm.ViewModels
{
    public abstract class BasePageVm : BaseVm
    {
        // TODO: Specialise for *Page* ViewModels here if necessary, e.g. responding to lifecycle events.
        public BasePageVm(ILogger logger) : base(logger)
        {
        }
        public override void OnOwnerPageAppearing()
        {
            _logger.LogLine($"{this.GetType().Name} appearing");
        }

        public override void OnOwnerPageDisappearing()
        {
            _logger.LogLine($"{this.GetType().Name} disappearing");
        }

        public override void OnOwnerPagePopped(bool isModal)
        {
            _logger.LogLine($"{this.GetType().Name} popped. Modal:{isModal}");
        }

        public override void OnOwnerPagePushed(bool isModal)
        {
            _logger.LogLine($"{this.GetType().Name} pushed. Modal:{isModal}");
        }

        public override void OnOwnerPagePushing(bool isModal)
        {
            _logger.LogLine($"{this.GetType().Name} pushing. Modal:{isModal}");
        }
    }
}
