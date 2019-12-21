using System.Collections.Generic;
using System.Linq;

namespace ChessDirectedGraphSimulation
{
    public class ChessPosition
    {
        public int X { get; }
        public int Y { get; }
        public string ChessX
        {
            get
            {
                return 
                (X == 1) ? "A" :
                (X == 2) ? "B" :
                (X == 3) ? "C" :
                (X == 4) ? "D" :
                (X == 5) ? "E" :
                (X == 6) ? "F" :
                (X == 7) ? "G" :
                (X == 8) ? "H" :
                "fuck";
            }
        }
        public ChessPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({ChessX},{Y})";
        }
    }

    public class Node
    {
        public string Name { get; }
        public ChessPosition Position { get; }
        public Node(string name, ChessPosition position)
        {
            Name = name;
            Position = position;
        }

        public bool EqualsNode(Node other)
        {
            return Position.X == other.Position.X && Position.Y == other.Position.Y;
        }

        public override string ToString()
        {
            return $"Node{Name.ToString()}\t@ {Position.ToString()}";
        }
    }

    public class AdjecencyList
    {
        private readonly Dictionary<Node, IEnumerable<Node>> adjacencyList;

        public AdjecencyList()
        {
            adjacencyList = new Dictionary<Node, IEnumerable<Node>>();
        }

        public void AddEntry(Node from, IEnumerable<Node> to)
        {
            adjacencyList.Add(from, to);
        }

        public bool HasEntryFor(Node from)
        {
            return adjacencyList.Keys.Where(node => from.EqualsNode(node)).Any();
        }

        public override string ToString()
        {
            string output = string.Empty;

            foreach (var entry in adjacencyList)
            {
                output += $"{entry.Key.ToString()}\t->\t";
                foreach (var target in entry.Value)
                {
                    output += $"{target.ToString()}, ";
                }
                output = output.Remove(output.Length - 2);
                output += System.Environment.NewLine;
            }

            return output;
        }
    }
}