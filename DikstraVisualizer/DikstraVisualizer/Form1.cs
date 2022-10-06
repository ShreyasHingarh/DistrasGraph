using Microsoft.VisualBasic.Devices;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;

namespace DikstraVisualizer
{
    public partial class Form1 : Form
    {

        Bitmap map;
        Graphics g;

        Vertex<int> StartPoint;
        Vertex<int> EndPoint;
        CreateGraphVisualizer visualizer;
        List<int> wallsValues;

        int HeurIndex = -1;
        bool hasRan = false;
        bool hasfinished = false;
        public Vertex<int> FindVertexBasedOnMouse(Point savedCordsOfMouse)
        {
            foreach (var vertex in visualizer.GridOfVertecies)
            {
                if (vertex.Position.Contains(savedCordsOfMouse))
                {
                    return vertex;
                }
            }
            return null;
        }
        public void EndOfSearch(List<Vertex<int>> verticies)
        {
            if (verticies.Count == 0)
            {
                MessageBox.Show("no possible path to endingpoint");
            }
            else
            {
                foreach (var vertex in verticies)
                {
                    if (vertex != StartPoint && vertex != EndPoint)
                    {
                        g.FillRectangle(Brushes.Purple, vertex.Position);
                    }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            map = new Bitmap(Picture.Width, Picture.Height);
            g = Graphics.FromImage(map);
            wallsValues = new List<int>();
            visualizer = new CreateGraphVisualizer(22, 32, 25);
            HeuristicsPicker.Items.Add("Manhattan");
            HeuristicsPicker.Items.Add("Chebyshev");
            HeuristicsPicker.Items.Add("Octile");
            HeuristicsPicker.Items.Add("Euclidian");
            visualizer.createGridOfVertices(g);
            visualizer.createEdgesForGrid();
            Directions.Text = "C,D,E,S";
            Picture.Image = map;
        }

        //add sand, mud, make animated
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Picture_MouseClick(object sender, MouseEventArgs e)
        {
            Point savedCordsOfMouse = PointToClient(MousePosition);
            if (e.Button == MouseButtons.Left && !hasRan)
            {
                Vertex<int> curvertex = new Vertex<int>(-1, new Rectangle(-10, -10, -2, -2));
                foreach (var vertex in visualizer.Graph.vertices)
                {
                    if (vertex.Position.Contains(savedCordsOfMouse))
                    {
                        curvertex = vertex;
                        break;
                    }
                }
                if (curvertex.Value == -1) return;
                if (StartPoint == null && EndPoint == null)
                {
                    g.FillRectangle(Brushes.Green, curvertex.Position);
                    StartPoint = curvertex;
                }
                else if (StartPoint != null && StartPoint == curvertex)
                {
                    g.FillRectangle(Brushes.White, curvertex.Position);
                    g.DrawRectangle(Pens.Black, curvertex.Position);
                    StartPoint = null;
                }
                else if (StartPoint != null && EndPoint == null && curvertex != StartPoint)
                {
                    g.FillRectangle(Brushes.Red, curvertex.Position);
                    EndPoint = curvertex;
                }
                else if (EndPoint != null && EndPoint == curvertex)
                {
                    g.FillRectangle(Brushes.White, curvertex.Position);
                    g.DrawRectangle(Pens.Black, curvertex.Position);
                    EndPoint = null;
                }
                else if (EndPoint != null && StartPoint == null && EndPoint != curvertex)
                {
                    g.FillRectangle(Brushes.Green, curvertex.Position);
                    StartPoint = curvertex;
                }
            }

            Picture.Image = map;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Point savedCordsOfMouse = PointToClient(MousePosition);
            //clear
            if (e.KeyCode == Keys.C)
            {
                g.Clear(Color.White);
                int totalNumberVertices = visualizer.Graph.VertexCount;
                for (int i = 0; i < totalNumberVertices; i++)
                {
                    visualizer.Graph.RemoveVertex(visualizer.Graph.vertices[0]);
                }
                ;
                visualizer.createGridOfVertices(g);
                visualizer.createEdgesForGrid();
                StartPoint = null;
                EndPoint = null;
                hasRan = false;
                hasfinished = false;
                SearchTimer.Enabled = false;
            }
            //start the search
            else if (e.KeyCode == Keys.S && !hasRan)
            {
                if (HeurIndex < 0)
                {
                    MessageBox.Show("Choose a gosh diddily darn heuristic");
                }
                else if (StartPoint != null && EndPoint != null)
                {
                    hasRan = true;
                    visualizer.Graph.SetupAStar(StartPoint, EndPoint, HeurIndex);
                    SearchTimer.Start();
                    SearchTimer.Enabled = true;
                }
            }
            //draw walls
            else if (e.KeyCode == Keys.D && !hasRan)
            {
                Vertex<int> Vertex = FindVertexBasedOnMouse(savedCordsOfMouse);

                if (Vertex != StartPoint && Vertex != EndPoint)
                {
                    if (visualizer.Graph.vertices.Contains(Vertex))
                    {
                        g.FillRectangle(Brushes.Gray, Vertex.Position);
                        g.DrawRectangle(Pens.Black, Vertex.Position);
                        visualizer.Graph.RemoveVertex(Vertex);
                        wallsValues.Add(Vertex.Value);
                    }

                }
            }
            // erase walls
            else if (e.KeyCode == Keys.E && !hasRan)
            {

                Vertex<int> Vertex = FindVertexBasedOnMouse(savedCordsOfMouse);
                if (Vertex != StartPoint && Vertex != EndPoint && !visualizer.Graph.vertices.Contains(Vertex))
                {
                    g.FillRectangle(Brushes.White, Vertex.Position);
                    g.DrawRectangle(Pens.Black, Vertex.Position);

                    for (int i = 0; i < visualizer.YAmount; i++)
                    {
                        for (int x = 0; x < visualizer.XAmount; x++)
                        {
                            if (visualizer.GridOfVertecies[i, x] == Vertex)
                            {
                                visualizer.Graph.AddVertex(Vertex);
                                visualizer.CreateEdgesForAVertex(x, i);
                                visualizer.CreateEdgesForAVertex(x - 1, i - 1);
                                visualizer.CreateEdgesForAVertex(x - 1, i);
                                visualizer.CreateEdgesForAVertex(x - 1, i + 1);
                                visualizer.CreateEdgesForAVertex(x, i + 1);
                                visualizer.CreateEdgesForAVertex(x + 1, i + 1);
                                visualizer.CreateEdgesForAVertex(x + 1, i);
                                visualizer.CreateEdgesForAVertex(x + 1, i - 1);
                                visualizer.CreateEdgesForAVertex(x, i - 1);
                                break;
                            }
                        }
                        wallsValues.Remove(Vertex.Value);
                    }



                }

            }
            Picture.Image = map;

        }
        private void HeuristicsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            HeurIndex = HeuristicsPicker.SelectedIndex;
        }

        private void PrevWalls_Click(object sender, EventArgs e)
        {
            if (wallsValues.Count == 0)
            {
                return;
            }
            foreach (var value in wallsValues)
            {
                if (visualizer.Graph.vertices.Contains(visualizer.Graph.Search(value)))
                {
                    if (visualizer.Graph.Search(value) != StartPoint && visualizer.Graph.Search(value) != EndPoint)
                    {
                        if (visualizer.Graph.vertices.Contains(visualizer.Graph.Search(value)))
                        {
                            g.FillRectangle(Brushes.Gray, visualizer.Graph.Search(value).Position);
                            g.DrawRectangle(Pens.Black, visualizer.Graph.Search(value).Position);
                            visualizer.Graph.RemoveVertex(visualizer.Graph.Search(value));
                        }
                    }
                }
            }
            Picture.Image = map;
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            Information info;
            SearchTimer.Enabled = visualizer.Graph.MainAStarPart(StartPoint, EndPoint, HeurIndex, out info);
            if (info.Position == StartPoint.Position || info.Position == EndPoint.Position) return;
            Brush brush = Brushes.LightGreen;
            if (info.result == Result.Dequeue)
            {
                brush = Brushes.LightBlue;
            }
            g.FillRectangle(brush, info.Position);
            g.DrawRectangle(Pens.Black, info.Position);
            if(SearchTimer.Enabled == false)
            {
                SearchTimer.Stop();
                EndOfSearch(visualizer.Graph.EndingAStar(StartPoint, EndPoint));
            }
            Picture.Image = map;
        }
        private void OneIteration_Click(object sender, EventArgs e)
        {
            
            if (StartPoint == null || EndPoint == null || HeurIndex < 0 || hasfinished) return;
            if (!hasRan)
            {
                hasRan = true;
                visualizer.Graph.SetupAStar(StartPoint, EndPoint, HeurIndex);
            }
            int i = 0;
            bool result = true;
            while (i < 10)
            {
                Information info;
                result = visualizer.Graph.MainAStarPart(StartPoint, EndPoint, HeurIndex, out info);
                if (info.Position == StartPoint.Position || info.Position == EndPoint.Position) return;
                Brush brush = Brushes.LightBlue;
                if (info.result == Result.Enqueue)
                {
                    brush = Brushes.LightGreen;
                }
                g.FillRectangle(brush, info.Position);
                g.DrawRectangle(Pens.Black, info.Position);

                Picture.Image = map;
                i++;
            }
            if (result == false)
            {
                hasfinished = true;
                EndOfSearch(visualizer.Graph.EndingAStar(StartPoint, EndPoint));
            }
        }
    }
}