using System.Collections.Generic;
using System.Text;
using AGT.Model;

namespace AGT
{
    public static partial class Algorithms
    {
        public static (IEnumerable<Vertex> path, double distance) PathDistTupleFromSPResult(
            (Dictionary<Vertex, Vertex> predecessors, Dictionary<Vertex, double> sourceDistances) spResult, 
            Vertex target)
        {
            List<Vertex> path = new List<Vertex>();
            Vertex cursor = target;

            while (!Equals(cursor, null))
            {
                path.Add(cursor);
                cursor = spResult.predecessors[cursor];
            }

            path.Reverse();

            return (path, spResult.sourceDistances[target]);
        }

        public static string FormatPathDistanceTuple((IEnumerable<Vertex> path, double distance) pathDistanceTuple)
        {
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append("shortest path: ");

            foreach (Vertex v in pathDistanceTuple.path) outputBuilder.Append($"{v.ToString()} -> ");
            outputBuilder.Remove(outputBuilder.Length - 4, 4);
            outputBuilder.AppendLine();

            outputBuilder.AppendLine($"length: {pathDistanceTuple.distance}");

            return outputBuilder.ToString();
        }
    }
}