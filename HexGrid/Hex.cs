namespace HexGrid
{
    public class Hex
    {
        public int Row { get; }

        public int Col { get; }

        public Terrain Terrain { get; set; }

        public Hex(int row, int col)
        {
            if ((row + col) % 2 != 0)
            {
                throw new InvalidOperationException($"({col}, {row}) is not a valid double-width coordinate!");
            }

            Row = row;
            Col = col;
        }
    }

    public enum Terrain
    {
        Plains,
        Hill,
    }
}
