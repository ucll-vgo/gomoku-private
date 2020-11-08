using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Gomoku
{
    internal class GameBoard : IGameBoard
    {
        private readonly Grid<Stone?> board;

        public const int MinimumSize = 5;

        public const int MaximumSize = 25;

        public GameBoard(int width, int height) : this(new Grid<Stone?>(width, height))
        {
            if (width < MinimumSize || width > MaximumSize)
            {
                throw new ArgumentException(nameof(width));
            }

            if (height < MinimumSize || height > MaximumSize)
            {
                throw new ArgumentException(nameof(height));
            }
        }

        private GameBoard(Grid<Stone?> board)
        {
            this.board = board;
        }

        public Stone? this[Vector2D position]
        {
            get => board[position];
            set => board[position] = value;
        }

        public int Width => board.Width;

        public int Height => board.Height;

        public GameBoard Copy() => new GameBoard(board.Copy());

        public ISet<Vector2D> FindConsecutive(Stone? stone, Vector2D position, Vector2D direction)
        {
            var result = new HashSet<Vector2D>();
            var slice = this.board.Slice(position, direction);
            var i = 0;

            while (slice.IsValidIndex(i) && slice[i] == stone)
            {
                result.Add(slice.PositionAt(i));
                ++i;
            }

            return result;
        }

        public ISet<Vector2D> SequenceAt(Vector2D position, Vector2D direction)
        {
            var owner = board[position];
            var result = new HashSet<Vector2D>();

            result.UnionWith(FindConsecutive(owner, position, direction));
            result.UnionWith(FindConsecutive(owner, position, -direction));

            return result;
        }

        public ISet<Vector2D>? FindSequenceOfFive(Vector2D position)
        {
            foreach (var direction in MainDirections)
            {
                var set = SequenceAt(position, direction);

                if (set.Count >= 5)
                {
                    return set;
                }
            }

            return null;
        }

        public ISet<Vector2D> CaptureIfPossible(Vector2D position, Vector2D direction)
        {
            var result = new HashSet<Vector2D>();
            var player = board[position];

            if (player != null)
            {
                var opponent = player.Other;
                var slice = board.Slice(position, direction);

                if (slice.IsValidIndex(0) && slice.IsValidIndex(3))
                {
                    if (slice[1] == opponent && slice[2] == opponent && slice[3] == player)
                    {
                        slice[1] = null;
                        slice[2] = null;

                        result.Add(slice.PositionAt(1));
                        result.Add(slice.PositionAt(2));
                    }
                }
            }

            return result;
        }

        public ISet<Vector2D> CaptureAt(Vector2D position)
        {
            var result = new HashSet<Vector2D>();

            foreach (var direction in Vector2D.AllDirections)
            {
                result.UnionWith(CaptureIfPossible(position, direction));
            }

            return result;
        }

        private IEnumerable<Vector2D> MainDirections
        {
            get
            {
                yield return Vector2D.NORTH;
                yield return Vector2D.NORTHEAST;
                yield return Vector2D.EAST;
                yield return Vector2D.SOUTHEAST;
            }
        }

        public ISet<Vector2D> ValidMoves => new HashSet<Vector2D>(this.board.Positions.Where(p => board[p] == null));
    }
}
