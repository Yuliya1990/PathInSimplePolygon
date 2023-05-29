using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathInSimplePolygon.Geometry;

namespace PathInSimplePolygon
{
    public class Algorithm
    {
        private Polygon _polygon;

        public Algorithm(Polygon polygon)
        {
                _polygon = polygon;
        }

        private Geometry.Point findIntersectionPoint(Geometry.Point vertex, Geometry.Point p1, Geometry.Point p2)
        {
   /*
    * 
    * if (p1.X > p2.X)
            {
                Geometry.Point t = new Geometry.Point(p1);
                p1 = p2;
                p2 = t;

            }*/
            double result = p2.X - Math.Abs(p2.X - p1.X) * Math.Abs(p2.Y - vertex.Y) / Math.Abs(p2.Y - p1.Y);
            return new Geometry.Point(result, vertex.Y);
        }

        public void PreliminaryProcessing()
        {
            foreach (var vertice in _polygon.Vertices)
            {
                var susp_edg = _polygon.Edges.Where(e => e.Start != vertice && e.End != vertice &&
                ((e.Start.Y >= vertice.Y && e.End.Y <= vertice.Y) || 
                (e.Start.Y <= vertice.Y && e.End.Y >= vertice.Y))).ToList();

                List<Geometry.Point> interseptionPoints = new List<Geometry.Point>();
                List<Geometry.Point> leftPoints = new List<Geometry.Point>();
                List<Geometry.Point> rightPoints = new List<Geometry.Point>();

                foreach (var susp in susp_edg)
                {
                    Geometry.Point p = findIntersectionPoint(vertice, susp.Start, susp.End);
                    interseptionPoints.Add(p);
                    if (p.X < vertice.X)
                        leftPoints.Add(p);
                    else rightPoints.Add(p);
                }
                if (leftPoints.Count > 0)
                {
                    var closestPoint = leftPoints.MaxBy(x => x.X);
                    _polygon.Segments.Add(new LineSegment(closestPoint, vertice));
                }
                if (rightPoints.Count > 0)
                {
                    var closestPoint = rightPoints.MinBy(x => x.X);
                    _polygon.Segments.Add(new LineSegment(vertice, closestPoint));
                }
                
            }
        }
    }
}
