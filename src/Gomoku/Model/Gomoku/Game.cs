using Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Model.Gomoku
{
    /// <summary>
    /// Represents a game of Gomoku.
    /// Objects of this type are immutable.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Static factory function to create new games.
        /// </summary>
        /// <param name="boardSize">Size of the board. See <see cref="MinimumBoardSize"/> and <see cref="MaximumBoardSize"/> for the allowed range.</param>
        /// <param name="capturing">Whether capturing stones is allowed.</param>
        /// <returns></returns>
        public static IGame Create(int boardSize, bool capturing)
        {
            return new InProgressGame(boardSize, capturing);
        }

        /// <summary>
        /// Current player.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the game is over.</exception>
        Stone CurrentPlayer { get; }

        /// <summary>
        /// Winner of the game.
        /// Has the value null in case of a tie.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the game is not yet over.</exception>
        Stone? Winner { get; }

        /// <summary>
        /// Game board.
        /// </summary>
        IGameBoard Board { get; }

        /// <summary>
        /// Returns true if the game is over, false otherwise.
        /// </summary>
        bool IsGameOver { get; }

        /// <summary>
        /// Adds a stone to the board.
        /// The color is determined by <see cref="CurrentPlayer" />.
        /// </summary>
        /// <param name="position">Where to place the new stone.</param>
        /// <returns>A new game object with the updated board.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the game is over.</exception>
        IGame PutStone(Vector2D position);

        /// <summary>
        /// Checks if placing a stone on <paramref name="position" /> is possible.
        /// </summary>
        /// <param name="position">Where to place the new stone.</param>
        /// <returns>True if the stone can be placed at <paramref name="position"/>, false otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the game is over.</exception>
        bool IsValidMove(Vector2D position);

        /// <summary>
        /// Returns the positions that have been captured by the previous move.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the game is over.</exception>
        ISet<Vector2D> CapturedPositions { get; }

        /// <summary>
        /// Returns the positions that make up the winning sequence of five.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the game is not yet over.</exception>
        ISet<Vector2D> WinningPositions { get; }

        /// <summary>
        /// Minimum board size.
        /// </summary>
        public const int MinimumBoardSize = GameBoard.MinimumSize;

        /// <summary>
        /// Maximum board size.
        /// </summary>
        public const int MaximumBoardSize = GameBoard.MaximumSize;
    }

    /// <summary>
    /// Represents a game board.
    /// </summary>
    public interface IGameBoard
    {
        /// <summary>
        /// Width of the board.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of the board.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Returns the stone at <paramref name="position"/>.
        /// Usage: <code>board[pos]</code>.
        /// </summary>
        /// <param name="position">Position of the stone to return.</param>
        /// <returns>Stone (or null) at <paramref name="position"/></returns>
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
