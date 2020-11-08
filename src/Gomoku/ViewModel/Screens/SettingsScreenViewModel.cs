using Cells;
using Model.Gomoku;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModel.Screens
{
    public class SettingsScreenViewModel : ScreenViewModel
    {
        public SettingsScreenViewModel(ICell<ScreenViewModel> currentScreen) : base(currentScreen)
        {
            this.BoardSize = Cell.Create(15);
            this.Capturing = Cell.Create(true);
            this.Play = new ActionCommand(PerformPlay);
        }

        public ICell<int> BoardSize { get; }

        public ICell<bool> Capturing { get; }

        public ICommand Play { get; }

        private void PerformPlay()
        {
            var game = IGame.Create(this.BoardSize.Value, this.Capturing.Value);
            CurrentScreen.Value = new PlayScreenViewModel(this.CurrentScreen, game);
        }

        public int MinimumBoardSize => IGame.MinimumBoardSize;

        public int MaximumBoardSize => IGame.MaximumBoardSize;
    }
}
