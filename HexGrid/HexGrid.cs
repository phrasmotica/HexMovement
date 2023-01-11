namespace HexGrid
{
    /// <summary>
    /// Represents a hexagonal grid using a double-width coordinate system (see https://www.redblobgames.com/grids/hexagons/).
    /// </summary>
    public class HexGrid : IHexGrid
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

            GenerateMap();
        }

        private void GenerateMap()
        {
            var rand = new Random();

            for (var row = 0; row < Height; row++)
            {
                var hexes = new List<Hex>();

                // odd rows start at column 1
                var startCol = row % 2 != 0 ? 1 : 0;

                for (var col = startCol; col < Width; col += 2)
                {
                    // random terrain for each tile
                    var r = rand.Next(2) + 1;

                    hexes.Add(new(row, col)
                    {
                        Terrain = (Terrain) r,
                    });
                }

                Rows.Add(hexes);
            }
        }

        /// <summary>
        /// Returns the hex in the given row and column. Column needs to be halved because of
        /// double-width coordinate system.
        /// </summary>
        public Hex HexAt(int row, int col) => Rows[(row + Height) % Height][(col + Width) % Width / 2];

        public List<Hex> GetNeighbours(Hex hex)
        {
            var coords = new[]
            {
                // anticlockwise, starting from east neighbour
                (hex.Col + 2, hex.Row),
                (hex.Col + 1, hex.Row - 1),
                (hex.Col - 1, hex.Row - 1),
                (hex.Col - 2, hex.Row),
                (hex.Col - 1, hex.Row + 1),
                (hex.Col + 1, hex.Row + 1),
            };

            if (!WrapMovement)
            {
                // filter to only coordinates that are inside the grid, i.e. nothing out of range
                coords = coords.Where(c => c.Item2 >= 0 && c.Item2 < Height && c.Item1 >= 0 && (c.Item1 / 2) < Rows[c.Item2].Count)
                               .ToArray();
            };

            return coords.Select(c => HexAt(c.Item2, c.Item1)).ToList();
        }

        public Hex MoveEast(Hex hex) => HexAt(hex.Row, GetNextCol(hex, true));

        public Hex MoveSouthEast(Hex hex)
        {
            if (!WrapMovement && !CanMoveSouthEast(hex))
            {
                return hex;
            }

            return HexAt(GetNextRow(hex), GetNextCol(hex, false));
        }

        public Hex MoveSouthWest(Hex hex)
        {
            if (!WrapMovement && !CanMoveSouthWest(hex))
            {
                return hex;
            }

            return HexAt(GetNextRow(hex), GetPreviousCol(hex, false));
        }

        public Hex MoveWest(Hex hex) => HexAt(hex.Row, GetPreviousCol(hex, true));

        public Hex MoveNorthWest(Hex hex)
        {
            if (!WrapMovement && !CanMoveNorthWest(hex))
            {
                return hex;
            }

            return HexAt(GetPreviousRow(hex), GetPreviousCol(hex, false));
        }

        public Hex MoveNorthEast(Hex hex)
        {
            if (!WrapMovement && !CanMoveNorthEast(hex))
            {
                return hex;
            }

            return HexAt(GetPreviousRow(hex), GetNextCol(hex, false));
        }

        public bool CanMoveEast(Hex hex, bool doubleWidth) => hex.Col < Width - (doubleWidth ? 2 : 1);

        public bool CanMoveWest(Hex hex, bool doubleWidth) => hex.Col > (doubleWidth ? 1 : 0);

        public bool CanMoveSouthEast(Hex hex) => CanMoveDown(hex) && CanMoveEast(hex, false);

        public bool CanMoveSouthWest(Hex hex) => CanMoveDown(hex) && CanMoveWest(hex, false);

        public bool CanMoveNorthWest(Hex hex) => CanMoveUp(hex) && CanMoveWest(hex, false);

        public bool CanMoveNorthEast(Hex hex) => CanMoveUp(hex) && CanMoveEast(hex, false);

        private bool CanMoveDown(Hex hex) => hex.Row < Height - 1;

        private bool CanMoveUp(Hex hex) => hex.Row > 0;

        private int GetNextCol(Hex hex, bool doubleWidth)
        {
            var newCol = doubleWidth ? hex.Col + 2 : hex.Col + 1;

            if (CanMoveEast(hex, doubleWidth))
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

            if (CanMoveWest(hex, doubleWidth))
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
