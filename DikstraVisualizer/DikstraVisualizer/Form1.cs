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
        public Form1()
        {
            InitializeComponent();
            map = new Bitmap(Picture.Width, Picture.Height);
            g = Graphics.FromImage(map);
            wallsValues = new List<int>();
            visualizer = new CreateGraphVisualizer(22,32,25);
            HeuristicsPicker.Items.Add("Manhattan");
            HeuristicsPicker.Items.Add("Diagonal");
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
                Vertex<int> curvertex = new Vertex<int>(-1,new Rectangle(-10,-10,-2,-2));
                foreach (var vertex in visualizer.Graph.vertices)
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
                    List<Vertex<int>> vertices =visualizer.Graph.AStarSearchAlgorithm(StartPoint, EndPoint, HeurIndex, g);
                    if (vertices.Count == 0)
                    {
                        MessageBox.Show("no possible path to endingpoint");
                    }
                    else
                    {
                        foreach (var vertex in vertices)
                        {
                            if (vertex != StartPoint && vertex != EndPoint)
                            {
                                g.FillRectangle(Brushes.Purple, vertex.Position);
                            }
                        }
                    }
                    
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
                
                    for(int i = 0;i < visualizer.YAmount;i++)
                    {
                        for(int x = 0;x < visualizer.XAmount;x++)
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
            if(wallsValues.Count == 0)
            {
                return;
            }
            foreach(var value in wallsValues)
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

       
    }
}