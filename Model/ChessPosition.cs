namespace AGT.Model
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
}
