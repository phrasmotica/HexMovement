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

            for (var i = 0; i < height; i++)
            {
                var row = new List<Hex>();

                for (var j = 0; j < width; j++)
                {
                    row.Add(new());
                }

                Rows.Add(row);
            }
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
