using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Gomoku
{
    /// <summary>
    /// Represents a stone. A stone can be either black or white.
    /// </summary>
    public abstract class Stone
    {
        /// <summary>
        /// White stone.
        /// </summary>
        public static readonly Stone WHITE = new WhiteStone();

        /// <summary>
        /// Black stone.
        /// </summary>
        public static readonly Stone BLACK = new BlackStone();
        
        /// <summary>
        /// Opponent's stone. Black returns white and vice versa.
        /// </summary>
        public abstract Stone Other { get; }

        public class WhiteStone : Stone
        {
            public override Stone Other => Stone.BLACK;
        }

        public class BlackStone : Stone
        {
            public override Stone Other => Stone.WHITE;
        }
    }
}
