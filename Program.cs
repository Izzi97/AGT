using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessDirectedGraphSimulation
{
    class Program
    {
        public static int nodeCounter = 1;
        public static List<Node> nodes = new List<Node>();

        static void Main(string[] args)
        {
            ChessPosition startPosition = new ChessPosition(3, 4);
            var adjacencyList = GetChessGraphAsAdjacencyList(KnightMovesAsNodes, startPosition);

            Console.WriteLine(adjacencyList.ToString());
        }

        public static AdjecencyList GetChessGraphAsAdjacencyList(Func<Node, IEnumerable<Node>> DiscoverPossibleMovesAsNodes, ChessPosition chessPieceStartPosition)
        {
            AdjecencyList adjacencyList = new AdjecencyList();

            Queue<Node> nodesToProcess = new Queue<Node>();
            Node startNode = NewNode(chessPieceStartPosition);

            nodesToProcess.Enqueue(startNode);

            while(nodesToProcess.Count != 0)
            {
                Node sourceNode = nodesToProcess.Dequeue();

                if (!adjacencyList.HasEntryFor(sourceNode)) {
                    IEnumerable<Node> targetNodes = DiscoverPossibleMovesAsNodes(sourceNode);
                    adjacencyList.AddEntry(sourceNode, targetNodes);
                    foreach (var node in targetNodes) nodesToProcess.Enqueue(node);
                }
            }

            return adjacencyList;
        }

        public static Node NewNode(ChessPosition position)
        {
            string name = (nodeCounter++).ToString();
            Node newNode = new Node(name, position);
            nodes.Add(newNode);
            return newNode;
        }

        public static IEnumerable<Node> KnightMovesAsNodes(Node startingNode)
        {
            List<Node> movesAsNodes = new List<Node>();
            ChessPosition pos = startingNode.Position;

            ChessPosition testPos = new ChessPosition(pos.X + 1, pos.Y + 2);
            if (InRange(testPos))
            {
                (bool exists, Node node) = NodeForPosExists(testPos);
                if(exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewNode(testPos));
            }

            testPos = new ChessPosition(pos.X + 2, pos.Y + 1);
            if (InRange(testPos))
            {
                (bool exists, Node node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewNode(testPos));
            }

            testPos = new ChessPosition(pos.X + 2, pos.Y - 1);
            if (InRange(testPos))
            {
                (bool exists, Node node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewNode(testPos));
            }

            testPos = new ChessPosition(pos.X + 1, pos.Y - 2);
            if (InRange(testPos))
            {
                (bool exists, Node node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewNode(testPos));
            }

            testPos = new ChessPosition(pos.X - 1, pos.Y - 2);
            if (InRange(testPos))
            {
                (bool exists, Node node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewNode(testPos));
            }

            testPos = new ChessPosition(pos.X - 2, pos.Y - 1);
            if (InRange(testPos))
            {
                (bool exists, Node node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewNode(testPos));
            }

            testPos = new ChessPosition(pos.X - 2, pos.Y + 1);
            if (InRange(testPos))
            {
                (bool exists, Node node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewNode(testPos));
            }

            testPos = new ChessPosition(pos.X - 1, pos.Y + 2);
            if (InRange(testPos))
            {
                (bool exists, Node node) = NodeForPosExists(testPos);
                if (exists) movesAsNodes.Add(node);
                else movesAsNodes.Add(NewNode(testPos));
            }

            return movesAsNodes;

            bool InRange(ChessPosition position)
            {
                return position.X > 0 && position.X <= 8 && position.Y > 0 && position.Y <= 8;
            }

            (bool exists, Node existingNode) NodeForPosExists(ChessPosition positionToTest)
            {
                Node dummy = new Node("phyack", positionToTest);
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
