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
