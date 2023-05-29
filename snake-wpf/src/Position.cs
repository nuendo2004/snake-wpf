using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_wpf.src
{
    public class Position
    {
        public int Row { get;}
        public int Column { get; }

        public Position(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }
        public Position translate(Direction dir)
        {
            return new Position(this.Row + dir.Row, this.Column + dir.Column);
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }
}
