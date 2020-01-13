using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AGT.Model;

namespace AGT
{
    public static partial class Algorithms
    {
        public static Dictionary<Vertex, (Vertex predecessor, int rootDistance, int index)> BFS(Graph graph, Vertex startVertice)
        {
            if (!graph.Vertices.Contains(startVertice)) throw new ArgumentException("start vertex not contained in vertice set of graph");

            Vertex[] vertsTmp = new Vertex[graph.Vertices.Count()];
            graph.Vertices.ToList().CopyTo(vertsTmp);
            IEnumerable<Vertex> vertices = vertsTmp;
            var adjacencyList = graph.AdjacencyList;
            var queue = new Queue<Vertex>();

            queue.Enqueue(startVertice);
            vertices = vertices.Except(startVertice);

            int rootDistance = 0;
            int index = 1;
            var result = new Dictionary<Vertex, (Vertex predecessor, int rootDistance, int index)>
            {
                { startVertice, (null, rootDistance, index) }
            };

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
                        result.Add(neighbour, (head, rootDistance + 1, ++index));
                    }
                }

                rootDistance++;
            }

            return result;
        }

        public static string BFSResultAsCSV(Dictionary<Vertex, (Vertex predecessor, int rootDistance, int index)> bfsResult)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.AppendLine("vertice;predecessor;root_distance;index");

            var verticeMappingList = bfsResult.ToList();

            foreach (var kvp in verticeMappingList)
            {
                string vertice = kvp.Key.ToString();
                string predecessor = !Equals(kvp.Value.predecessor, null) ? kvp.Value.predecessor.ToString() : "nil";
                string root_dist = kvp.Value.rootDistance.ToString();
                string index = kvp.Value.index.ToString();
                resultBuilder.AppendLine($"{vertice};{predecessor};{root_dist};{index}");
            }

            return resultBuilder.ToString();
        }
    }
}
