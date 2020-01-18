using System;
using System.Collections.Generic;

namespace AGT.Model
{
    public class Vertex : IEquatable<Vertex>, IComparer<Vertex>
    {
        public string Name { get; }
        public ChessPosition Position { get; }
        public Vertex(string name, ChessPosition position)
        {
            Name = name;
            Position = position;
        }

        public static Func<Vertex, Vertex, double> GetScaledManhattenDist(double scale = 1.0)
        {
            return (Vertex from, Vertex to) =>
            {
                double dx = Math.Abs(to.Position.X - from.Position.X);
                double dy = Math.Abs(to.Position.Y - from.Position.Y);
                return scale * (dx + dy);
            };
        }

        public static Func<Vertex, Vertex, double> GetScaledEuclideanMax(double scale = 1.0)
        {
            return (Vertex from, Vertex to) =>
            {
                double dx = Math.Abs(to.Position.X - from.Position.X);
                double dy = Math.Abs(to.Position.Y - from.Position.Y);
                return scale * Math.Max(dx, dy);
            };
        }

        public override string ToString()
        {
            return $"v{Name.ToString()}@{Position.ToString()}";
        }

        public bool Equals(Vertex other)
        {
            return Position.X == other.Position.X && Position.Y == other.Position.Y;
        }

        public int Compare(Vertex x, Vertex y)
        {
            int result = 0;

            if (x.Position.X < y.Position.X) result = -1;

            else if (x.Position.X == y.Position.X)
            {
                if (x.Position.Y < y.Position.Y) result = -1;
                else if (x.Position.Y == y.Position.Y) result = 0;
                else if (x.Position.Y > y.Position.Y) result = 1;
            }

            else result = 1;

            return result;
        }

        public static bool operator ==(Vertex x, Vertex y) => x.Equals(y);
        public static bool operator !=(Vertex x, Vertex y) => !x.Equals(y);
        public static bool operator <(Vertex x, Vertex y) => x.Compare(x, y) == -1 ? true : false;
        public static bool operator >(Vertex x, Vertex y) => x.Compare(x, y) == 1 ? true : false;
        public static bool operator <=(Vertex x, Vertex y) => x < y || x == y;
        public static bool operator >=(Vertex x, Vertex y) => x > y || x == y;
    }
}
