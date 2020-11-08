using Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Model.Gomoku
{
    public interface IGame
    {
        public static IGame Create(int boardSize, bool capturing)
        {
            return new InProgressGame(boardSize, capturing);
        }

        Stone CurrentPlayer { get; }

        Stone? Winner { get; }

        IGameBoard Board { get; }

        bool IsGameOver { get; }

        IGame PutStone(Vector2D position);

        bool IsValidMove(Vector2D position);

        ISet<Vector2D> CapturedPositions { get; }

        ISet<Vector2D> WinningPositions { get; }

        public const int MinimumBoardSize = GameBoard.MinimumSize;

        public const int MaximumBoardSize = GameBoard.MaximumSize;
    }

    public interface IGameBoard
    {
        int Width { get; }

        int Height { get; }

        Stone? this[Vector2D position] { get; }
    }

    internal abstract class Game : IGame
    {
        protected Game(GameBoard gameBoard)
        {
            this.Board = gameBoard;
        }

        public abstract Stone? Winner { get; }

        public abstract Stone CurrentPlayer { get; }

        IGameBoard IGame.Board => this.Board;

        public GameBoard Board { get; }

        public abstract bool IsGameOver { get; }

        public abstract IGame PutStone(Vector2D position);

        public abstract bool IsValidMove(Vector2D position);

        public abstract ISet<Vector2D> CapturedPositions { get; }

        public abstract ISet<Vector2D> WinningPositions { get; }
    }

    internal class InProgressGame : Game
    {
        private readonly bool capturing;

        private readonly ISet<Vector2D> capturedPositions;

        public InProgressGame(int boardSize, bool capturing) : this(Stone.BLACK, new GameBoard(boardSize, boardSize), capturing, new HashSet<Vector2D>())
        {
            this.capturing = capturing;
        }

        protected InProgressGame(Stone currentPlayer, GameBoard gameBoard, bool capturing, ISet<Vector2D> capturedPositions) : base(gameBoard)
        {
            this.CurrentPlayer = currentPlayer;
            this.capturedPositions = capturedPositions;
            this.capturing = capturing;
        }

        public override bool IsGameOver => false;

        public override Stone? Winner => throw new InvalidOperationException("Game still in progress; no winner yet");

        public override Stone CurrentPlayer { get; }

        public override IGame PutStone(Vector2D position)
        {
            if ( !IsValidMove(position) )
            {
                throw new InvalidOperationException("Invalid move");
            }

            var nextPlayer = this.CurrentPlayer.Other;
            var nextBoard = this.Board.Copy();
            nextBoard[position] = this.CurrentPlayer;

            var capturedPositions = this.capturing ? nextBoard.CaptureAt(position) : new HashSet<Vector2D>();
            var fiveInARowPositions = nextBoard.FindSequenceOfFive(position);
            var validMoves = nextBoard.ValidMoves;
            validMoves.ExceptWith(capturedPositions);
            var validMovesLeft = validMoves.Count > 0;

            if (fiveInARowPositions != null)
            {
                return new FinishedGame(this.CurrentPlayer, nextBoard, fiveInARowPositions);
            }
            else if ( !validMovesLeft )
            {
                return new FinishedGame(null, nextBoard, new HashSet<Vector2D>());
            }
            else
            {
                return new InProgressGame(nextPlayer, nextBoard, capturing, capturedPositions);
            }
        }

        public override bool IsValidMove(Vector2D position)
        {
            return Board[position] == null && !this.capturedPositions.Contains(position);
        }

        public override ISet<Vector2D> CapturedPositions => new HashSet<Vector2D>(capturedPositions);

        public override ISet<Vector2D> WinningPositions => throw new InvalidOperationException("Game not over yet");
    }

    internal class FinishedGame : Game
    {
        private readonly ISet<Vector2D> winningPositions;

        public FinishedGame(Stone? winner, GameBoard gameBoard, ISet<Vector2D> winningPositions) : base(gameBoard)
        {
            this.Winner = winner;
            this.winningPositions = winningPositions;
        }

        public override bool IsGameOver => true;

        public override Stone? Winner { get; }

        public override Stone CurrentPlayer => throw new InvalidOperationException("Game over");

        public override IGame PutStone(Vector2D position) => throw new InvalidOperationException("Game finished");

        public override bool IsValidMove(Vector2D position) => throw new InvalidOperationException("Game finished");

        public override ISet<Vector2D> CapturedPositions => throw new InvalidOperationException("Game finished");

        public override ISet<Vector2D> WinningPositions => new HashSet<Vector2D>(this.winningPositions);
    }
}
