using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace DikstraVisualizer
{
    public class GraphFunctions<T>
    {
        public List<Vertex<T>> vertices;
        public List<Edge<T>> edges;
        private List<T> verticesValues;

        public int VertexCount => vertices.Count;

        public GraphFunctions()
        {
            vertices = new List<Vertex<T>>();
            edges = new List<Edge<T>>();
            verticesValues = new List<T>();
        }
        public void AddVertex(T Value, Rectangle position)
        {
            AddVertex(new Vertex<T>(Value, position));
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
            Edge<T> edge = new Edge<T>(a, b, distance);
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
    }
}
