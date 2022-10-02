using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DikstraVisualizer
{
    public class GraphForAStar<T> : GraphFunctions<T>
    {
        public int Scalar;
        public int Scalar2;
        
        
        static readonly Func<int, int, int, int, int,int,int>[] Funcs = new Func<int, int, int, int, int, int, int>[]
        {
            HeurManhattan,
            HeurEuclidean,
            HeurDiagonal
        };

        public GraphForAStar(int scalar, int scalar2)
        {
            Scalar = scalar;
            Scalar2 = scalar2;
            
        }
      
        public static int HeurManhattan(int nodeX,int nodeY,int goalX,int goalY, int Scalar,int Scalar2)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(nodeY - goalY);
            return Scalar * (dx + dy);
        }
        public static int HeurDiagonal(int nodeX, int nodeY, int goalX, int goalY, int Scalar,int Scalar2)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(nodeY - goalY);
            return Scalar * (dx + dy) + (Scalar2 - 2 * Scalar2) * Math.Min(dx,dy);
        }
        public static int HeurEuclidean(int nodeX, int nodeY, int goalX, int goalY, int Scalar,int Scalar2)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(nodeY - goalY);
            return (int)(Scalar * Math.Sqrt(dx * dx + dy * dy));
        }
        public List<Vertex<T>> AStarSearchAlgorithm(Vertex<T> a, Vertex<T> b,int HeurType,Graphics g)
        {
            if(a == null || b == null || a == b || a.NeighborCount == 0)
            {
                return null;
            }
            foreach(var vertex in vertices)
            {
                vertex.HasBeenVisited = false;
                vertex.CumlativeDistance = float.PositiveInfinity;
                vertex.FinalDistance = float.PositiveInfinity;
                vertex.Founder = null;
            }
            a.CumlativeDistance = 0;
            
            a.FinalDistance = a.CumlativeDistance + Funcs[HeurType](a.Position.X,a.Position.Y,b.Position.X,b.Position.Y,Scalar,Scalar2);

            PriorityQueue<Vertex<T>, float> queue = new PriorityQueue<Vertex<T>, float>();
            List<Vertex<T>> list = new List<Vertex<T>>();

            queue.Enqueue(a, a.FinalDistance);
            list.Add(a);
            while (queue.Count != 0 && !b.HasBeenVisited)
            {
                Vertex<T> current = queue.Dequeue();

                for(int i = 0;i < current.NeighborCount;i++)
                {
                    Edge<T> curEdge = current.Neighbors[i];
                    float tentativeDistance = current.CumlativeDistance + curEdge.Distance;
                    if (tentativeDistance < curEdge.EndingPoint.CumlativeDistance)
                    {
                        curEdge.EndingPoint.CumlativeDistance = tentativeDistance;
                        curEdge.EndingPoint.Founder = current;
                        curEdge.EndingPoint.FinalDistance = curEdge.EndingPoint.CumlativeDistance + Funcs[HeurType](curEdge.EndingPoint.Position.X, curEdge.EndingPoint.Position.Y,b.Position.X,b.Position.Y,Scalar,Scalar2);
                    }
                    if(!curEdge.EndingPoint.HasBeenVisited && !list.Contains(curEdge.EndingPoint))
                    {
                        queue.Enqueue(curEdge.EndingPoint,curEdge.EndingPoint.FinalDistance);
                        if(curEdge.EndingPoint != a && curEdge.EndingPoint != b && vertices.Contains(curEdge.EndingPoint))
                        { 
                            g.FillRectangle(Brushes.LightGreen, curEdge.EndingPoint.Position);
                            g.DrawRectangle(Pens.Black, curEdge.EndingPoint.Position);
                            
                        }
                        list.Add(curEdge.EndingPoint);
                    }
                }
                
                current.HasBeenVisited = true;
                if (current != a && current != b && vertices.Contains(current))
                {
                    g.FillRectangle(Brushes.LightBlue, current.Position);
                    g.DrawRectangle(Pens.Black, current.Position);
                    
                }
            }
            List<Vertex<T>> verticesVisited = new List<Vertex<T>>();
            Vertex<T> ToAdd = b;

            while (!verticesVisited.Contains(a) && ToAdd != null&& ToAdd.Founder != null)
            {
                verticesVisited.Add(ToAdd);
                ToAdd = ToAdd.Founder;
            }
            return verticesVisited;
        }
    }
}
