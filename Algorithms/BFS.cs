using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AGT.Model;

namespace AGT
{
    public static partial class Algorithms
    {
        public static (
            Dictionary<Vertex, Vertex> predecessors, 
            Dictionary<Vertex, double> sourceDistances, 
            Dictionary<Vertex, int> index, 
            int queuedVerticesCnt, 
            int processedVerticesCnt
        )
        BFS(Graph graph, Vertex source, Vertex target)
        {
            if (!graph.Vertices.Contains(source)) throw new ArgumentException("start vertex not contained in vertice set of graph");

            Vertex[] vertsTmp = new Vertex[graph.Vertices.Count()];
            graph.Vertices.ToList().CopyTo(vertsTmp);
            IEnumerable<Vertex> vertices = vertsTmp;
            var adjacencyList = graph.AdjacencyList;
            var queue = new Queue<Vertex>();
            var processedVerts = new List<Vertex>();

            var result = InitResult(graph, source);

            queue.Enqueue(source);
            vertices = vertices.Except(source);

            int index = 1;

            while (queue.Count != 0)
            {
                var head = queue.Dequeue();
                processedVerts.Add(head);
                var neighbours = adjacencyList.GetNeighboursFor(head);

                foreach (var neighbour in neighbours)
                {
                    if (vertices.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        vertices = vertices.Except(neighbour);
                        result.predecessors[neighbour] = head;
                        result.sourceDistances[neighbour] = result.sourceDistances[head] + 6;
                        result.index[neighbour] = ++index;

                        if (Equals(neighbour, target))
                        {
                            result.queuedVerticesCnt = queue.Count();
                            result.processedVertsCnt = processedVerts.Count();
                            return result;
                        }
                    }
                }
            }

            result.queuedVerticesCnt = queue.Count();
            result.processedVertsCnt = processedVerts.Count();
            return result;

            (
            Dictionary<Vertex, Vertex> predecessors,
            Dictionary<Vertex, double> sourceDistances,
            Dictionary<Vertex, int> index,
            int queuedVerticesCnt,
            int processedVertsCnt
            ) 
            InitResult(Graph g, Vertex s)
            {
                (Dictionary<Vertex, Vertex> predecessors, Dictionary<Vertex, double> sourceDistances, Dictionary<Vertex, int> index, int queuedVerticesCnt, int processedVertsCnt) output =
                    (new Dictionary<Vertex, Vertex>(), new Dictionary<Vertex, double>(), new Dictionary<Vertex, int>(), 0, 0);
                foreach (Vertex v in g.Vertices)
                {
                    output.predecessors[v] = null;
                    output.sourceDistances[v] = Equals(v, s) ? 0 : double.PositiveInfinity;
                    output.index[v] = Equals(v, s) ? 0 : -1;
                }

                return output;
            }
        }

        public static string BFSResultToExcel((Dictionary<Vertex, Vertex> predecessors, Dictionary<Vertex, double> sourceDistances, Dictionary<Vertex, int> index, int queuedVerticesCnt, int processedVertsCnt) bfsResult)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.AppendLine("vertex;predecessor;source_distance;index");

            foreach (Vertex v in bfsResult.predecessors.Keys)
            {
                string predecessor = !Equals(bfsResult.predecessors[v], null) ? bfsResult.predecessors[v].ToString() : "nil";
                string sourceDist = bfsResult.sourceDistances[v].ToString();
                string index = bfsResult.index[v].ToString();
                resultBuilder.AppendLine($"{v};{predecessor};{sourceDist};{index}");
            }
            resultBuilder
                .AppendLine()
                .AppendLine("vertices_discovered;vertices_processed")
                .AppendLine($"{bfsResult.queuedVerticesCnt};{bfsResult.processedVertsCnt}");

            return resultBuilder.ToString();
        }
    }
}
