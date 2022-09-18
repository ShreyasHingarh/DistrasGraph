using System.Runtime.CompilerServices;

namespace DikstraVisualizer
{
    public partial class Form1 : Form
    {
        GraphForAStar<int> graph;
        Bitmap map;
        Graphics g;
        int YAmount;
        int XAmount;
        VertexForA<int> StartPoint;
        VertexForA<int> EndPoint;
        VertexForA<int>[,] vertices;
        List<VertexForA<int>> Walls = new List<VertexForA<int>>();
        public void createVertices(int width)
        {
            int startingX = 0;
            int startingY = 0;

            int count = 0;
            for (int i = 0; i < YAmount; i++)
            {
                for (int x = 0; x < XAmount; x++)
                {
                    VertexForA<int> temp = new VertexForA<int>(count, new Rectangle(startingX, startingY, width, width));
                    graph.AddVertex(temp);
                    g.FillRectangle(Brushes.White, startingX, startingY, width, width);
                    g.DrawRectangle(Pens.Black,startingX, startingY, width, width);
                    vertices[i, x] = temp;
                    count++;
                    startingX += width;
                    
                }
                startingX = 0;
                startingY += width;
            }
        }
        public void createEdges()
        {
          
            int distance = 15;
            
            for(int i = 0;i < YAmount;i++)
            {
                for (int x = 0; x < XAmount; x++)
                {
                    if (i - 1 > 0)
                    {
                        graph.AddEdge(vertices[i,x], vertices[i - 1, x], distance);
                    }
                    if (i + 1 < YAmount)
                    {
                        graph.AddEdge(vertices[i, x], vertices[i + 1, x], distance);
                    }
                    if (x - 1 > 0)
                    {
                        graph.AddEdge(vertices[i, x], vertices[i, x - 1], distance);
                    }
                    if (x + 1 < XAmount)
                    {
                        graph.AddEdge(vertices[i, x], vertices[i, x + 1], distance);
                    }
                    if(x - 1 > 0 && i + 1 < YAmount)
                    {
                        graph.AddEdge(vertices[i, x], vertices[i + 1, x - 1], distance);
                    }
                    if (x - 1 > 0 && i - 1 > 0)
                    {
                        graph.AddEdge(vertices[i, x], vertices[i - 1, x - 1], distance);
                    }
                    if (x + 1 < XAmount && i + 1 < YAmount)
                    {
                        graph.AddEdge(vertices[i, x], vertices[i + 1, x + 1], distance);
                    }
                    if (x + 1 < XAmount && i - 1 > 0)
                    {
                        graph.AddEdge(vertices[i, x], vertices[i - 1, x + 1], distance);
                    }
                }
            }
            
        }
      
        public Form1()
        {
            InitializeComponent();
            map = new Bitmap(Picture.Width, Picture.Height);
            g = Graphics.FromImage(map);
            graph = new GraphForAStar<int>(1, 1);
            Picture.Image = map;
            YAmount = 22;
            XAmount = 32;
            vertices = new VertexForA<int>[YAmount,XAmount];

            HeuristicsPicker.Items.Add("Manhattan");
            HeuristicsPicker.Items.Add("Diagonal");
            HeuristicsPicker.Items.Add("Euclidian");
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            int width = 25;
            createVertices(width);
            createEdges();
        }
        private void Picture_MouseClick(object sender, MouseEventArgs e)
        {
            Point savedCordsOfMouse = PointToClient(MousePosition); 
            if (e.Button == MouseButtons.Left)
            {
                VertexForA<int> curvertex = new VertexForA<int>(-1,new Rectangle(-10,-10,-2,-2));
                foreach (var vertex in vertices)
                {
                    if(vertex.Position.Contains(savedCordsOfMouse))
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
                else if(EndPoint != null && StartPoint == null && EndPoint != curvertex)
                {
                    g.FillRectangle(Brushes.Green, curvertex.Position);
                    StartPoint = curvertex;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                
                VertexForA<int> curvertex = new VertexForA<int>(-1, new Rectangle(-10, -10, -2, -2));
                foreach (var vertex in vertices)
                {
                    if (vertex.Position.Contains(savedCordsOfMouse))
                    {
                        curvertex = vertex;
                        break;
                    }
                }

                if(curvertex != StartPoint && curvertex != EndPoint)
                {
                    g.FillRectangle(Brushes.Gray, curvertex.Position);
                    g.DrawRectangle(Pens.Black, curvertex.Position);
                    Walls.Add(curvertex);
                }
            }
            Picture.Image = map;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.C)
            {
                foreach (var vertex in vertices)
                {
                    g.FillRectangle(Brushes.White, vertex.Position);
                    g.DrawRectangle(Pens.Black, vertex.Position);
                }
                StartPoint = null;
                EndPoint = null;
                
            }
            if (e.KeyCode == Keys.S)
            {
                if (StartPoint != null && EndPoint != null)
                {
                    List<VertexForA<int>> vertices = graph.AStarSearchAlgorithm(StartPoint, EndPoint,Walls);
                    foreach (var vertex in vertices)
                    {
                        if (vertex != StartPoint && vertex != EndPoint)
                        {
                            g.FillRectangle(Brushes.Blue, vertex.Position);
                        }
                    }
                }

            }
            Picture.Image = map;
        }

        private void HeuristicsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<int, Func<int, int, int, int, int>> functions;
           
        }
    }
}