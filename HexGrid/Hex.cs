namespace HexGrid
{
    public class Hex
    {
        public int Row { get; }

        public int Col { get; }

        /// <summary>
        /// Gets the axial Q-coordinate of the hex.
        /// </summary>
        public int Q => (Col - Row) / 2;

        /// <summary>
        /// Gets the axial R-coordinate of the hex.
        /// </summary>
        public int R => Row;

        /// <summary>
        /// Gets the axial S-coordinate of the hex.
        /// </summary>
        public int S => -Q - R;

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
