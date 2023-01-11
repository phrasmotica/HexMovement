namespace HexGrid
{
    /// <summary>
    /// Represents a hex in a double-width coordinate system.
    /// </summary>
    public class DoubleWidthHex : IHex
    {
        public int Row { get; }

        public int Col { get; }

        public int Q => (Col - Row) / 2;

        public int R => Row;

        public int S => -Q - R;

        public Terrain Terrain { get; set; }

        public DoubleWidthHex(int row, int col)
        {
            if ((row + col) % 2 != 0)
            {
                throw new InvalidOperationException($"({col}, {row}) is not a valid double-width coordinate!");
            }

            Row = row;
            Col = col;
        }

        public (int, int) EastNeighbour() => (Row, Col + 2);

        public (int, int) SouthEastNeighbour() => (Row + 1, Col + 1);

        public (int, int) SouthWestNeighbour() => (Row + 1, Col - 1);

        public (int, int) WestNeighbour() => (Row, Col - 2);

        public (int, int) NorthWestNeighbour() => (Row - 1, Col - 1);

        public (int, int) NorthEastNeighbour() => (Row - 1, Col + 1);
    }
}
