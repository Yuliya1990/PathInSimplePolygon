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

        private Geometry.Point? findIntersectionPoint(Geometry.Point vertex, Geometry.Point p1, Geometry.Point p2)
        {
            if (p1.Y == p2.Y) return null;
            
            double result = 0;
            if (p1.X > p2.X)
            {
                result = p2.X + Math.Abs(p1.X - p2.X) * Math.Abs(vertex.Y - p2.Y) / Math.Abs(p1.Y - p2.Y);
            }
            else
                //double result = p2.X - Math.Abs(p2.X - p1.X) * Math.Abs(p2.Y - vertex.Y) / Math.Abs(p2.Y - p1.Y);
                result = p1.X + Math.Abs(p2.X - p1.X) * Math.Abs(p1.Y - vertex.Y) / Math.Abs(p2.Y - p1.Y);
            return new Geometry.Point(result, vertex.Y);
        }



        public void PreliminaryProcessing()
        {
            foreach (var vertice in _polygon.Vertices)
            {
                var susp_edg = _polygon.Edges.Where(e => 
                e.Start != vertice && 
                e.End != vertice &&
                ((e.Start.Y >= vertice.Y && e.End.Y <= vertice.Y) || 
                (e.Start.Y <= vertice.Y && e.End.Y >= vertice.Y))).ToList();

                List<Geometry.Point> interseptionPoints = new List<Geometry.Point>();
                List<Geometry.Point> leftPoints = new List<Geometry.Point>();
                List<Geometry.Point> rightPoints = new List<Geometry.Point>();

                foreach (var susp in susp_edg)
                {
                    Geometry.Point p = findIntersectionPoint(vertice, susp.Start, susp.End);

                    if (p == null)
                        continue;


                    if (!interseptionPoints.Any(x => x == p) && 
                        !_polygon.Segments.Any(s => 
                        s.Start.Y == vertice.Y &&
                        ((s.Start.X < s.End.X && 
                        s.Start.X == p.X && s.End.X < vertice.X) ||
                            (s.Start.X > s.End.X && s.End.X == p.X && s.Start.X > vertice.X))
                         ))
                    {
                        interseptionPoints.Add(p);
                        if (p.X < vertice.X)
                            leftPoints.Add(p);
                        else rightPoints.Add(p);
                    }
                }
                if (leftPoints.Count > 0 && leftPoints.Count % 2 != 0)
                {
                        var closestPoint = leftPoints.MaxBy(x => x.X);
                        LineSegment newSegment = new LineSegment(closestPoint, vertice);
                        if(!_polygon.Segments.Any(x=>(x.Start.X == newSegment.Start.X && x.Start.Y == newSegment.Start.Y) || (x.Start.X == newSegment.End.X && x.Start.Y == newSegment.Start.Y)))
                            _polygon.Segments.Add(newSegment);
                }
                if (rightPoints.Count > 0 && rightPoints.Count % 2 != 0)
                {
                        var closestPoint = rightPoints.MinBy(x => x.X);
                        LineSegment newSegment = new LineSegment(vertice, closestPoint);
                    if (!_polygon.Segments.Any(x => (x.Start.X == newSegment.Start.X && x.Start.Y == newSegment.Start.Y) || (x.Start.X == newSegment.End.X && x.Start.Y == newSegment.Start.Y)))
                        _polygon.Segments.Add(newSegment);
                }
                
            }
            _polygon.Segments = _polygon.Segments.OrderBy(x => x.Start.Y).ToList();
            _polygon.CreateMaximalCriticalSegments();
        }
    }
}
