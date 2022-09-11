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
        VertexForA<int> IsThereAStartPoint;
        VertexForA<int> IsThereAEndPoint;
        VertexForA<int>[,] vertices;
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
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
            int width = 25;
            createVertices(width);
            createEdges();

            while(true)
            {
                
            }
        }

        private void Picture_MouseClick(object sender, MouseEventArgs e)
        {
            Point savedCordsOfMouse = MousePosition; 
            if (e.Button == MouseButtons.Left)
            {
                foreach (var vertex in vertices)
                {
                    if(vertex.Position.Contains(savedCordsOfMouse))
                    {
                        if(IsThereAStartPoint == null && IsThereAEndPoint == null)
                        {
                            g.FillRectangle(Brushes.Orange, vertex.Position);
                            IsThereAStartPoint = vertex;
                        }
                        else if(IsThereAStartPoint != null && IsThereAStartPoint == vertex)
                        {
                            g.FillRectangle(Brushes.White, vertex.Position);
                            IsThereAStartPoint = null;
                        }
                        else if(IsThereAStartPoint != null && IsThereAEndPoint == null)
                        {

                        }
                    }
                }
            }
            if(e.Button == MouseButtons.Right)
            {
                graph.AStarSearchAlgorithm();
            }
        }
    }
}