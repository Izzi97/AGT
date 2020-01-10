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
        public static List<Vertice> nodes = new List<Vertice>();

        static void Main(string[] args)
        {
            ChessPosition startPosition = new ChessPosition(3, 4);
            var graph = GetChessGraph(KnightMovesAsNodes, startPosition);
            var adjacencyListString = graph.AdjecencyList.ToString();

            Console.WriteLine(adjacencyListString);

            var bfsResult = Algorithms.BFS(graph, graph.Vertices.First());

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

        public static Graph GetChessGraph(Func<Vertice, IEnumerable<Vertice>> DiscoverPossibleMovesAsNodes, ChessPosition chessPieceStartPosition)
        {
            AdjecencyList adjacencyList = new AdjecencyList();
            HashSet<Vertice> vertices = new HashSet<Vertice>();

            Queue<Vertice> verticeQueue = new Queue<Vertice>();
            Vertice startVertice = NewVertice(chessPieceStartPosition);
            vertices.Add(startVertice);

            verticeQueue.Enqueue(startVertice);

            while(verticeQueue.Count != 0)
            {
                Vertice sourceVertice = verticeQueue.Dequeue();
                vertices.Add(sourceVertice);

                if (!adjacencyList.HasEntryFor(sourceVertice)) {
                    IEnumerable<Vertice> targetNodes = DiscoverPossibleMovesAsNodes(sourceVertice);
                    adjacencyList.AddEntry(sourceVertice, targetNodes);
                    foreach (var node in targetNodes) verticeQueue.Enqueue(node);
                }
            }

            return new Graph(vertices, adjacencyList);
        }

        public static Vertice NewVertice(ChessPosition position)
        {
            string name = (nodeCounter++).ToString();
            Vertice newNode = new Vertice(name, position);
            nodes.Add(newNode);
            return newNode;
        }

        public static IEnumerable<Vertice> KnightMovesAsNodes(Vertice startingNode)
        {
            List<Vertice> movesAsNodes = new List<Vertice>();
            ChessPosition pos = startingNode.Position;

            ChessPosition testPos = new ChessPosition(pos.X + 1, pos.Y + 2);
            if (InRange(testPos))
            {
                (bool exists, Vertice node) = NodeForPosExists(testPos);
                if(exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X + 2, pos.Y + 1);
            if (InRange(testPos))
            {
                (bool exists, Vertice node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X + 2, pos.Y - 1);
            if (InRange(testPos))
            {
                (bool exists, Vertice node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X + 1, pos.Y - 2);
            if (InRange(testPos))
            {
                (bool exists, Vertice node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X - 1, pos.Y - 2);
            if (InRange(testPos))
            {
                (bool exists, Vertice node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X - 2, pos.Y - 1);
            if (InRange(testPos))
            {
                (bool exists, Vertice node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X - 2, pos.Y + 1);
            if (InRange(testPos))
            {
                (bool exists, Vertice node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            testPos = new ChessPosition(pos.X - 1, pos.Y + 2);
            if (InRange(testPos))
            {
                (bool exists, Vertice node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewVertice(testPos));
            }

            // lexicographic order
            movesAsNodes.Sort(Vertice.CompareNodes);
            return movesAsNodes;

            bool InRange(ChessPosition position)
            {
                return position.X > 0 && position.X <= 8 && position.Y > 0 && position.Y <= 8;
            }

            (bool exists, Vertice existingNode) NodeForPosExists(ChessPosition positionToTest)
            {
                Vertice dummy = new Vertice("phyack", positionToTest);
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
