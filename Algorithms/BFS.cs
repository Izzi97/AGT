using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AGT.Model;

namespace AGT
{
    public static partial class Algorithms
    {
        public static Dictionary<Vertex, (Vertex predecessor, double sourceDistance, int index)> BFS(Graph graph, Vertex source, Vertex target)
        {
            if (!graph.Vertices.Contains(source)) throw new ArgumentException("start vertex not contained in vertice set of graph");

            Vertex[] vertsTmp = new Vertex[graph.Vertices.Count()];
            graph.Vertices.ToList().CopyTo(vertsTmp);
            IEnumerable<Vertex> vertices = vertsTmp;
            var adjacencyList = graph.AdjacencyList;
            var queue = new Queue<Vertex>();

            queue.Enqueue(source);
            vertices = vertices.Except(source);

            int sourceDistance = 0;
            int index = 1;
            Dictionary<Vertex, (Vertex predecessor, double sourceDistance, int index)> result = InitResult(graph, source);

            while (queue.Count != 0)
            {
                var head = queue.Dequeue();
                var neighbours = adjacencyList.GetNeighboursFor(head);

                foreach (var neighbour in neighbours)
                {
                    if (vertices.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        vertices = vertices.Except(neighbour);
                        result[neighbour] = (head, sourceDistance + 6, ++index);
                        if (Equals(neighbour, target)) return result;
                    }
                }

                sourceDistance += 6;
            }

            return result;

            Dictionary<Vertex, (Vertex predecessor, double sourceDistance, int index)> InitResult(Graph g, Vertex s)
            {
                Dictionary<Vertex, (Vertex predecessor, double sourceDistance, int index)> output = new Dictionary<Vertex, (Vertex, double, int)>();
                foreach (Vertex v in g.Vertices) output.Add(v, (null, Equals(v, s) ? 0 : double.PositiveInfinity, -1));
                return output;
            }
        }

        public static string BFSResultAsCSV(Dictionary<Vertex, (Vertex predecessor, double sourceDistance, int index)> bfsResult)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.AppendLine("vertice;predecessor;root_distance;index");

            var verticeMappingList = bfsResult.ToList();

            foreach (var kvp in verticeMappingList)
            {
                string vertice = kvp.Key.ToString();
                string predecessor = !Equals(kvp.Value.predecessor, null) ? kvp.Value.predecessor.ToString() : "nil";
                string sourceDist = kvp.Value.sourceDistance.ToString();
                string index = kvp.Value.index.ToString();
                resultBuilder.AppendLine($"{vertice};{predecessor};{sourceDist};{index}");
            }

            return resultBuilder.ToString();
        }
    }
}
