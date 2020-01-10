using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AGT
{
    public static partial class Algorithms
    {
        public static Dictionary<Vertice, (Vertice predecessor, int rootDistance, int index)> BFS(Graph graph, Vertice startVertice)
        {
            if (!graph.Vertices.Contains(startVertice)) throw new ArgumentException("start vertice not contained in vertice set of graph");

            Vertice[] tmpVerts = new Vertice[graph.Vertices.Count];
            graph.Vertices.CopyTo(tmpVerts);
            var vertices = new HashSet<Vertice>(tmpVerts);
            var adjacencyList = graph.AdjecencyList;

            var queue = new Queue<Vertice>();
            queue.Enqueue(startVertice);
            vertices.Remove(startVertice);

            int rootDistance = 0;
            int index = 1;
            var result = new Dictionary<Vertice, (Vertice predecessor, int rootDistance, int index)>();

            result.Add(startVertice, (null, rootDistance, index));

            while (queue.Count != 0)
            {
                var head = queue.Dequeue();
                var neighbours = adjacencyList.GetNeighboursFor(head);

                foreach (var neighbour in neighbours)
                {
                    if (vertices.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        vertices.Remove(neighbour);
                        result.Add(neighbour, (head, rootDistance + 1, ++index));
                    }
                }

                rootDistance++;
            }

            return result;
        }

       
        public static string BFSResultAsCSV(Dictionary<Vertice, (Vertice predecessor, int rootDistance, int index)> bfsResult)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.AppendLine("vertice;predecessor;root_distance;index");

            var verticeMappingList = bfsResult.ToList();
            verticeMappingList.Sort((kvp1, kvp2) => kvp1.Value.index.CompareTo(kvp1.Value.index));

            foreach (var kvp in verticeMappingList)
            {
                string vertice = kvp.Key.ToString();
                string predecessor = kvp.Value.predecessor != null ? kvp.Value.predecessor.ToString() : "nil";
                string root_dist = kvp.Value.rootDistance.ToString();
                string index = kvp.Value.index.ToString();
                resultBuilder.AppendLine($"{vertice};{predecessor};{root_dist};{index}");
            }

            return resultBuilder.ToString();
        }
    }
}
