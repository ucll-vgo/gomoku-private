using Cells;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModel.Screens
{
    public class IntroductionScreenViewModel : ScreenViewModel
    {
        public IntroductionScreenViewModel(ICell<ScreenViewModel> currentScreen) : base(currentScreen)
        {
            ToMainScreen = new ActionCommand(() => currentScreen.Value = new SettingsScreenViewModel(currentScreen));
        }

        public ICommand ToMainScreen { get; }
    }
}
