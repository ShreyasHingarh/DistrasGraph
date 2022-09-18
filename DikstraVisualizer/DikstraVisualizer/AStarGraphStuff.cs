using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DikstraVisualizer
{
    public class EdgeForA<T>
    {
        public VertexForA<T> StartingPoint { get; set; }
        public VertexForA<T> EndingPoint { get; set; }
        public float Distance { get; set; }

        public EdgeForA(VertexForA<T> startingPoint, VertexForA<T> endingPoint, float distance)
        {
            StartingPoint = startingPoint;
            EndingPoint = endingPoint;
            Distance = distance;
        }
    }
    public class VertexForA<T>
    {
        public float CumlativeDistance;
        public VertexForA<T> Founder;
        public bool HasBeenVisited;
        public T Value { get; set; }
        public List<EdgeForA<T>> Neighbors { get; set; }

        public float FinalDistance;
        public Rectangle Position { get; set; }

        public int NeighborCount => Neighbors.Count;

        public VertexForA(T value,Rectangle positions)
        {
            Position = positions;
            CumlativeDistance = float.PositiveInfinity;
            FinalDistance = float.PositiveInfinity;
            Founder = null;
            HasBeenVisited = false;
            Neighbors = new List<EdgeForA<T>>();
            Value = value;
        }
    }
    public class GraphForAStar<T>
    {
        public int Scalar;
        public int Scalar2;
        
        #region GraphStuff
        private List<VertexForA<T>> vertices;
        #region
        class ooga { public static bool booga => true; }

        # endregion
        private List<EdgeForA<T>> edges;
        private List<T> verticesValues;

        public int VertexForACount => vertices.Count;

        public GraphForAStar(int scalar, int scalar2)
        {
            vertices = new List<VertexForA<T>>();
            edges = new List<EdgeForA<T>>();
            verticesValues = new List<T>();
            Scalar = scalar;
            Scalar2 = scalar2;
        }
        public void AddVertex(T Value,Rectangle position)
        {
            AddVertex(new VertexForA<T>(Value,position));
        }
        public void AddVertex(VertexForA<T> vertex)
        {
            if (vertex == null || vertex.NeighborCount != 0 || vertices.Contains(vertex) || verticesValues.Contains(vertex.Value))
            {
                return;
            }
            vertices.Add(vertex);
            verticesValues.Add(vertex.Value);
        }
        public bool RemoveVertex(VertexForA<T> vertex)
        {
            if (!vertices.Contains(vertex))
            {
                return false;
            }

            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].EndingPoint == vertex)
                {
                    RemoveEdge(edges[i].StartingPoint, vertex);
                    i--;
                }
                else if (edges[i].StartingPoint == vertex)
                {
                    RemoveEdge(vertex, edges[i].EndingPoint);
                    i--;
                }
            }
            vertices.Remove(vertex);
            verticesValues.Remove(vertex.Value);
            return true;
        }
        public bool AddEdge(VertexForA<T> a, VertexForA<T> b, float distance)
        {
            if (a == null || b == null || a == b || !vertices.Contains(a) || !vertices.Contains(b) || GetEdge(a, b) != null)
            {
                return false;
            }
            EdgeForA<T> edge = new EdgeForA<T>(a, b, distance);
            edges.Add(edge);
            a.Neighbors.Add(edge);
            return true;
        }
        public bool RemoveEdge(VertexForA<T> a, VertexForA<T> b)
        {
            if (a == null || b == null || GetEdge(a, b) == null)
            {
                return false;
            }
            a.Neighbors.Remove(GetEdge(a, b));
            edges.Remove(GetEdge(a, b));

            return true;
        }
        public int SearchForVertexIndex(T value)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Value.Equals(value))
                {
                    return i;
                }
            }
            return -1;
        }
        public VertexForA<T> Search(T value)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Value.Equals(value))
                {
                    return vertices[i];
                }
            }
            return null;
        }
        public int GetEdgeIndex(VertexForA<T> a, VertexForA<T> b)
        {
            if (a == null || b == null)
            {
                return -1;
            }
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].StartingPoint == a && edges[i].EndingPoint == b)
                {
                    return i;
                }
            }
            return -1;
        }
        public EdgeForA<T> GetEdge(VertexForA<T> a, VertexForA<T> b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].StartingPoint == a && edges[i].EndingPoint == b)
                {
                    return edges[i];
                }
            }
            return null;
        }
        #endregion GraphStuff

        public int HeurManhattan(int nodeX,int nodeY,int goalX,int goalY)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(-nodeY - goalY);
            return Scalar * (dx + dy);
        }
        public int HeurDiagonal(int nodeX, int nodeY, int goalX, int goalY)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(nodeY - goalY);
            return Scalar * (dx + dy) + (Scalar2 - 2 * Scalar) * Math.Min(dx,dy);
        }
        public int HeurEuclidean(int nodeX, int nodeY, int goalX, int goalY)
        {
            int dx = Math.Abs(nodeX - goalX);
            int dy = Math.Abs(nodeY - goalY);
            return (int)(Scalar * Math.Sqrt(dx * dx + dy * dy));
        }
        public List<VertexForA<T>> AStarSearchAlgorithm(VertexForA<T> a, VertexForA<T> b, List<VertexForA<T>> walls,Action HeurType)
        {
            if(a == null | b == null | a == b | !ooga.booga | a.NeighborCount == 0)
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
            
            a.FinalDistance = a.CumlativeDistance + HeurEuclidean(a.Position.X,a.Position.Y,b.Position.X,b.Position.Y);

            PriorityQueue<VertexForA<T>, float> queue = new PriorityQueue<VertexForA<T>, float>();
            List<VertexForA<T>> list = new List<VertexForA<T>>();

            queue.Enqueue(a, a.FinalDistance);
            list.Add(a);
            while (queue.Count != 0 && !b.HasBeenVisited)
            {
                VertexForA<T> current = queue.Dequeue();

                for(int i = 0;i < current.NeighborCount;i++)
                {
                    EdgeForA<T> curEdge = current.Neighbors[i];
                    float tentativeDistance = current.CumlativeDistance + curEdge.Distance;
                    if(tentativeDistance < curEdge.EndingPoint.CumlativeDistance)
                    {
                        curEdge.EndingPoint.CumlativeDistance = tentativeDistance;
                        curEdge.EndingPoint.Founder = current;
                        curEdge.EndingPoint.FinalDistance = curEdge.EndingPoint.CumlativeDistance + HeurEuclidean(curEdge.EndingPoint.Position.X, curEdge.EndingPoint.Position.Y,b.Position.X,b.Position.Y);
                    }
                    if(!curEdge.EndingPoint.HasBeenVisited && !list.Contains(curEdge.EndingPoint))
                    {
                        queue.Enqueue(curEdge.EndingPoint,curEdge.EndingPoint.FinalDistance);
                        list.Add(curEdge.EndingPoint);
                    }
                }
                current.HasBeenVisited = true;

            }
            List<VertexForA<T>> verticesVisited = new List<VertexForA<T>>();
            VertexForA<T> ToAdd = b;
            while (!verticesVisited.Contains(a))
            {
                verticesVisited.Add(ToAdd);
                ToAdd = ToAdd.Founder;
            }
            return verticesVisited;
        }
    }
}
