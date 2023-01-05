namespace HexMovement
{
    /// <summary>
    /// Represents a hexagonal grid in which a player can move.
    /// </summary>
    public class HexGrid
    {
        public int Width { get; }

        public int Height { get; }

        private readonly List<List<Hex>> _grid;

        public Player Player { get; private set; }

        public event Action<Player>? OnMove;

        public HexGrid(int width, int height)
        {
            Width = width;
            Height = height;

            _grid = new();

            for (var i = 0; i < height; i++)
            {
                var row = new List<Hex>();

                for (var j = 0; j < width; j++)
                {
                    row.Add(new());
                }

                _grid.Add(row);
            }

            Player = new();
        }

        public void MoveRight()
        {
            MoveRightAndWrap();
            OnMove?.Invoke(Player);
        }

        public void MoveDownRight()
        {
            MoveDownAndWrap();

            if (Player.PosY % 2 == 0)
            {
                MoveRightAndWrap();
            }

            OnMove?.Invoke(Player);
        }

        public void MoveDownLeft()
        {
            MoveDownAndWrap();

            if (Player.PosY % 2 != 0)
            {
                MoveLeftAndWrap();
            }

            OnMove?.Invoke(Player);
        }

        public void MoveLeft()
        {
            MoveLeftAndWrap();
            OnMove?.Invoke(Player);
        }

        public void MoveUpLeft()
        {
            MoveUpAndWrap();

            if (Player.PosY % 2 != 0)
            {
                MoveLeftAndWrap();
            }

            OnMove?.Invoke(Player);
        }

        public void MoveUpRight()
        {
            MoveUpAndWrap();

            if (Player.PosY % 2 == 0)
            {
                MoveRightAndWrap();
            }

            OnMove?.Invoke(Player);
        }

        private void MoveRightAndWrap()
        {
            if (Player.PosX == Width - 1)
            {
                // wrap around when they're at the end of a row
                Player.PosX = 0;
            }
            else
            {
                Player.MoveRight();
            }
        }

        private void MoveDownAndWrap()
        {
            if (Player.PosY == Height - 1)
            {
                // wrap around when they're in the last row
                Player.PosY = 0;
            }
            else
            {
                Player.MoveDown();
            }
        }

        private void MoveLeftAndWrap()
        {
            if (Player.PosX == 0)
            {
                // wrap around when they're at the start of a row
                Player.PosX = Width - 1;
            }
            else
            {
                Player.MoveLeft();
            }
        }

        private void MoveUpAndWrap()
        {
            if (Player.PosY == 0)
            {
                // wrap around when they're in the first row
                Player.PosY = Height - 1;
            }
            else
            {
                Player.MoveUp();
            }
        }

        public override string ToString()
        {
            return string.Join("\n", _grid.Select(RowToString));
        }

        private string RowToString(List<Hex> hexes, int row)
        {
            var joined = string.Join(" ", hexes.Select((h, col) => HexToString(row, col)));

            if (row % 2 != 0)
            {
                joined = " " + joined;
            }

            return joined;
        }

        private string HexToString(int row, int col)
        {
            return IsOccupied(row, col) ? "X" : ".";
        }

        private bool IsOccupied(int row, int col)
        {
            return Player.PosX == col && Player.PosY == row;
        }
    }
}
