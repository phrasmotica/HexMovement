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

            for (var y = 0; y < height; y++)
            {
                var row = new List<Hex>();

                // odd rows start at X = 1
                var startX = y % 2 != 0 ? 1 : 0;

                for (var x = startX; x < width; x += 2)
                {
                    row.Add(new(y, x));
                }

                Rows.Add(row);
            }
        }

        public List<Hex> GetNeighbours(int posX, int posY)
        {
            var coords = new[]
            {
                (posX - 1, posY - 1), (posX + 1, posY - 1), // row above
                (posX - 2, posY), (posX + 2, posY), // this row
                (posX - 1, posY + 1), (posX + 1, posY + 1), // row below
            }
            // filter to only those hexes that are in the grid
            .Where(c => c.Item2 >= 0 && c.Item2 < Height && c.Item1 >= 0 && (c.Item1 / 2) < Rows[c.Item2].Count)
            .ToList();

            return coords.Select(c => Rows[c.Item2][c.Item1 / 2]).ToList();
        }

        public (int, int) MoveRight(int posX, int posY) => (MoveRightAndWrap(posX, true), posY);

        public (int, int) MoveDownRight(int posX, int posY)
        {
            if (!WrapMovement && !CanMoveDownRight(posX, posY))
            {
                return (posX, posY);
            }

            return (MoveRightAndWrap(posX, false), MoveDownAndWrap(posY));
        }

        public (int, int) MoveDownLeft(int posX, int posY)
        {
            if (!WrapMovement && !CanMoveDownLeft(posX, posY))
            {
                return (posX, posY);
            }

            return (MoveLeftAndWrap(posX, false), MoveDownAndWrap(posY));
        }

        public (int, int) MoveLeft(int posX, int posY) => (MoveLeftAndWrap(posX, true), posY);

        public (int, int) MoveUpLeft(int posX, int posY)
        {
            if (!WrapMovement && !CanMoveUpLeft(posX, posY))
            {
                return (posX, posY);
            }

            return (MoveLeftAndWrap(posX, false), MoveUpAndWrap(posY));
        }

        public (int, int) MoveUpRight(int posX, int posY)
        {
            if (!WrapMovement && !CanMoveUpRight(posX, posY))
            {
                return (posX, posY);
            }

            return (MoveRightAndWrap(posX, false), MoveUpAndWrap(posY));
        }

        public bool CanMoveRight(int posX, bool doubleWidth) => posX < Width - (doubleWidth ? 2 : 1);

        public bool CanMoveLeft(int posX, bool doubleWidth) => posX > (doubleWidth ? 1 : 0);

        public bool CanMoveDownRight(int posX, int posY) => CanMoveDown(posY) && CanMoveRight(posX, false);

        public bool CanMoveDownLeft(int posX, int posY) => CanMoveDown(posY) && CanMoveLeft(posX, false);

        public bool CanMoveUpLeft(int posX, int posY) => CanMoveUp(posY) && CanMoveLeft(posX, false);

        public bool CanMoveUpRight(int posX, int posY) => CanMoveUp(posY) && CanMoveRight(posX, false);

        private bool CanMoveDown(int posY) => posY < Height - 1;

        private bool CanMoveUp(int posY) => posY > 0;

        private int MoveRightAndWrap(int posX, bool doubleWidth)
        {
            var newPos = doubleWidth ? posX + 2 : posX + 1;

            if (CanMoveRight(posX, doubleWidth))
            {
                return newPos;
            }

            return WrapMovement ? newPos % Width : posX;
        }

        private int MoveDownAndWrap(int posY)
        {
            var newPos = posY + 1;

            if (CanMoveDown(posY))
            {
                return newPos;
            }

            return WrapMovement ? newPos % Height : posY;
        }

        private int MoveLeftAndWrap(int posX, bool doubleWidth)
        {
            var newPos = doubleWidth ? posX - 2 : posX - 1;

            if (CanMoveLeft(posX, doubleWidth))
            {
                return newPos;
            }

            // cannot use modulo on negative ints
            return WrapMovement ? newPos + Width : posX;
        }

        private int MoveUpAndWrap(int posY)
        {
            var newPos = posY - 1;

            if (CanMoveUp(posY))
            {
                return newPos;
            }

            // cannot use modulo on negative ints
            return WrapMovement ? newPos + Height : posY;
        }
    }
}
