namespace HexGrid
{
    /// <summary>
    /// Represents a hexagonal grid using a double-width coordinate system (see https://www.redblobgames.com/grids/hexagons/).
    /// </summary>
    public class DoubleWidthHexGrid : IHexGrid
    {
        private readonly IPathCalculator _pathCalculator;

        public int Width { get; }

        public int Height { get; }

        public List<List<IHex>> Rows { get; }

        public bool WrapMovement { get; set; }

        public DoubleWidthHexGrid(int width, int height, IPathCalculator? pathCalculator = null)
        {
            Width = width;
            Height = height;

            Rows = new();

            _pathCalculator = pathCalculator ?? new AStarPathCalculator();

            GenerateMap();
        }

        private void GenerateMap()
        {
            var rand = new Random();

            for (var row = 0; row < Height; row++)
            {
                var hexes = new List<IHex>();

                // odd rows start at column 1
                var startCol = row % 2 != 0 ? 1 : 0;

                for (var col = startCol; col < Width; col += 2)
                {
                    // random terrain for each tile
                    var r = rand.Next(2) + 1;

                    hexes.Add(new DoubleWidthHex(row, col)
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
        public IHex HexAt(int row, int col) => Rows[(row + Height) % Height][(col + Width) % Width / 2];

        public List<IHex> GetNeighbours(IHex hex)
        {
            var coords = new[]
            {
                hex.EastNeighbour(),
                hex.SouthEastNeighbour(),
                hex.SouthWestNeighbour(),
                hex.WestNeighbour(),
                hex.NorthWestNeighbour(),
                hex.NorthEastNeighbour(),
            };

            if (!WrapMovement)
            {
                // filter to only coordinates that are inside the grid, i.e. nothing out of range
                coords = coords.Where(c => IsInGrid(c.Item1, c.Item2)).ToArray();
            };

            return coords.Select(c => HexAt(c.Item1, c.Item2)).ToList();
        }

        /// <summary>
        /// Returns the distance between two hexes in terms of the number of hexes. Will account for
        /// the grid having wrapping enabled.
        ///
        /// Taken from https://www.redblobgames.com/grids/hexagons/#distances-doubled
        /// </summary>
        public int ComputeWrappedDistance(IHex start, IHex end)
        {
            var distance = ComputeDistance(start, end);

            if (WrapMovement)
            {
                // We'll bring the candidate (rightmost) hex "around" the edges of the grid,
                // and find the shortest distance to each of these wrapped positions.
                // Not sure why we don't need to ensure the candidate is also the bottom-most
                // hex, but this seems to be working!
                var (candidateHex, otherHex) = start.Col > end.Col ? (start, end) : (end, start);

                var candidateDistances = new[]
                {
                    distance,

                    // wrapped around the west edge
                    ComputeDistance(otherHex, new DoubleWidthHex(candidateHex.Row, candidateHex.Col - Width)),

                    // wrapped around the north edge
                    ComputeDistance(otherHex, new DoubleWidthHex(candidateHex.Row - Height, candidateHex.Col)),

                    // wrapped around both west and north edges
                    ComputeDistance(otherHex, new DoubleWidthHex(candidateHex.Row - Height, candidateHex.Col - Width)),
                };

                return candidateDistances.Min();
            }

            return distance;
        }

        private static int ComputeDistance(IHex start, IHex end)
        {
            var distX = Math.Abs(start.Col - end.Col);
            var distY = Math.Abs(start.Row - end.Row);

            return distY + Math.Max(0, (distX - distY) / 2);
        }

        /// <summary>
        /// Returns the shortest path between two hexes. Will account for the grid having wrapping enabled.
        /// </summary>
        public Path ComputeWrappedPath(IHex start, IHex end)
        {
            var path = _pathCalculator.Compute(this, start, end);

            if (WrapMovement)
            {
                // We'll bring the candidate (rightmost) hex "around" the edges of the grid,
                // and find the shortest path to each of these wrapped positions.
                // Not sure why we don't need to ensure the candidate is also the bottom-most
                // hex, but this seems to be working!
                var (candidateHex, otherHex) = start.Col > end.Col ? (start, end) : (end, start);

                var candidatePaths = new[]
                {
                    path,

                    // wrapped around the west edge
                    _pathCalculator.Compute(this, otherHex, HexAt(candidateHex.Row, candidateHex.Col - Width)),

                    // wrapped around the north edge
                    _pathCalculator.Compute(this, otherHex, HexAt(candidateHex.Row - Height, candidateHex.Col)),

                    // wrapped around both west and north edges
                    _pathCalculator.Compute(this, otherHex, HexAt(candidateHex.Row - Height, candidateHex.Col - Width)),
                };

                return candidatePaths.MinBy(p => p.Length)!;
            }

            return path;
        }

        private bool IsInGrid(int row, int col)
        {
            var isWellDefined = (row + col) % 2 == 0;
            return isWellDefined && row >= 0 && row < Height && col >= 0 && (col / 2) < Rows[row].Count;
        }

        public IHex MoveEast(IHex hex)
        {
            if (CanMoveEast(hex))
            {
                return HexAt(hex.Row, hex.Col + 2);
            }

            return WrapMovement ? HexAt(hex.Row, (hex.Col + 2) % Width) : hex;
        }

        public IHex MoveSouthEast(IHex hex)
        {
            if (CanMoveSouthEast(hex))
            {
                return HexAt(hex.Row + 1, hex.Col + 1);
            }

            return WrapMovement ? HexAt((hex.Row + 1) % Height, (hex.Col + 1) % Width) : hex;
        }

        public IHex MoveSouthWest(IHex hex)
        {
            if (CanMoveSouthWest(hex))
            {
                return HexAt(hex.Row + 1, hex.Col - 1);
            }

            return WrapMovement ? HexAt((hex.Row + 1) % Height, hex.Col - 1 + Width) : hex;
        }

        public IHex MoveWest(IHex hex)
        {
            if (CanMoveWest(hex))
            {
                return HexAt(hex.Row, hex.Col - 2);
            }

            return WrapMovement ? HexAt(hex.Row, hex.Col - 2 + Width) : hex;
        }

        public IHex MoveNorthWest(IHex hex)
        {
            if (CanMoveNorthWest(hex))
            {
                return HexAt(hex.Row - 1, hex.Col - 1);
            }

            return WrapMovement ? HexAt(hex.Row - 1 + Height, hex.Col - 1 + Width) : hex;
        }

        public IHex MoveNorthEast(IHex hex)
        {
            if (CanMoveNorthEast(hex))
            {
                return HexAt(hex.Row - 1, hex.Col + 1);
            }

            return WrapMovement ? HexAt(hex.Row - 1 + Height, (hex.Col + 1) % Width) : hex;
        }

        public bool CanMoveEast(IHex hex) => hex.Col < Width - 2;

        public bool CanMoveWest(IHex hex) => hex.Col > 1;

        public bool CanMoveSouthEast(IHex hex) => CanMoveDown(hex) && hex.Col < Width - 1;

        public bool CanMoveSouthWest(IHex hex) => CanMoveDown(hex) && hex.Col > 0;

        public bool CanMoveNorthWest(IHex hex) => CanMoveUp(hex) && hex.Col > 0;

        public bool CanMoveNorthEast(IHex hex) => CanMoveUp(hex) && hex.Col < Width - 1;

        private bool CanMoveDown(IHex hex) => hex.Row < Height - 1;

        private bool CanMoveUp(IHex hex) => hex.Row > 0;
    }
}
