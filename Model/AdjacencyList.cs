using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGT.Model
{
    public class AdjacencyList
    {
        private readonly Dictionary<Vertex, IEnumerable<Vertex>> adjacencyList;

        public AdjacencyList()
        {
            adjacencyList = new Dictionary<Vertex, IEnumerable<Vertex>>();
        }

        public void AddEntry(Vertex from, IEnumerable<Vertex> to)
        {
            adjacencyList.Add(from, to);
        }

        public IEnumerable<Vertex> GetNeighboursFor(Vertex v)
        {
            if (!adjacencyList.TryGetValue(v, out IEnumerable<Vertex> neighbours)) throw new ArgumentException("adjacency list does not contain an entry for vertice " + v.ToString());
            return neighbours;
        }

        public bool HasEntryFor(Vertex from)
        {
            return adjacencyList.Keys.Where(node => from.Equals(node)).Any();
        }

        public string AdjacencyListAsCSV(string delimiter = ";")
        {
            StringBuilder outputBuilder = new StringBuilder("vertex_from;vertices_to").AppendLine();
            foreach (var kvp in adjacencyList)
            {
                outputBuilder.Append($"{kvp.Key};");
                foreach (Vertex to in kvp.Value) outputBuilder.Append($"{to};");
                outputBuilder.Remove(outputBuilder.Length - 1, 1);
                outputBuilder.AppendLine();
            }
            return outputBuilder.ToString();
        }

        public override string ToString()
        {
            var outputBuilder = new StringBuilder();

            foreach (var entry in adjacencyList)
            {
                outputBuilder.Append($"{entry.Key.ToString()}\t->\t");
                foreach (var target in entry.Value)
                {
                    outputBuilder.Append($"{target.ToString()},\t");
                }
                outputBuilder.Remove(outputBuilder.Length - 2, 2);
                outputBuilder.Append(Environment.NewLine);
            }

            return outputBuilder.ToString();
        }
    }
}
