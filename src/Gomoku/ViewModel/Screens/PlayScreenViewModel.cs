using Cells;
using Model.Gomoku;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModel.Screens
{
    public class PlayScreenViewModel : ScreenViewModel
    {
        public PlayScreenViewModel(ICell<ScreenViewModel> currentScreen, IGame game) : base(currentScreen)
        {
            this.Game = new GameViewModel(game);
            this.PlayAgain = new ActionCommand(() => this.CurrentScreen.Value = new SettingsScreenViewModel(this.CurrentScreen));
        }

        public GameViewModel Game { get; }

        public ICommand PlayAgain { get; }
    }
}
