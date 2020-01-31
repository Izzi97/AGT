using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AGT.Model;

namespace AGT
{
    public static partial class Algorithms
    {
        public static AlgorithmResult BFS(Graph graph, Vertex source, Vertex target)
        {
            if (!graph.Vertices.Contains(source)) throw new ArgumentException("start vertex not contained in vertice set of graph");

            Vertex[] vertsTmp = new Vertex[graph.Vertices.Count()];
            graph.Vertices.ToList().CopyTo(vertsTmp);
            IEnumerable<Vertex> vertices = vertsTmp;
            var adjacencyList = graph.AdjacencyList;
            var queue = new Queue<Vertex>();
            var processedVerts = new List<Vertex>();

            var init = Init(graph, source);
            var predecessors = init.predecessors;
            var distances = init.sourceDistances;
            var indices = init.index;
            int queuedVerticesCnt = 0;
            int processedVertsCnt = 0;

            var protocol = new AlgorithmProtocol();
            var firstProtocolStep = new AlgorithmProtocolStep(
                null,
                new Dictionary<Vertex, (Vertex, double)>(predecessors.Select(kvp => new KeyValuePair<Vertex, (Vertex, double)>(kvp.Key, (kvp.Value, distances[kvp.Key]))))
            );
            protocol.AddStep(firstProtocolStep);

            queue.Enqueue(source);
            vertices = vertices.Except(source);

            int index = 1;

            while (queue.Count != 0)
            {
                var head = queue.Dequeue();
                processedVerts.Add(head);
                var neighbours = adjacencyList.GetNeighboursFor(head);
                var nextProtocolStep = new AlgorithmProtocolStep(head, new Dictionary<Vertex, (Vertex, double)>(protocol.Steps.Last().Table.Select(entry => entry)));

                foreach (var neighbour in neighbours)
                {
                    if (vertices.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        vertices = vertices.Except(neighbour);
                        predecessors[neighbour] = head;
                        distances[neighbour] = distances[head] + 6;
                        indices[neighbour] = ++index;
                        nextProtocolStep.AddOrUpdate(neighbour, head, distances[neighbour]);

                        if (Equals(neighbour, target))
                        {
                            queuedVerticesCnt = queue.Count();
                            processedVertsCnt = processedVerts.Count();
                            return new AlgorithmResult(protocol, queuedVerticesCnt, processedVertsCnt);
                        }
                    }
                }

                protocol.AddStep(nextProtocolStep);
            }

            queuedVerticesCnt = queue.Count();
            processedVertsCnt = processedVerts.Count();
            return new AlgorithmResult(protocol, queuedVerticesCnt, processedVertsCnt);

            (
            Dictionary<Vertex, Vertex> predecessors,
            Dictionary<Vertex, double> sourceDistances,
            Dictionary<Vertex, int> index
            ) 
            Init(Graph g, Vertex s)
            {
                (Dictionary<Vertex, Vertex> predecessors, Dictionary<Vertex, double> sourceDistances, Dictionary<Vertex, int> index) tmp =
                    (new Dictionary<Vertex, Vertex>(), new Dictionary<Vertex, double>(), new Dictionary<Vertex, int>());
                foreach (Vertex v in g.Vertices)
                {
                    tmp.predecessors[v] = null;
                    tmp.sourceDistances[v] = Equals(v, s) ? 0 : double.PositiveInfinity;
                    tmp.index[v] = Equals(v, s) ? 0 : -1;
                }

                return tmp;
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
