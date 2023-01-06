namespace HexGrid
{
    /// <summary>
    /// Represents a hexagonal grid in which something can be positioned.
    /// Contains public methods for moving things around the grid.
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

        public (int, int) MoveRight(int posX, int posY)
        {
            posX = MoveRightAndWrap(posX);

            return (posX, posY);
        }

        public (int, int) MoveDownRight(int posX, int posY)
        {
            posY = MoveDownAndWrap(posY);

            if (posY % 2 == 0)
            {
                posX = MoveRightAndWrap(posX);
            }

            return (posX, posY);
        }

        public (int, int) MoveDownLeft(int posX, int posY)
        {
            posY = MoveDownAndWrap(posY);

            if (posY % 2 != 0)
            {
                posX = MoveLeftAndWrap(posX);
            }

            return (posX, posY);
        }

        public (int, int) MoveLeft(int posX, int posY) => (MoveLeftAndWrap(posX), posY);

        public (int, int) MoveUpLeft(int posX, int posY)
        {
            posY = MoveUpAndWrap(posY);

            if (posY % 2 != 0)
            {
                posX = MoveLeftAndWrap(posX);
            }

            return (posX, posY);
        }

        public (int, int) MoveUpRight(int posX, int posY)
        {
            posY = MoveUpAndWrap(posY);

            if (posY % 2 == 0)
            {
                posX = MoveRightAndWrap(posX);
            }

            return (posX, posY);
        }

        public bool CanMoveRight(int posX) => posX < Width - 1;

        public bool CanMoveDown(int posY) => posY < Height - 1;

        public bool CanMoveLeft(int posX) => posX > 0;

        public bool CanMoveUp(int posY) => posY > 0;

        private int MoveRightAndWrap(int posX)
        {
            var newPos = posX + 1;

            if (CanMoveRight(posX))
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

        private int MoveLeftAndWrap(int posX)
        {
            var newPos = posX - 1;

            if (CanMoveLeft(posX))
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
