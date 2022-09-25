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
    public class Vertex <T> 
    {
        public float CumlativeDistance;
        public Vertex<T> Founder;
        public bool HasBeenVisited;
        public T Value { get; set; }
        public List<Edge<T>> Neighbors { get; set; }

        public float FinalDistance;
        public Rectangle Position { get; set; }

        public int NeighborCount => Neighbors.Count;

        public Vertex(T value, Rectangle positions) 
        {
            Position = positions;
            CumlativeDistance = float.PositiveInfinity;
            FinalDistance = float.PositiveInfinity;
            Founder = null;
            HasBeenVisited = false;
            Neighbors = new List<Edge<T>>();
            Value = value;
        }
    }
}
