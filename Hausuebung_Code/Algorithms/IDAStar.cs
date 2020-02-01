using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGT.Model;

namespace AGT
{
    public static partial class Algorithms
    {
        public static AlgorithmResult IDAStar(
            WeightedDigraph graph,
            Vertex source,
            Vertex target,
            Func<Vertex, Vertex, double> heuristic
        )
        {
            Dictionary<Vertex, Vertex> predecessors;
            Dictionary<Vertex, double> sourceDistances;
            (predecessors, sourceDistances) = Init(graph, source);
            double bound = heuristic(source, target);

            var protocol = new AlgorithmProtocol();
            var initialProtocolStep = new AlgorithmProtocolStep(
                null,
                new Dictionary<Vertex, (Vertex, double)>(predecessors.Select(kvp => new KeyValuePair<Vertex, (Vertex, double)>(kvp.Key, (kvp.Value, sourceDistances[kvp.Key]))))
            );
            protocol.AddStep(initialProtocolStep);

            (bool found, double distance) result;
            while (true)
            {
                var nextProtocolStep = new AlgorithmProtocolStep(source, new Dictionary<Vertex, (Vertex, double)>(protocol.Steps.Last().Table.Select(entry => entry)));
                result = IDASearch(source, 0, bound, nextProtocolStep);
                protocol.AddStep(nextProtocolStep);

                if (result.found)
                {
                    return new AlgorithmResult(protocol, -1, -1);
                }
                if (!result.found && result.distance == double.PositiveInfinity) return new AlgorithmResult(protocol, -1, -1);
                bound = result.distance;
            }

            (bool found, double distance) IDASearch(Vertex v, double d, double b, AlgorithmProtocolStep protocolStep)
            {
                double f = d + heuristic(v, target);

                if (f > b) return (false, f);
                if (v == target) return (true, d);

                double min = double.PositiveInfinity;
                foreach (Vertex w in graph.AdjacencyList.GetNeighboursFor(v))
                {
                    var nb = IDASearch(w, d + 6, b, protocolStep);
                    sourceDistances[w] = nb.distance;
                    protocolStep.AddOrUpdate(w, protocolStep.Table[w].predecessor, sourceDistances[w]);

                    if (nb.found)
                    {
                        predecessors[w] = v;
                        protocolStep.AddOrUpdate(w, v, sourceDistances[w]);
                        return (true, nb.distance);
                    }
                    if (nb.distance < min) min = nb.distance;
                }

                return (false, min);
            }

            (Dictionary<Vertex, Vertex>, Dictionary<Vertex, double>) Init(WeightedDigraph g, Vertex s)
            {
                var tmp = (preds: new Dictionary<Vertex, Vertex>(), dists: new Dictionary<Vertex, double>());
                foreach (Vertex v in g.Vertices)
                {
                    tmp.preds[v] = null;
                    if (v == s) tmp.dists[s] = 0;
                    else tmp.dists[v] = double.PositiveInfinity;
                }
                return tmp;
            }
        }

        public static string IDAStarResultTableToExcel((bool found, IEnumerable<Vertex> path, double distance, Dictionary<Vertex, double> sourceDistances) IDAStarResult)
        {
            var outputBuilder = new StringBuilder();
            outputBuilder.AppendLine("vertex;source_distance");
            
            foreach (var kvp in IDAStarResult.sourceDistances)
            {
                outputBuilder.AppendLine($"{kvp.Key};{kvp.Value}");
            }

            outputBuilder
                .AppendLine()
                .AppendLine("vertices_traversed;vertices_total")
                .AppendLine($"{IDAStarResult.sourceDistances.Where(kvp => kvp.Value != double.PositiveInfinity).Count()};{IDAStarResult.sourceDistances.Count()}");

            return outputBuilder.ToString();
        }
    }
}
