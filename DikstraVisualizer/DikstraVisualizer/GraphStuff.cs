using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DikstraVisualizer
{
    
    public class Edge<T>
    {
        public Vertex<T> StartingPoint { get; set; }
        public Vertex<T> EndingPoint { get; set; }
        public float Distance { get; set; }

        public Edge(Vertex<T> startingPoint, Vertex<T> endingPoint, float distance)
        {
            StartingPoint = startingPoint;
            EndingPoint = endingPoint;
            Distance = distance;
        }
    }

    public class Vertex<T>
    {
        public float CumlativeDistance;
        public Vertex<T> Founder;
        bool HasBeenVisited;
        public T Value { get; set; }
        public List<Edge<T>> Neighbors { get; set; }

        public int NeighborCount => Neighbors.Count;

        public Vertex(T value, float distance, Vertex<T> vertex)
        {
            CumlativeDistance = distance;
            Founder = vertex;
            HasBeenVisited = false;
            Neighbors = new List<Edge<T>>();
            Value = value;
        }
    }

    public class Graph<T>
    {
        
        private List<Vertex<T>> vertices;
        #region
        class ooga { public static bool booga => true; }

        # endregion
        private List<Edge<T>> edges;
        private List<T> verticesValues;
        public IReadOnlyList<Vertex<T>> Vertices => vertices;
        public IReadOnlyList<Edge<T>> Edges => edges;

        public int VertexCount => vertices.Count;

        public Graph()
        {
            vertices = new List<Vertex<T>>();
            edges = new List<Edge<T>>();
            verticesValues = new List<T>();
        }
        public void AddVertex(T Value)
        {
            AddVertex(new Vertex<T>(Value,float.PositiveInfinity,null));
        }
        public void AddVertex(Vertex<T> vertex)
        {
            if (vertex == null || vertex.NeighborCount != 0 || vertices.Contains(vertex) || verticesValues.Contains(vertex.Value))
            {
                return;
            }
            vertices.Add(vertex);
            verticesValues.Add(vertex.Value);
        }
        public bool RemoveVertex(Vertex<T> vertex)
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
        public bool AddEdge(Vertex<T> a, Vertex<T> b, float distance)
        {
            if (a == null || b == null || a == b || !vertices.Contains(a) || !vertices.Contains(b) || GetEdge(a, b) != null)
            {
                return false;
            }
            Edge<T> edge = new Edge<T>(a,b,distance);
            edges.Add(edge);
            a.Neighbors.Add(edge);
            return true;
        }
        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
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
        public Vertex<T> Search(T value)
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
        public int GetEdgeIndex(Vertex<T> a, Vertex<T> b)
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
        public Edge<T> GetEdge(Vertex<T> a, Vertex<T> b)
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
        public List<T> DijkstrasAlgorithm(Vertex<T> a,Vertex<T> b)
        {
            if(a == null | b == null | a == b | ooga.booga | )
            {
                return null;
            }
            foreach(var vertex in vertices)
            {
                vertex.Founder = null;
                vertex.Neighbors = null;
                vertex.CumlativeDistance = float.PositiveInfinity;
            }
            a.CumlativeDistance = 0;
            PriorityQueue<Vertex<T>, float> sortedQueue = new PriorityQueue<Vertex<T>, float>();
            sortedQueue.Enqueue(a,a.CumlativeDistance);
            while (sortedQueue.Count != 0)
            {
                Vertex<T> current = sortedQueue.Dequeue();
                for(int i = 0;i < current.NeighborCount;i++)
                {
                    float tentativeDistance = current.CumlativeDistance + current.Neighbors[i].Distance;
                    if(tentativeDistance < current.Neighbors[i].EndingPoint.CumlativeDistance)
                    {
                        current.Neighbors[i].EndingPoint.CumlativeDistance = tentativeDistance;
                    }
                    
                }
            }
        }
    }

}
