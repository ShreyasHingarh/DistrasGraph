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
       
        public int Width;

        public CreateGraphVisualizer( int yAmount, int xAmount, int width)
        {
            Graph = new GraphForAStar<int>(1, 1);
            YAmount = yAmount;
            XAmount = xAmount;
            GridOfVertecies = new Vertex<int>[YAmount, XAmount];
            
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
        private void compareForTheGreater(bool isForX, int ypos, int xpos, int yamount, int xamount, float horDistance)
        {
            int posToChance = isForX ? xpos : ypos;
            int amountToChance = isForX ? xamount : yamount;
            if (posToChance + 1 < amountToChance)
            {
                if (!isForX)
                {
                    Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos], horDistance);
                }
                else
                {
                    Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos + 1], horDistance);
                }
            }

        }
        private void compareForTheLower(bool isForX, int ypos, int xpos, float horDistance)
        {
            int posToChance = isForX ? xpos : ypos;
            if (posToChance - 1 >= 0)
            {
                if (!isForX)
                {
                    Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos], horDistance);
                }
                else
                {
                    Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos, xpos - 1], horDistance);
                }
            }
        }
        public void GetDiagonalEdges(int xpos, int ypos, float diaDistance)
        {
            if (xpos - 1 >= 0 && ypos + 1 < YAmount)
            {
                Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos - 1], diaDistance);
                
            }
            if (xpos - 1 >= 0 && ypos - 1 >= 0)
            {
                Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos - 1], diaDistance);
            }
            if (xpos + 1 < XAmount && ypos + 1 < YAmount)
            {
                Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos + 1, xpos + 1], diaDistance);
            }
            if (xpos + 1 < XAmount && ypos - 1 >= 0)
            {
                Graph.AddEdge(GridOfVertecies[ypos, xpos], GridOfVertecies[ypos - 1, xpos + 1], diaDistance);
            }
        }
        public void CreateEdgesForAVertex(int xpos, int ypos)
        {
            if (xpos < 0 || xpos >= XAmount || ypos < 0 || ypos >= YAmount) return;
            float horDistance = 1;
            float diaDistance = (float)Math.Sqrt(2) * horDistance;

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
