using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotBits.WorldDictionary
{
    internal struct PointSlim : IEquatable<PointSlim>
    {
        public PointSlim(ushort x, ushort y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public ushort X { get; set; }
        public ushort Y { get; set; }

        public bool Equals(PointSlim other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PointSlim && this.Equals((PointSlim)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X * 397) ^ this.Y;
            }
        }

        public static bool operator ==(PointSlim left, PointSlim right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PointSlim left, PointSlim right)
        {
            return !left.Equals(right);
        }
        
        public static explicit operator Point(PointSlim slim)
        {
            return new Point(slim.X, slim.Y);
        }

        public static explicit operator PointSlim(Point point)
        {
            return new PointSlim((ushort)point.X, (ushort)point.Y);
        }
    }
}