using Microsoft.VisualBasic.Devices;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;

namespace DikstraVisualizer
{
    public partial class Form1 : Form
    {
        GraphForAStar<int> graph;
        Bitmap map;
        Graphics g;
        int YAmount;
        int XAmount;
        Vertex<int> StartPoint;
        Vertex<int> EndPoint;
        Vertex<int>[,] GridOfVertecies;
        List<Edge<int>> EdgesOfAllVertices;
        int HeurIndex = -1;
        int width = 25;
        bool hasRan = false;
        public void createGridOfVertices(int width)
        {
            int startingX = 0;
            int startingY = 0;

            int count = 0;
            for (int i = 0; i < YAmount; i++)
            {
                for (int x = 0; x < XAmount; x++)
                {
                    Vertex<int> temp = new Vertex<int>(count, new Rectangle(startingX, startingY, width, width));
                    graph.AddVertex(temp);
                    g.FillRectangle(Brushes.White, startingX, startingY, width, width);
                    g.DrawRectangle(Pens.Black,startingX, startingY, width, width);
                    GridOfVertecies[i, x] = temp;
                    count++;
                    startingX += width;
                    
                }
                startingX = 0;
                startingY += width;
            }
        }
        private void compareForTheGreater(bool isYorX,int ypos,int xpos, int highAmount,int horDistance)
        {
            int posToChance = isYorX ? xpos : ypos;
            if (posToChance + 1 < highAmount)
            {
                Edge<int> temp;
                if(posToChance == ypos)
                {
                    graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos], horDistance);
                    temp = graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos]);
                }
                else if(posToChance == xpos)
                {
                    graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos + 1], horDistance);
                    temp = graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos + 1]);
                }
                if (!EdgesOfAllVertices.Contains(temp))
                {
                    EdgesOfAllVertices.Add(graph.edges[graph.edges.Count - 1]);
                }
            }

        }
        public void CreateEdgesForAVertex(int xpos,int ypos)
        { 
            float horDistance = 25;
            float diaDistance = (float)Math.Sqrt(horDistance * horDistance * 2);
            
            if (ypos - 1 >= 0)
            {
                graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos], horDistance);
                Edge<int> temp = graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos]);
                if (!EdgesOfAllVertices.Contains(temp))
                {
                    EdgesOfAllVertices.Add(graph.edges[graph.edges.Count - 1]);
                }
            }
            if (ypos+ 1 < YAmount)
            {
                graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos], horDistance);
                Edge<int> temp = graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos]);
                if (!EdgesOfAllVertices.Contains(temp))
                {
                    EdgesOfAllVertices.Add(graph.edges[graph.edges.Count - 1]);
                }
                
            }
            if (xpos - 1 >= 0)
            {
                graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos - 1], horDistance);
                Edge<int> temp = graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos - 1]);
                if (!EdgesOfAllVertices.Contains(temp))
                {
                    EdgesOfAllVertices.Add(graph.edges[graph.edges.Count - 1]);
                }
            }
            if (xpos + 1 < XAmount)
            {
                graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos + 1], horDistance);
                EdgesOfAllVertices.Add(graph.edges[graph.edges.Count - 1]);
            }

            if (xpos - 1 >= 0 && ypos + 1 < YAmount)
            {
                graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos - 1], diaDistance);
                EdgesOfAllVertices.Add(graph.edges[graph.edges.Count - 1]);
            }
            if (xpos - 1 >= 0 && ypos - 1 >= 0)
            {
                graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos - 1], diaDistance);
                EdgesOfAllVertices.Add(graph.edges[graph.edges.Count - 1]);
            }
            if (xpos + 1 < XAmount && ypos + 1 < YAmount)
            {
                graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos + 1], diaDistance);
                EdgesOfAllVertices.Add(graph.edges[graph.edges.Count - 1]);
            }
            if (xpos + 1 < XAmount && ypos - 1 >= 0)
            {
                graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos + 1], diaDistance);
                EdgesOfAllVertices.Add(graph.edges[graph.edges.Count - 1]);
            }
        }
        public void createEdgesForGrid()
        {
            for(int i = 0;i < YAmount;i++)
            {
                for (int x = 0; x < XAmount; x++)
                {
                    CreateEdgesForAVertex(x, i);
                }
            }
        }
      
        public Form1()
        {
            InitializeComponent();
            map = new Bitmap(Picture.Width, Picture.Height);
            g = Graphics.FromImage(map);
            graph = new GraphForAStar<int>(1,1);
            Picture.Image = map;

            YAmount = 22;
            XAmount = 32;
            GridOfVertecies = new Vertex<int>[YAmount,XAmount];
            EdgesOfAllVertices = new List<Edge<int>>();

            HeuristicsPicker.Items.Add("Manhattan");
            HeuristicsPicker.Items.Add("Diagonal");
            HeuristicsPicker.Items.Add("Euclidian");
            createGridOfVertices(width);
            createEdgesForGrid();
        }
        
        //add walls
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void Picture_MouseClick(object sender, MouseEventArgs e)
        {
            Point savedCordsOfMouse = PointToClient(MousePosition); 
            if (e.Button == MouseButtons.Left && !hasRan)
            {
                Vertex<int> curvertex = new Vertex<int>(-1,new Rectangle(-10,-10,-2,-2));
                foreach (var vertex in graph.vertices)
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

            Picture.Image = map;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Point savedCordsOfMouse = PointToClient(MousePosition);
            //clear
            if (e.KeyCode == Keys.C)
            {
                g.Clear(Color.White);
                int totalNumberVertices = graph.VertexCount;
                for (int i = 0; i < totalNumberVertices; i++)
                {
                    graph.RemoveVertex(graph.vertices[0]);
                }
                createGridOfVertices(width);
                createEdgesForGrid();
                StartPoint = null;
                EndPoint = null;
                hasRan = false;
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
                    List<Vertex<int>> vertices = graph.AStarSearchAlgorithm(StartPoint, EndPoint, HeurIndex, g);
                    foreach (var vertex in vertices)
                    {
                        if (vertex != StartPoint && vertex != EndPoint)
                        {
                            g.FillRectangle(Brushes.Purple, vertex.Position);
                        }
                    }
                }

            }
            //draw walls
            else if (e.KeyCode == Keys.D && !hasRan)
            {
                Vertex<int> Vertex = new Vertex<int>(-1, new Rectangle(-10, -10, -2, -2));
                foreach (var vertex in GridOfVertecies)
                {
                    if (vertex.Position.Contains(savedCordsOfMouse))
                    {
                        Vertex = vertex;
                        break;
                    }
                }


                if (Vertex != StartPoint && Vertex != EndPoint)
                {
                    if (graph.vertices.Contains(Vertex))
                    {
                        g.FillRectangle(Brushes.Gray, Vertex.Position);
                        g.DrawRectangle(Pens.Black, Vertex.Position);
                    }
                    
                }
            }
            // erase walls
            else if (e.KeyCode == Keys.E && !hasRan)
            {
                Vertex<int> Vertex = new Vertex<int>(-1, new Rectangle(-10, -10, -2, -2));
                foreach (var vertex in GridOfVertecies)
                {
                    if (vertex.Position.Contains(savedCordsOfMouse))
                    {
                        Vertex = vertex;
                        break;
                    }
                }

                if (Vertex != StartPoint && Vertex != EndPoint && !graph.vertices.Contains(Vertex))
                {
                    g.FillRectangle(Brushes.White, Vertex.Position);
                    g.DrawRectangle(Pens.Black, Vertex.Position);
                
                    int i = 0;
                    int x = 0;
                    for(i = 0;i < YAmount;i++)
                    {
                        for(x = 0;x < XAmount;x++)
                        {
                            if (GridOfVertecies[i, x] == Vertex)
                            {
                                break;
                            }
                        }
                    }
                    CreateEdgesForAVertex(x,i);
                }
               
            }
                Picture.Image = map;
            
        }

        private void HeuristicsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            HeurIndex = HeuristicsPicker.SelectedIndex;   
        }

    }
}