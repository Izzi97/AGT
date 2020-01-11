using System;
using System.Collections.Generic;
using System.Linq;

namespace AGT
{
    public static partial class Algorithms
    {
        public static Dictionary<Vertex, (Vertex predecessor, double sourceDistance)> AStar(WeightedDigraph graph /*unneccessary - figure something out*/, Vertex source, Vertex target, Func<Vertex, Vertex, double> heuristic)
        {
            if (!graph.Vertices.Contains(source)) throw new ArgumentException("source vertex not contained in vertice set of graph");

            Dictionary<Vertex, double> sourceDistances = initSourceDistances(graph, source);

            HashSet<Vertex> processedVertices = new HashSet<Vertex>();

            Vertex[] tmpVerts = new Vertex[graph.Vertices.Count];
            graph.Vertices.CopyTo(tmpVerts);
            HashSet<Vertex> remainingVertices = new HashSet<Vertex>(tmpVerts);
            remainingVertices.ExceptWith(processedVertices);

            Dictionary<Vertex, (Vertex predecessor, double sourceDistance)> result = new Dictionary<Vertex, (Vertex predecessor, double sourceDistance)>();
            
            while (!processedVertices.Contains(target))
            {
                Vertex minRemaining = getMinRemainingVertex(sourceDistances, heuristic, target, remainingVertices);
                processedVertices.Add(minRemaining);

                if (minRemaining != target)
                {
                    var remainingNeighbours = new HashSet<Vertex>(graph.AdjecencyList.GetNeighboursFor(minRemaining));
                    remainingNeighbours.ExceptWith(processedVertices);
                    sourceDistances.TryGetValue(minRemaining, out double dv);

                    foreach (Vertex neighbour in remainingNeighbours)
                    {
                        sourceDistances.TryGetValue(neighbour, out double dn);
                        
                        if (dv + 6.0 < dn)
                        {
                            sourceDistances.Remove(neighbour);
                            sourceDistances.Add(neighbour, dn + 6.0);

                            /* update results with own function: every time a d and p value gets updated, it has to be updated in result too => use only the result dict for the storing of the ds */
                        }
                    }
                }
            }

            Vertex getMinRemainingVertex(Dictionary<Vertex, double> dists, Func<Vertex, Vertex, double> heur, Vertex t, HashSet<Vertex> remainingVerts)
            {
                Vertex min = dists.First().Key;
                double dRef = double.PositiveInfinity;

                foreach (Vertex v in remainingVertices)
                {
                    if (dists.TryGetValue(v, out double dv))
                    {
                        if (dv + heur(v, t) < dRef) min = v;
                    }
                }

                return min;
            }

            Dictionary<Vertex, double> initSourceDistances(WeightedDigraph g, Vertex s)
            {
                var dists = new Dictionary<Vertex, double>();
                foreach (Vertex v in g.Vertices)
                {
                    if (v == s) dists.Add(s, 0);
                    else dists.Add(v, double.PositiveInfinity);
                }
                return dists;
            }
        }
    }
}
