using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_wpf.src
{
    public class Direction
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public readonly static Direction Left = new Direction(0, -1);
        public readonly static Direction Right = new Direction(0, 1);
        public readonly static Direction Up = new Direction(-1, 0);
        public readonly static Direction Down = new Direction(1, 0);
        private Direction(int row, int col) 
        {
            Row = row;
            Column = col;
        }
        public Direction Opposite()
        {
            return new Direction(-Row, -Column);
        }

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   Row == direction.Row &&
                   Column == direction.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }
    }
}
