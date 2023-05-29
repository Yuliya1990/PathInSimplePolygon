using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathInSimplePolygon.Geometry
{
    public class LineSegment
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public Point MidPoint { get { return new Point((Start.X + End.X) / 2, (Start.Y + End.Y) / 2); } set { } }
        public LineSegment() { 
            Start = new Point();
            End = new Point();
        }
        public LineSegment(Point p1, Point p2)
        {
            Start = p1;
            End = p2;
        }

        public static bool operator ==(LineSegment s1, LineSegment s2)
        {
            if (ReferenceEquals(s1, s2))
                return true;

            if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null))
                return false;

            return s1.Start == s2.Start && s2.End == s2.End;
        }

        public static bool operator !=(LineSegment s1, LineSegment s2)
        {
            return !(s1 == s2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            LineSegment other = (LineSegment)obj;
            return Start == other.Start && End == other.End;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Start.GetHashCode();
                hash = hash * 23 + End.GetHashCode();
                return hash;
            }
        }
    }
}
