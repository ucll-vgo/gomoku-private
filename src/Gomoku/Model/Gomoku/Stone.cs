using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Gomoku
{
    public abstract class Stone
    {
        public static readonly Stone WHITE = new WhiteStone();

        public static readonly Stone BLACK = new BlackStone();
        
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
