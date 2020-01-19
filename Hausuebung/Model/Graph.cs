using System.Collections.Generic;

namespace AGT.Model
{
    public class Graph
    {
        public IEnumerable<Vertex> Vertices { get; }
        public AdjacencyList AdjacencyList { get; }
        public Graph(IEnumerable<Vertex> vertices, AdjacencyList adjacencyList)
        {
            Vertices = vertices;
            AdjacencyList = adjacencyList;
        }
    }

    public class WeightedDigraph : Graph
    {
        public IEnumerable<WeightedEdge> WeightedEdges;
        public WeightedDigraph(IEnumerable<Vertex> vertices, AdjacencyList adjacencyList, IEnumerable<WeightedEdge> edges) : base(vertices, adjacencyList)
        {
            WeightedEdges = edges;
        }
    }
}