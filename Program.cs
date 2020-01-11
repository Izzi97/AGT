using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using static System.Environment;

namespace AGT
{
    class Program
    {
        public static int nodeCounter = 1;
        public static List<Vertex> nodes = new List<Vertex>();

        static void Main(string[] args)
        {
            ChessPosition startPosition = new ChessPosition(3, 4);
            var graph = GetChessGraph(KnightMovesAsNodes, startPosition);
            var adjacencyListString = graph.AdjecencyList.ToString();

            Console.WriteLine(adjacencyListString);

            var bfsResult = Algorithms.BFS(graph, graph.Vertices.First());

            Console.WriteLine("BFS finished ...");
            Console.WriteLine("... with results table:");
            Console.WriteLine(Algorithms.BFSResultAsCSV(bfsResult));

            //try
            //{
            //    var desktopPath = GetFolderPath(SpecialFolder.Desktop);
            //    var outputFolderPath = Path.Combine(desktopPath, "ChessDirectedGraphSimulation");
            //    Directory.CreateDirectory(outputFolderPath);
            //    var outputFilePath = Path.Combine(outputFolderPath, "adjacency_list.txt");

            //    File.WriteAllText(outputFilePath, adjacencyListString);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("u fucked up: " + e);
            //}
        }

        public static Graph GetChessGraph(Func<Vertex, IEnumerable<Vertex>> DiscoverPossibleMovesAsNodes, ChessPosition chessPieceStartPosition)
        {
            AdjecencyList adjacencyList = new AdjecencyList();
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

            return new Graph(vertices, adjacencyList);
        }

        public static Vertex NewVertice(ChessPosition position)
        {
            string name = (nodeCounter++).ToString();
            Vertex newNode = new Vertex(name, position);
            nodes.Add(newNode);
            return newNode;
        }

        public static IEnumerable<Vertex> KnightMovesAsNodes(Vertex startingNode)
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
            movesAsNodes.Sort(Vertex.CompareNodes);
            return movesAsNodes;

            bool InRange(ChessPosition position)
            {
                return position.X > 0 && position.X <= 8 && position.Y > 0 && position.Y <= 8;
            }

            (bool exists, Vertex existingNode) NodeForPosExists(ChessPosition positionToTest)
            {
                Vertex dummy = new Vertex("phyack", positionToTest);
                var candidates = nodes.Where(node => dummy.EqualsNode(node));
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
    }
}
