using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathInSimplePolygon.Geometry
{
    public class Polygon
    {
        public List<Point> Vertices { get; set;} = new List<Point>();
        public List<LineSegment> Edges { get; set; } = new List<LineSegment>();
        public List<LineSegment> Segments { get; set; } = new List<LineSegment>();
        public List<List<LineSegment>> MaximalNonTrivialCriticalSegments { get; set; } = new List<List<LineSegment>>();

        public void CreateMaximalCriticalSegments()
        {
            Dictionary<double, List<LineSegment>> GroupedSegmentsByY = new Dictionary<double, List<LineSegment>>();
            foreach(var segment in Segments)
            {
                if (GroupedSegmentsByY.ContainsKey(segment.Start.Y))
                    GroupedSegmentsByY[segment.Start.Y].Add(segment);
                else GroupedSegmentsByY.Add(segment.Start.Y, new List<LineSegment> { segment });
            }

            foreach (List<LineSegment> segments in GroupedSegmentsByY.Values)
            {
                if (segments.Count > 1)
                {
                    List<LineSegment> sortSegments = segments.OrderBy(x => x.Start.X).ToList();
                    bool isNewCritical = true;
                    for (int i=1; i<sortSegments.Count; i++)
                    {
                        if (sortSegments[i-1].End.X == sortSegments[i].Start.X)
                        {
                            if (isNewCritical)
                            {
                                MaximalNonTrivialCriticalSegments.Add(new List<LineSegment> { sortSegments[i - 1] });
                                isNewCritical = false;
                            }

                            MaximalNonTrivialCriticalSegments.Last().Add(sortSegments[i]);
                        }
                        else
                        {
                            isNewCritical = true;
                        }
                    }
                }
            }

            MaximalNonTrivialCriticalSegments = MaximalNonTrivialCriticalSegments.OrderBy(x => x[0].Start.Y).ToList();
        }

        public Polygon() { 

        }
        public Polygon (List<Point> vertices)
        {
            Vertices = vertices;
            GenerateEdges();
        }

        public void Clear()
        {
            Vertices.Clear();
            Edges.Clear();
            Segments.Clear();
        }

        public void Open(List<Point> vertices)
        {
            Vertices = vertices;
            GenerateEdges();
        }

        private void GenerateEdges()
        {
            for (int i = 0; i < Vertices.Count-1; i++)
            {
                var e = new LineSegment(Vertices[i], Vertices[i+1]);
                Edges.Add(e);
            }
            var e_end = new LineSegment(Vertices.Last(), Vertices.First());
            Edges.Add(e_end);
        }
    }
}
