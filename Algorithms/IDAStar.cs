using System;
using System.Linq;
using System.Collections.Generic;
using AGT.Model;

namespace AGT
{
    public static partial class Algorithms
    {
        public static (bool found, IEnumerable<Vertex> path, double distance, Dictionary<Vertex, double> sourceDistances) IDAStar(
            WeightedDigraph graph,
            Vertex source,
            Vertex target,
            Func<Vertex, Vertex, double> heuristic
        )
        {
            List<Vertex> path = new List<Vertex>();
            Dictionary<Vertex, double> sourceDistances = InitDists(graph, source);
            double bound = heuristic(source, target);
            (bool found, double distance) result;

            while (true)
            {
                result = IDASearch(source, 0, bound);

                if (result.found)
                {
                    path.Add(source);
                    path.Reverse();
                    return (true, path, result.distance, sourceDistances);
                }
                if (!result.found && result.distance == double.PositiveInfinity) return (false, null, double.PositiveInfinity, sourceDistances);
                bound = result.distance;
            }

            (bool found, double distance) IDASearch(Vertex v, double d, double b)
            {
                double f = d + heuristic(v, target);

                if (f > b) return (false, f);
                if (v == target) return (true, d);

                double min = double.PositiveInfinity;
                foreach (Vertex w in graph.AdjacencyList.GetNeighboursFor(v))
                {
                    var nb = IDASearch(w, d + 6, b);
                    sourceDistances[w] = nb.distance;

                    if (nb.found)
                    {
                        path.Add(w);
                        return (true, nb.distance);
                    }
                    if (nb.distance < min) min = nb.distance;
                }

                return (false, min);
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
    }
}
