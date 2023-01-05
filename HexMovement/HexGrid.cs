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

        public event Action<Player>? OnMoveFail;

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
            if (Player.PosX == Width - 1)
            {
                // player cannot move like this when they're at the end of a row
                OnMoveFail?.Invoke(Player);
                return;
            }

            Player.MoveRight();
            OnMove?.Invoke(Player);
        }

        public void MoveDownRight()
        {
            if (Player.PosY == Height - 1 || (Player.PosY % 2 != 0 && Player.PosX == Width - 1))
            {
                // player cannot move like this when they're in the last row
                // or when they're at the end of an odd row
                OnMoveFail?.Invoke(Player);
                return;
            }

            Player.MoveDown();

            if (Player.PosY % 2 == 0)
            {
                Player.MoveRight();
            }

            OnMove?.Invoke(Player);
        }

        public void MoveDownLeft()
        {
            if (Player.PosY == Height - 1 || (Player.PosY % 2 == 0 && Player.PosX == 0))
            {
                // player cannot move like this when they're in the last row
                // or when they're at the start of an even row
                OnMoveFail?.Invoke(Player);
                return;
            }

            Player.MoveDown();

            if (Player.PosY % 2 != 0)
            {
                Player.MoveLeft();
            }

            OnMove?.Invoke(Player);
        }

        public void MoveLeft()
        {
            if (Player.PosX == 0)
            {
                // player cannot move like this when they're at the end of a row
                OnMoveFail?.Invoke(Player);
                return;
            }

            Player.MoveLeft();
            OnMove?.Invoke(Player);
        }

        public void MoveUpLeft()
        {
            if (Player.PosY == 0 || (Player.PosY % 2 == 0 && Player.PosX == 0))
            {
                // player cannot move like this when they're in the first row
                // or when they're at the start of an even row
                OnMoveFail?.Invoke(Player);
                return;
            }

            Player.MoveUp();

            if (Player.PosY % 2 != 0)
            {
                Player.MoveLeft();
            }

            OnMove?.Invoke(Player);
        }

        public void MoveUpRight()
        {
            if (Player.PosY == 0 || (Player.PosY % 2 != 0 && Player.PosX == Width - 1))
            {
                // player cannot move like this when they're in the first row
                // or when they're at the end of an odd row
                OnMoveFail?.Invoke(Player);
                return;
            }

            Player.MoveUp();

            if (Player.PosY % 2 == 0)
            {
                Player.MoveRight();
            }

            OnMove?.Invoke(Player);
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
