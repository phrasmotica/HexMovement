namespace HexGrid
{
    /// <summary>
    /// Represents a hexagonal grid using a double-width coordinate system (see https://www.redblobgames.com/grids/hexagons/).
    /// </summary>
    public class HexGrid
    {
        public int Width { get; }

        public int Height { get; }

        public List<List<Hex>> Rows { get; }

        public bool WrapMovement { get; set; }

        public HexGrid(int width, int height)
        {
            Width = width;
            Height = height;

            Rows = new();

            for (var row = 0; row < height; row++)
            {
                var hexes = new List<Hex>();

                // odd rows start at column 1
                var startCol = row % 2 != 0 ? 1 : 0;

                for (var col = startCol; col < width; col += 2)
                {
                    hexes.Add(new(row, col));
                }

                Rows.Add(hexes);
            }
        }

        public List<Hex> GetNeighbours(Hex hex)
        {
            // TODO: support wrapping

            var coords = new[]
            {
                (hex.Col - 1, hex.Row - 1), (hex.Col + 1, hex.Row - 1), // row above
                (hex.Col - 2, hex.Row), (hex.Col + 2, hex.Row), // this row
                (hex.Col - 1, hex.Row + 1), (hex.Col + 1, hex.Row + 1), // row below
            }
            // filter to only those hexes that are in the grid
            .Where(c => c.Item2 >= 0 && c.Item2 < Height && c.Item1 >= 0 && (c.Item1 / 2) < Rows[c.Item2].Count)
            .ToList();

            return coords.Select(c => HexAt(c.Item2, c.Item1)).ToList();
        }

        public Hex MoveRight(Hex hex) => HexAt(hex.Row, GetNextCol(hex, true));

        public Hex MoveDownRight(Hex hex)
        {
            if (!WrapMovement && !CanMoveDownRight(hex))
            {
                return hex;
            }

            return HexAt(GetNextRow(hex), GetNextCol(hex, false));
        }

        public Hex MoveDownLeft(Hex hex)
        {
            if (!WrapMovement && !CanMoveDownLeft(hex))
            {
                return hex;
            }

            return HexAt(GetNextRow(hex), GetPreviousCol(hex, false));
        }

        public Hex MoveLeft(Hex hex) => HexAt(hex.Row, GetPreviousCol(hex, true));

        public Hex MoveUpLeft(Hex hex)
        {
            if (!WrapMovement && !CanMoveUpLeft(hex))
            {
                return hex;
            }

            return HexAt(GetPreviousRow(hex), GetPreviousCol(hex, false));
        }

        public Hex MoveUpRight(Hex hex)
        {
            if (!WrapMovement && !CanMoveUpRight(hex))
            {
                return hex;
            }

            return HexAt(GetPreviousRow(hex), GetNextCol(hex, false));
        }

        public bool CanMoveRight(Hex hex, bool doubleWidth) => hex.Col < Width - (doubleWidth ? 2 : 1);

        public bool CanMoveLeft(Hex hex, bool doubleWidth) => hex.Col > (doubleWidth ? 1 : 0);

        public bool CanMoveDownRight(Hex hex) => CanMoveDown(hex) && CanMoveRight(hex, false);

        public bool CanMoveDownLeft(Hex hex) => CanMoveDown(hex) && CanMoveLeft(hex, false);

        public bool CanMoveUpLeft(Hex hex) => CanMoveUp(hex) && CanMoveLeft(hex, false);

        public bool CanMoveUpRight(Hex hex) => CanMoveUp(hex) && CanMoveRight(hex, false);

        /// <summary>
        /// Returns the hex in the given row and column. Column needs to be halved because of
        /// double-width coordinate system.
        /// </summary>
        public Hex HexAt(int row, int col) => Rows[row][col / 2];

        private bool CanMoveDown(Hex hex) => hex.Row < Height - 1;

        private bool CanMoveUp(Hex hex) => hex.Row > 0;

        private int GetNextCol(Hex hex, bool doubleWidth)
        {
            var newCol = doubleWidth ? hex.Col + 2 : hex.Col + 1;

            if (CanMoveRight(hex, doubleWidth))
            {
                return newCol;
            }

            return WrapMovement ? newCol % Width : hex.Col;
        }

        private int GetNextRow(Hex hex)
        {
            var newRow = hex.Row + 1;

            if (CanMoveDown(hex))
            {
                return newRow;
            }

            return WrapMovement ? newRow % Height : hex.Row;
        }

        private int GetPreviousCol(Hex hex, bool doubleWidth)
        {
            var newCol = doubleWidth ? hex.Col - 2 : hex.Col - 1;

            if (CanMoveLeft(hex, doubleWidth))
            {
                return newCol;
            }

            // cannot use modulo on negative ints
            return WrapMovement ? newCol + Width : hex.Col;
        }

        private int GetPreviousRow(Hex hex)
        {
            var newRow = hex.Row - 1;

            if (CanMoveUp(hex))
            {
                return newRow;
            }

            // cannot use modulo on negative ints
            return WrapMovement ? newRow + Height : hex.Row;
        }
    }
}
