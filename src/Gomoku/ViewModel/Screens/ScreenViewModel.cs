using Cells;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.Screens
{
    public abstract class ScreenViewModel
    {
        protected ScreenViewModel(ICell<ScreenViewModel> currentScreen)
        {
            this.CurrentScreen = currentScreen;
        }

        public ICell<ScreenViewModel> CurrentScreen { get; }
    }
}
