using System.Collections.Generic;
using System.Text;
using AGT.Model;

namespace AGT
{
    public static partial class Algorithms
    {
        public static (IEnumerable<Vertex> path, double distance) PathDistTupleFromSPResult(
            Dictionary<Vertex, Vertex> predecessors,
            Dictionary<Vertex, double> sourceDistances, 
            Vertex target)
        {
            List<Vertex> path = new List<Vertex>();
            Vertex cursor = target;

            while (!Equals(cursor, null))
            {
                path.Add(cursor);
                cursor = predecessors[cursor];
            }

            path.Reverse();

            return (path, sourceDistances[target]);
        }

        public static string FormatPathDistanceTuple((IEnumerable<Vertex> path, double distance) pathDistanceTuple)
        {
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append("shortest path: ");

            foreach (Vertex v in pathDistanceTuple.path) outputBuilder.Append($"{v.ToString()} -> ");
            outputBuilder.Remove(outputBuilder.Length - 4, 4);
            outputBuilder.AppendLine();

            outputBuilder.Append($"length: {pathDistanceTuple.distance}");

            return outputBuilder.ToString();
        }

        public class AlgorithmProtocolStep
        {
            public Vertex ProcessedVertex { get; }
            public Dictionary<Vertex, (Vertex predecessor, double distance)> Table { get; }

            public AlgorithmProtocolStep(Vertex processedVertex, Dictionary<Vertex, (Vertex, double)> initialTable)
            {
                ProcessedVertex = processedVertex;
                Table = initialTable;
            }

            public void AddOrUpdate(Vertex v, Vertex p, double d)
            {
                Table[v] = (p, d);
            }
        }

        public class AlgorithmProtocol
        {
            private readonly List<AlgorithmProtocolStep> _steps;
            public IEnumerable<AlgorithmProtocolStep> Steps => _steps;
            public AlgorithmProtocol()
            {
                _steps = new List<AlgorithmProtocolStep>();
            }

            public void AddStep(AlgorithmProtocolStep step)
            {
                _steps.Add(step);
            }
        }

        public class AlgorithmResult
        {
            public AlgorithmProtocol Protocol { get; }
            public int Q { get; }
            public int S { get; }

            public AlgorithmResult(AlgorithmProtocol protocol, int q, int s)
            {
                Protocol = protocol;
                Q = q;
                S = s;
            }
        }

        public static string AlgorithmProtocolToExcel(AlgorithmResult result)
        {
            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.AppendLine("v;p;d");

            foreach (AlgorithmProtocolStep step in result.Protocol.Steps)
            {
                resultBuilder.AppendLine($"processed {step.ProcessedVertex}");
                foreach (var entry in step.Table)
                {
                    string predecessor = !Equals(entry.Value.predecessor, null) ? entry.Value.predecessor.ToString() : "nil";
                    resultBuilder.AppendLine($"{entry.Key};{predecessor};{entry.Value.distance}");
                }
                resultBuilder.AppendLine();
            }

            resultBuilder.AppendLine($"Q: {result.Q};S: {result.S}");

            return resultBuilder.ToString();
        }

    }
}
