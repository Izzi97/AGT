using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGT.Model;

namespace AGT
{
    public static partial class Algorithms
    {
        public static (Dictionary<Vertex, Vertex> predecessors, Dictionary<Vertex, double> sourceDistances, int queuedVertices, int processedVertices) AStar(WeightedDigraph graph, Vertex source, Vertex target, Func<Vertex, Vertex, double> heuristic)
        {
            if (!graph.Vertices.Contains(source)) throw new ArgumentException("source vertex not contained in vertice set of graph");

            IEnumerable<Vertex> processedVertices = new HashSet<Vertex>();

            Vertex[] tmpVerts = new Vertex[graph.Vertices.Count()];
            graph.Vertices.ToList().CopyTo(tmpVerts);
            HashSet<Vertex> remainingVertices = new HashSet<Vertex>(tmpVerts);

            Dictionary<Vertex, Vertex> predecessors = InitPredecessors(graph, source);
            Dictionary<Vertex, double> sourceDistances = InitDists(graph, source);

            while (!processedVertices.Contains(target))
            {
                Vertex vertex = GetNextVertex(sourceDistances, heuristic, target, remainingVertices);

                processedVertices = processedVertices.Concat(vertex);
                remainingVertices.Remove(vertex);

                if (vertex != target)
                {
                    var remainingNeighbours = graph.AdjacencyList.GetNeighboursFor(vertex).Except(processedVertices);
                    foreach (Vertex neighbour in remainingNeighbours)
                    {
                        if (sourceDistances[vertex] + 6.0 < sourceDistances[neighbour]) //get dist (6.0) from edge instead of hard coded
                        {
                            sourceDistances[neighbour] = sourceDistances[vertex] + 6.0; //get dist (6.0) from edge instead of hard coded
                            predecessors[neighbour] = vertex;
                        }
                    }
                }
            }

            return (predecessors, sourceDistances, sourceDistances.Where(kvp => kvp.Value != double.PositiveInfinity).Select(kvp => kvp.Key).Except(processedVertices).Count(), processedVertices.Count());

            Vertex GetNextVertex(Dictionary<Vertex, double> dists, Func<Vertex, Vertex, double> heur, Vertex t, IEnumerable<Vertex> remainingVerts)
            {
                Vertex min = remainingVerts.Aggregate((v1, v2) =>
                {
                    if (dists[v1] + heur(v1, t) < dists[v2] + heur(v2, t)) return v1;
                    else
                    {
                        if (dists[v1] + heur(v1, t) == dists[v2] + heur(v2, t)) return v1 < v2 ? v1 : v2;
                        else return v2;
                    }
                });

                return min;
            }

            Dictionary<Vertex, Vertex> InitPredecessors(WeightedDigraph g, Vertex s)
            {
                var output = new Dictionary<Vertex, Vertex>();
                foreach (Vertex v in g.Vertices) output.Add(v, null);
                return output;
            }

            Dictionary<Vertex, double> InitDists(WeightedDigraph g, Vertex s)
            {
                var output = new Dictionary<Vertex, double>();
                foreach (Vertex v in g.Vertices)
                {
                    if (v == s) output.Add(s, 0);
                    else output.Add(v, double.PositiveInfinity);
                }
                return output;
            }
        }

        public static string AStarResultTableToExcel((Dictionary<Vertex, Vertex> predecessors, Dictionary<Vertex, double> sourceDistances, int discoveredVertsCnt, int processedVertsCnt) aStarResult)
        {
            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.AppendLine("vertex;predecessor;source_distance");

            foreach (Vertex v in aStarResult.predecessors.Keys)
            {
                string predecessor = !Equals(aStarResult.predecessors[v], null) ? aStarResult.predecessors[v].ToString() : "nil";
                resultBuilder.AppendLine($"{v};{predecessor};{aStarResult.sourceDistances[v]}");
            }

            resultBuilder
                .AppendLine()
                .AppendLine("vertices_remaining_queue;vertices_processed")
                .AppendLine($"{aStarResult.discoveredVertsCnt};{aStarResult.processedVertsCnt}");

            return resultBuilder.ToString();
        }
    }
}
