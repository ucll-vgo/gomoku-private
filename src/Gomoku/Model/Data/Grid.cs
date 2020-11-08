using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Model.Data
{
    internal class Grid<T>
    {
        private readonly T[][] grid;

        public Grid(int width, int height, Func<Vector2D, T> initializer)
        {
            if ( width <= 0 )
            {
                throw new ArgumentException("Should be at least 1", nameof(width));
            }

            if ( height <= 0 )
            {
                throw new ArgumentException("Should be at least 1", nameof(height));
            }

            this.grid = Enumerable.Range(0, height).Select(CreateRow).ToArray();


            T[] CreateRow(int row)
            {
                return Enumerable.Range(0, width).Select(col => CreateCell(row, col)).ToArray();
            }

            T CreateCell(int row, int column)
            {
                var position = new Vector2D(column, row);

                return initializer(position);
            }
        }

        public Grid(int width, int height, T initial = default) : this(width, height, _ => initial)
        {
            // NOP
        }

        public T this[Vector2D position]
        {
            get
            {
                return grid[position.Y][position.X];
            }
            set
            {
                grid[position.Y][position.X] = value;
            }
        }

        public int Width => this.grid[0].Length;

        public int Height => this.grid.Length;

        public Grid<T> Copy()
        {
            return new Grid<T>(this.Width, this.Height, p => this[p]);
        }

        public bool IsValidPosition(Vector2D position)
        {
            return 0 <= position.X && position.X < Width && 0 <= position.Y && position.Y < Height;
        }

        public GridSlice<T> Row(int rowIndex) => new GridSlice<T>(this, new Vector2D(0, rowIndex), Vector2D.EAST);

        public GridSlice<T> Column(int columnIndex) => new GridSlice<T>(this, new Vector2D(columnIndex, 0), Vector2D.NORTH);

        public IEnumerable<GridSlice<T>> Rows => Enumerable.Range(0, Width).Select(Row);

        public IEnumerable<GridSlice<T>> Columns => Enumerable.Range(0, Width).Select(Column);

        public GridSlice<T> Slice(Vector2D startPosition, Vector2D direction)
        {
            return new GridSlice<T>(this, startPosition, direction);
        }

        public IEnumerable<Vector2D> Positions
        {
            get
            {
                return from y in Enumerable.Range(0, Height)
                       from x in Enumerable.Range(0, Width)
                       select new Vector2D(x, y);
            }
        }        
    }

    internal class GridSlice<T>
    {
        private readonly Grid<T> grid;

        private readonly Vector2D position;

        private readonly Vector2D direction;

        public GridSlice(Grid<T> grid, Vector2D position, Vector2D direction)
        {
            this.grid = grid;
            this.position = position;
            this.direction = direction;
        }

        public T this[int index]
        {
            get => grid[PositionAt(index)];
            set => grid[PositionAt(index)] = value;
        }

        public bool IsValidIndex(int index)
        {
            return this.grid.IsValidPosition(PositionAt(index));
        }

        public Vector2D PositionAt(int index)
        {
            return position + index * direction;
        }
    }    
}
