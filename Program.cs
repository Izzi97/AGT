using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using static System.Environment;
using AGT.Model;

namespace AGT
{
    class Program
    {
        public static int vertexCounter = 1;
        public static List<Vertex> vertices = new List<Vertex>();

        static void Main(string[] args)
        {
            ////////////////////////////////////////////////////////////////////////////////
            /// GRAPH
            ////////////////////////////////////////////////////////////////////////////////
            ChessPosition startPosition = new ChessPosition(3, 4);
            var graph = GetChessGraph(KnightMovesAsNodes, startPosition);
            var adjacencyListCSV = graph.AdjacencyList.AdjacencyListAsCSV();

            string results = new StringBuilder("graph for knight moves as adjacency table:")
                .AppendLine(adjacencyListCSV)
                .ToString();
            Console.Write(results);
            ResultsToDesktop(adjacencyListCSV, "graph.csv");

            var source = graph.Vertices.First();
            var target = graph.Vertices.Where(v => v.Position.ToString() == "(A,1)").First();


            ////////////////////////////////////////////////////////////////////////////////
            /// BFS
            ////////////////////////////////////////////////////////////////////////////////
            var bfsResult = Algorithms.BFS(graph, source, target);
            var bfsResultsCSV = Algorithms.BFSResultAsCSV(bfsResult);

            var p = new Dictionary<Vertex, Vertex>(bfsResult.Select(kvp => new KeyValuePair<Vertex, Vertex>(kvp.Key, kvp.Value.predecessor)));
            var d = new Dictionary<Vertex, double>(bfsResult.Select(kvp => new KeyValuePair<Vertex, double>(kvp.Key, kvp.Value.sourceDistance)));
            var bfsDiscoverRatio = DiscoveryRatio(d);

            var bfsPathDistTuple = Algorithms.PathDistTupleFromSPResult((p, d), target);
            var bfsPathDistTupleString = Algorithms.FormatPathDistanceTuple(bfsPathDistTuple);

            Console.WriteLine($"BFS results table - source {source}:");
            Console.WriteLine(bfsResultsCSV);
            Console.WriteLine(bfsPathDistTupleString);
            Console.WriteLine($"discovery ratio: {bfsDiscoverRatio}");
            Console.WriteLine();
            ResultsToDesktop(bfsResultsCSV, "bfs.csv");


            ////////////////////////////////////////////////////////////////////////////////
            /// A*
            ////////////////////////////////////////////////////////////////////////////////
            var aStarRes = Algorithms.AStar(
                (WeightedDigraph)graph, 
                source, 
                target, 
                Vertex.GetScaledManhattenDist());
            var aStarResCSV = Algorithms.AStarResultTableAsCSV(aStarRes);
            var aStarResPath = Algorithms.FormatPathDistanceTuple(Algorithms.PathDistTupleFromSPResult(aStarRes, target));
            var aStarDiscoveryRatio = DiscoveryRatio(aStarRes.sourceDistances);
            Console.WriteLine($"A* from {source} to {target}");
            Console.WriteLine(aStarResCSV);
            Console.WriteLine(aStarResPath);
            Console.WriteLine($"discovery ratio: {aStarDiscoveryRatio}");
            Console.WriteLine();
            ResultsToDesktop(aStarResCSV, "aStar.csv");


            ////////////////////////////////////////////////////////////////////////////////
            /// IDA*
            ////////////////////////////////////////////////////////////////////////////////
            var iDAStarRes = Algorithms.IDAStar(
                (WeightedDigraph) graph, 
                source, 
                target, 
                Vertex.GetScaledManhattenDist());
            var iDAStarResPath = Algorithms.FormatPathDistanceTuple((iDAStarRes.path, iDAStarRes.distance));
            var iDAStarDiscoveryRatio = DiscoveryRatio(iDAStarRes.sourceDistances);
            Console.WriteLine($"IDA* from {source} to {target}");
            Console.WriteLine(iDAStarResPath);
            Console.WriteLine($"discovery ratio: {iDAStarDiscoveryRatio}");
        }

        private static Graph GetChessGraph(Func<Vertex, IEnumerable<Vertex>> DiscoverPossibleMovesAsNodes, ChessPosition chessPieceStartPosition)
        {
            AdjacencyList adjacencyList = new AdjacencyList();
            HashSet<Vertex> vertices = new HashSet<Vertex>();

            Queue<Vertex> verticeQueue = new Queue<Vertex>();
            Vertex startVertice = NewVertice(chessPieceStartPosition);
            vertices.Add(startVertice);

            verticeQueue.Enqueue(startVertice);

            while(verticeQueue.Count != 0)
            {
                Vertex sourceVertice = verticeQueue.Dequeue();
                vertices.Add(sourceVertice);

                if (!adjacencyList.HasEntryFor(sourceVertice)) {
                    IEnumerable<Vertex> targetNodes = DiscoverPossibleMovesAsNodes(sourceVertice);
                    adjacencyList.AddEntry(sourceVertice, targetNodes);
                    foreach (var node in targetNodes) verticeQueue.Enqueue(node);
                }
            }

            return new WeightedDigraph(vertices, adjacencyList, null);
        }

        private static Vertex NewVertice(ChessPosition position)
        {
            string name = (vertexCounter++).ToString();
            Vertex newNode = new Vertex(name, position);
            vertices.Add(newNode);
            return newNode;
        }

        private static IEnumerable<Vertex> KnightMovesAsNodes(Vertex startingNode)
        {
            List<Vertex> movesAsNodes = new List<Vertex>();
            ChessPosition pos = startingNode.Position;

            ChessPosition testPos = new ChessPosition(pos.X + 1, pos.Y + 2);
            if (InRange(testPos))
            {
                (bool exists, Vertex node) = NodeForPosExists(testPos);
                if(exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X + 2, pos.Y + 1);
            if (InRange(testPos))
            {
                (bool exists, Vertex node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X + 2, pos.Y - 1);
            if (InRange(testPos))
            {
                (bool exists, Vertex node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X + 1, pos.Y - 2);
            if (InRange(testPos))
            {
                (bool exists, Vertex node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X - 1, pos.Y - 2);
            if (InRange(testPos))
            {
                (bool exists, Vertex node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X - 2, pos.Y - 1);
            if (InRange(testPos))
            {
                (bool exists, Vertex node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X - 2, pos.Y + 1);
            if (InRange(testPos))
            {
                (bool exists, Vertex node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X - 1, pos.Y + 2);
            if (InRange(testPos))
            {
                (bool exists, Vertex node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            // lexicographic order
            movesAsNodes.Sort(startingNode.Compare);
            return movesAsNodes;

            bool InRange(ChessPosition position)
            {
                return position.X > 0 && position.X <= 8 && position.Y > 0 && position.Y <= 8;
            }

            (bool exists, Vertex existingNode) NodeForPosExists(ChessPosition positionToTest)
            {
                Vertex dummy = new Vertex("phyack", positionToTest);
                var candidates = vertices.Where(node => dummy.Equals(node));
                if (candidates.Any())
                {
                    return (true, candidates.First());
                }
                else
                {
                    return (false, null);
                }
            }
        }

        private static double DiscoveryRatio(Dictionary<Vertex, double> sourceDistances)
        {
            int discoveries = sourceDistances.Where(kvp => kvp.Value != double.PositiveInfinity).Count();
            int total = sourceDistances.Count();
            return ((double)discoveries) / total;
        }

        private static void ResultsToDesktop(string results, string filename)
        {
            try
            {
                var desktopPath = GetFolderPath(SpecialFolder.Desktop);
                var outputFolderPath = Path.Combine(desktopPath, "AGT_Hausuebung");
                Directory.CreateDirectory(outputFolderPath);
                var outputFilePath = Path.Combine(outputFolderPath, filename);

                File.WriteAllText(outputFilePath, results);
            }
            catch (Exception e)
            {
                Console.WriteLine("u fucked up: " + e);
            }
        }
    }
}
