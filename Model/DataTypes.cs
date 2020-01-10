using System.Collections.Generic;
using System.Linq;
using System;

namespace AGT
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

    public class Vertice
    {
        public string Name { get; }
        public ChessPosition Position { get; }
        public Vertice(string name, ChessPosition position)
        {
            Name = name;
            Position = position;
        }

        public bool EqualsNode(Vertice other)
        {
            return Position.X == other.Position.X && Position.Y == other.Position.Y;
        }

        public static int CompareNodes(Vertice first, Vertice second)
        {
            int result = 0;

            if (first.Position.X < second.Position.X) result = -1;

            else if (first.Position.X == second.Position.X)
            {
                if (first.Position.Y < second.Position.Y) result = - 1;
                else if (first.Position.Y == second.Position.Y) result = 0;
                else if (first.Position.Y > second.Position.Y) result = 1;
            }

            else result = 1;

            return result;
        }

        public override string ToString()
        {
            return $"Node{Name.ToString()}\t@ {Position.ToString()}";
        }
    }

    public class AdjecencyList
    {
        private readonly Dictionary<Vertice, IEnumerable<Vertice>> adjacencyList;

        public AdjecencyList()
        {
            adjacencyList = new Dictionary<Vertice, IEnumerable<Vertice>>();
        }

        public void AddEntry(Vertice from, IEnumerable<Vertice> to)
        {
            adjacencyList.Add(from, to);
        }

        public IEnumerable<Vertice> GetNeighboursFor(Vertice v)
        {
            if (!adjacencyList.TryGetValue(v, out IEnumerable<Vertice> neighbours)) throw new ArgumentException("adjacency list does not contain an entry for vertice " + v.ToString());
            return neighbours;
        }

        public bool HasEntryFor(Vertice from)
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

    public class Graph
    {
        public HashSet<Vertice> Vertices { get; }
        public AdjecencyList AdjecencyList { get; }
        public Graph(HashSet<Vertice> vertices, AdjecencyList adjecencyList)
        {
            Vertices = vertices;
            AdjecencyList = adjecencyList;
        }
    }
}