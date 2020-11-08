using Cells;
using Model.Data;
using Model.Gomoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using static System.Linq.Enumerable;

namespace ViewModel
{
    public class GameViewModel
    {
        private readonly ICell<IGame> game;

        public GameViewModel(IGame game)
        {
            this.game = Cell.Create(game);

            var width = game.Board.Width;
            var height = game.Board.Height;
            this.Rows = from row in Range(0, height)
                        select (from col in Range(0, width)
                                let pos = new Vector2D(col, row)
                                select new BoardSquareViewModel(this.game, pos));

            this.CurrentPlayer = this.game.Derive(g => g.IsGameOver ? null : g.CurrentPlayer);
            this.IsGameOver = this.game.Derive(g => g.IsGameOver);
            this.Winner = this.game.Derive(g => g.IsGameOver ? g.Winner : null);
        }

        public IEnumerable<IEnumerable<BoardSquareViewModel>> Rows { get; }

        public ICell<Stone> CurrentPlayer { get; }

        public ICell<bool> IsGameOver { get; }

        public ICell<Stone> Winner { get; }
    }

    public class BoardSquareViewModel
    {
        private readonly ICell<IGame> game;

        public BoardSquareViewModel(ICell<IGame> game, Vector2D position)
        {
            this.game = game;
            this.Position = position;
            this.IsValidMove = this.game.Derive(g => !g.IsGameOver && g.IsValidMove(position));
            this.IsCaptured = this.game.Derive(g => !g.IsGameOver && g.CapturedPositions.Contains(position));
            this.PutStone = new ActionCommand(PerformPutStone, this.IsValidMove);
            this.Contents = this.game.Derive(g => g.Board[position]);
            this.IsWinningSquare = this.game.Derive(g => g.IsGameOver && g.WinningPositions.Contains(position));
        }
        
        public Vector2D Position { get; }

        public ICell<Stone> Contents { get; }

        public ICell<bool> IsValidMove { get; }

        public ICell<bool> IsCaptured { get; }        

        public ICell<bool> IsWinningSquare { get; }

        public ICommand PutStone { get; }


        private void PerformPutStone()
        {
            game.Update(g => g.PutStone(this.Position));
        }
    }
}
