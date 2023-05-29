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
    }
}
