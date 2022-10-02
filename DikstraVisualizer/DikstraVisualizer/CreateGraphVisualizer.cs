using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DikstraVisualizer
{
    public class CreateGraphVisualizer
    {
        public GraphForAStar<int> Graph;
        public int YAmount;
        public int XAmount;
        public Vertex<int>[,] GridOfVertecies;
        public List<Edge<int>> EdgesOfAllVertices;
        public int Width;

        public CreateGraphVisualizer( int yAmount, int xAmount, int width)
        {
            Graph = new GraphForAStar<int>(1, 1);
            YAmount = yAmount;
            XAmount = xAmount;
            GridOfVertecies = new Vertex<int>[YAmount, XAmount];
            EdgesOfAllVertices = new List<Edge<int>>();
            Width = width;
        }

        public void createGridOfVertices(Graphics g)
        {
            int startingX = 0;
            int startingY = 0;

            int count = 0;
            for (int i = 0; i < YAmount; i++)
            {
                for (int x = 0; x < XAmount; x++)
                {
                    Vertex<int> temp = new Vertex<int>(count, new Rectangle(startingX, startingY, Width, Width));
                    Graph.AddVertex(temp);
                    g.FillRectangle(Brushes.White, startingX, startingY, Width, Width);
                    g.DrawRectangle(Pens.Black, startingX, startingY, Width, Width);
                    GridOfVertecies[i, x] = temp;
                    count++;
                    startingX += Width;

                }
                startingX = 0;
                startingY += Width;
            }
        }

        private void WhenContain(Edge<int> temp)
        {
            if (!EdgesOfAllVertices.Contains(temp))
            {
                EdgesOfAllVertices.Add(Graph.edges[Graph.edges.Count - 1]);
            }
        }
        private void compareForTheGreater(bool isForX, int ypos, int xpos, int yamount, int xamount, float horDistance)
        {
            int posToChance = isForX ? xpos : ypos;
            int amountToChance = isForX ? xamount : yamount;
            if (posToChance + 1 < amountToChance)
            {
                Edge<int> temp = new Edge<int>(null, null, -1);
                if (!isForX)
                {
                    Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos], horDistance);
                    temp = Graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos]);
                }
                else
                {
                    Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos + 1], horDistance);
                    temp = Graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos + 1]);
                }
                WhenContain(temp);
            }

        }
        private void compareForTheLower(bool isForX, int ypos, int xpos, float horDistance)
        {
            int posToChance = isForX ? xpos : ypos;
            if (posToChance - 1 >= 0)
            {
                Edge<int> temp = new Edge<int>(null,null,-1);

                if (!isForX)
                {
                    Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos], horDistance);
                    temp = Graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos]);
                }
                else
                {
                    Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos - 1], horDistance);
                    temp = Graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos - 1]);
                }
                WhenContain(temp);
            }
        }
        private void GetDiagonalEdges(int xpos, int ypos, float diaDistance)
        {
            if (xpos - 1 >= 0 && ypos + 1 < YAmount)
            {
                Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos - 1], diaDistance);
                WhenContain(Graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos - 1]));
            }
            if (xpos - 1 >= 0 && ypos - 1 >= 0)
            {
                Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos - 1], diaDistance);
                WhenContain(Graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos - 1]));
            }
            if (xpos + 1 < XAmount && ypos + 1 < YAmount)
            {
                Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos + 1], diaDistance);
                WhenContain(Graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos + 1]));
            }
            if (xpos + 1 < XAmount && ypos - 1 >= 0)
            {
                Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos + 1], diaDistance);
                WhenContain(Graph.GetEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos + 1]));
            }
        }
        public void CreateEdgesForAVertex(int xpos, int ypos)
        {
            if (xpos < 0 || xpos >= XAmount || ypos < 0 || ypos >= YAmount) return;
            float horDistance = 25;
            float diaDistance = (float)Math.Sqrt(horDistance * horDistance * 2);

            compareForTheLower(true, ypos, xpos, horDistance);
            compareForTheLower(false, ypos, xpos, horDistance);
            compareForTheGreater(true, ypos, xpos, YAmount, XAmount, horDistance);
            compareForTheGreater(false, ypos, xpos, YAmount, XAmount, horDistance);
            GetDiagonalEdges(xpos, ypos, diaDistance);
        }
        public void createEdgesForGrid()
        {
            for (int i = 0; i < YAmount; i++)
            {
                for (int x = 0; x < XAmount; x++)
                {
                    CreateEdgesForAVertex(x, i);
                }
            }
        }
    }
}
