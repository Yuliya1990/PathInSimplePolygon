using PathInSimplePolygon.Geometry;

namespace PathInSimplePolygon
{
    public partial class Form1 : Form
    {
        private Polygon polygon = new Polygon();
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (polygon.Vertices.Count > 0)
            {
                Graphics graphics = e.Graphics;
                Pen pen = new Pen(Color.Black);

                // Display the polygon
                PointF[] points = new PointF[polygon.Vertices.Count+1]; // Exclude the duplicate vertex
                for (int i = 0; i < points.Length-1; i++)
                {
                    points[i] = new PointF((float)polygon.Vertices[i].X, (float)polygon.Vertices[i].Y);
                }
                points[points.Length-1] = new PointF((float)polygon.Vertices[0].X, ((float)polygon.Vertices[0].Y));
                graphics.DrawPolygon(pen, points);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DGridFile|*.txt";
            openFileDialog.Title = "Select Polygon File";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            string filePath = openFileDialog.FileName;

            polygon.Clear();
            List<Geometry.Point> vertices = new List<Geometry.Point>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] coordinates = line.Split(' ');
                    if (coordinates.Length >= 2)
                    {
                        if (double.TryParse(coordinates[0], out double x) &&
                            double.TryParse(coordinates[1], out double y))
                        {
                            Geometry.Point vertice = new Geometry.Point(x, y);
                            vertices.Add(vertice);
                        }
                    }
                }
            }

            polygon.Open(vertices);
            Refresh();

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var algorithm = new Algorithm(polygon);
            algorithm.PreliminaryProcessing();

            // Отримання графічного об'єкта для форми
            Graphics g = this.CreateGraphics();
            // Домалювання відрізків
            foreach (var segment in polygon.Segments)
            {
                // Отримання початкової і кінцевої точок відрізка
                PointF startPoint = new PointF((float)segment.Start.X, (float)segment.Start.Y);
                PointF endPoint = new PointF((float)segment.End.X, (float)segment.End.Y);

                // Домалювання відрізка на формі
                g.DrawLine(Pens.Red, startPoint, endPoint);
                g.FillEllipse(Brushes.Red, new Rectangle((int)segment.MidPoint.X-2, (int)segment.MidPoint.Y-2, 5, 5));
            }

            string resultDebug = String.Empty;
            /*    foreach (var segment in polygon.MaximalNonTrivialCriticalSegments)
                {
                    foreach (var k in segment)
                    {
                        resultDebug += k.Start.X.ToString() + " " + k.Start.Y.ToString() + "     " + k.End.X + " " + k.End.Y + "\n";
                    }
                    resultDebug += "\n\n\n";
                }
            */

            foreach (var segment in polygon.Segments)
            {
                resultDebug += segment.Start.X.ToString() + " " + segment.Start.Y.ToString() + "     " + segment.End.X + " " + segment.End.Y + "\n";
            }
            MessageBox.Show(resultDebug);
            // Звільнення ресурсів графічного об'єкта

            g.Dispose();
        }
    }
}