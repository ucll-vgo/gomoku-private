using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Model.Data
{
    public class Vector2D : IEquatable<Vector2D>
    {
        public Vector2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public override string ToString() => $"({X}, {Y})";

        public bool Equals(Vector2D? other) => other != null && this.X == other.X && this.Y == other.Y;

        public override bool Equals(object? obj) => Equals(obj as Vector2D);

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

        public static Vector2D operator +(Vector2D u, Vector2D v) => new Vector2D(u.X + v.X, u.Y + v.Y);

        public static Vector2D operator -(Vector2D v) => new Vector2D(-v.X, -v.Y);

        public static Vector2D operator -(Vector2D u, Vector2D v) => new Vector2D(u.X - v.X, u.Y - v.Y);

        public static Vector2D operator *(Vector2D v, int f) => new Vector2D(v.X * f, v.Y * f);

        public static Vector2D operator *(int f, Vector2D v) => v * f;

        public static readonly Vector2D NORTH = new Vector2D(0, 1);

        public static readonly Vector2D SOUTH = new Vector2D(0, -1);

        public static readonly Vector2D EAST = new Vector2D(1, 0);

        public static readonly Vector2D WEST = new Vector2D(-1, 0);

        public static readonly Vector2D NORTHEAST = NORTH + EAST;

        public static readonly Vector2D SOUTHEAST = SOUTH + EAST;

        public static readonly Vector2D NORTHWEST = NORTH + WEST;

        public static readonly Vector2D SOUTHWEST = SOUTH + WEST;

        public static IEnumerable<Vector2D> AllDirections
        {
            get
            {
                yield return NORTH;
                yield return NORTHEAST;
                yield return EAST;
                yield return SOUTHEAST;
                yield return SOUTH;
                yield return SOUTHWEST;
                yield return WEST;
                yield return NORTHWEST;
            }
        }
    }
}
