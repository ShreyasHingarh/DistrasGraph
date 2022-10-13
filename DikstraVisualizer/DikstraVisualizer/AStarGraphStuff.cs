using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DikstraVisualizer
{

    public enum Result
    {
        Enqueue,
        Dequeue,
    }
    public record struct Information(Result result, Rectangle Position);
    public class GraphForAStar<T> : GraphFunctions<T>
    {
        public int Scalar;
        public int Scalar2;
        PriorityQueue<Vertex<T>, float> Queue;
        List<Vertex<T>> list;
        Vertex<T> current;
        int Index;



        static readonly Func<int, int, int, int, int, int, int>[] Funcs = new Func<int, int, int, int, int, int, int>[]
        {
            HeurManhattan,
            HeurEuclidean,
            HeurOctile,
            HeurChebyshev
        };

        public GraphForAStar(int scalar, int scalar2)
        {
            Scalar = scalar;
            Scalar2 = scalar2;
            Queue = new PriorityQueue<Vertex<T>, float>();
            list = new List<Vertex<T>>();
            Index = 0;

        }

        public static int HeurManhattan(int nodeX, int nodeY, int goalX, int goalY, int Scalar, int Scalar2)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(nodeY - goalY);
            return Scalar * (dx + dy);
        }
        public static int HeurChebyshev(int nodeX, int nodeY, int goalX, int goalY, int Scalar, int Scalar2)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(nodeY - goalY);
            return Scalar * (dx + dy) + (Scalar2 - 2 * Scalar) * Math.Min(dx, dy);
        }
        public static int HeurOctile(int nodeX, int nodeY, int goalX, int goalY, int Scalar, int Scalar2)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(nodeY - goalY);
            return Scalar * (dx + dy) + (int)(Math.Sqrt(2) - 2 * Scalar) * Math.Min(dx, dy);
        }
        public static int HeurEuclidean(int nodeX, int nodeY, int goalX, int goalY, int Scalar, int Scalar2)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(nodeY - goalY);
            return (int)(Scalar * Math.Sqrt(dx * dx + dy * dy));
        }
        private Information WhenDequeue()
        {
            current = Queue.Dequeue();
            Index = 0; 
            current.HasBeenVisited = true;
            return new Information(Result.Dequeue, current.Position);

        }
        private void HelperOne(float tentativeDistance, int HeurType, Edge<T> curEdge, Vertex<T> b)
        {
            curEdge.EndingPoint.CumlativeDistance = tentativeDistance;
            curEdge.EndingPoint.FinalDistance = curEdge.EndingPoint.CumlativeDistance + Funcs[HeurType](curEdge.EndingPoint.Position.X, curEdge.EndingPoint.Position.Y, b.Position.X, b.Position.Y, Scalar, Scalar2);

            curEdge.EndingPoint.Founder = current;

        }
        public bool MainAStarPart(Vertex<T> b, int HeurType, out Information info)
        {
            if(Queue.Count == 0)
            {
                info = new Information();
                return false;
            }
            if (!b.HasBeenVisited)
            {
                
                if (Index >= current.NeighborCount)
                {
                    info = WhenDequeue();

                    return true;
                }
                else
                {
                    Edge<T> curEdge = current.Neighbors[Index];

                    float tentativeDistance = current.CumlativeDistance + curEdge.Distance;
                    if (curEdge.EndingPoint.HasBeenVisited || list.Contains(curEdge.EndingPoint))
                    {
                        do
                        {
                            if (Index >= current.NeighborCount)
                            {
                                info = WhenDequeue();
                                return true;
                            }
                            if (tentativeDistance < curEdge.EndingPoint.CumlativeDistance)
                            {
                                HelperOne(tentativeDistance, HeurType, curEdge, b);
                            }
                            curEdge = current.Neighbors[Index++];

                        } while (curEdge.EndingPoint.HasBeenVisited || list.Contains(curEdge.EndingPoint));
                    }
                    else
                    {
                        Index++;
                    }
                    if(tentativeDistance < curEdge.EndingPoint.CumlativeDistance)
                    {
                        HelperOne(tentativeDistance, HeurType, curEdge, b);
                    }
                   
                    Queue.Enqueue(curEdge.EndingPoint, curEdge.EndingPoint.FinalDistance);
                    list.Add(curEdge.EndingPoint);
                    info = new Information(Result.Enqueue, curEdge.EndingPoint.Position);
                   
                    return true;
                }
                

            }
            info = new Information();
            return false;
        }
        public bool SetupAStar(Vertex<T> a, Vertex<T> b, int HeurType)
        {
            if (a == null || b == null || a == b || a.NeighborCount == 0)
            {
                return false;
            }
            foreach (var vertex in vertices)
            {
                vertex.HasBeenVisited = false;
                vertex.CumlativeDistance = float.PositiveInfinity;
                vertex.FinalDistance = float.PositiveInfinity;
                vertex.Founder = null;
            }
            a.CumlativeDistance = 0;

            a.FinalDistance = a.CumlativeDistance + Funcs[HeurType](a.Position.X, a.Position.Y, b.Position.X, b.Position.Y, Scalar, Scalar2);

            Queue = new PriorityQueue<Vertex<T>, float>();
            list = new List<Vertex<T>>();

            //Queue.Enqueue(a, a.FinalDistance);
            list.Add(a);
            current = a;
            return true;
        }
        public List<Vertex<T>> EndingAStar(Vertex<T> a, Vertex<T> b)
        {
            List<Vertex<T>> verticesVisited = new List<Vertex<T>>();
            Vertex<T> ToAdd = b;

            while (!verticesVisited.Contains(a) && ToAdd != null && ToAdd.Founder != null)
            {
                verticesVisited.Add(ToAdd);
                ToAdd = ToAdd.Founder;
            }
            return verticesVisited;
        }

    }
}
