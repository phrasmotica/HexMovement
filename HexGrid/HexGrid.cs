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

        private int MoveRightAndWrap(int posX)
        {
            if (posX == Height - 1)
            {
                // wrap around when they're in the last row
                posX = 0;
            }
            else
            {
                posX += 1;
            }

            return posX;
        }

        private int MoveDownAndWrap(int posY)
        {
            if (posY == Height - 1)
            {
                // wrap around when they're in the last row
                posY = 0;
            }
            else
            {
                posY += 1;
            }

            return posY;
        }

        private int MoveLeftAndWrap(int posX)
        {
            if (posX == 0)
            {
                // wrap around when they're at the start of a row
                posX = Width - 1;
            }
            else
            {
                posX -= 1;
            }

            return posX;
        }

        private int MoveUpAndWrap(int posY)
        {
            if (posY == 0)
            {
                // wrap around when they're in the first row
                posY = Height - 1;
            }
            else
            {
                posY -= 1;
            }

            return posY;
        }
    }
}
