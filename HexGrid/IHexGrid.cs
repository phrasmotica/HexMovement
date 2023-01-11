namespace HexGrid
{
    public interface IHexGrid
    {
        int Height { get; }

        List<List<IHex>> Rows { get; }

        int Width { get; }

        bool WrapMovement { get; set; }

        bool CanMoveEast(IHex hex);

        bool CanMoveNorthEast(IHex hex);

        bool CanMoveNorthWest(IHex hex);

        bool CanMoveSouthEast(IHex hex);

        bool CanMoveSouthWest(IHex hex);

        bool CanMoveWest(IHex hex);

        List<IHex> GetNeighbours(IHex hex);

        int ComputeWrappedDistance(IHex start, IHex end);

        Path ComputeWrappedPath(IHex start, IHex end);

        IHex HexAt(int row, int col);

        IHex MoveEast(IHex hex);

        IHex MoveNorthEast(IHex hex);

        IHex MoveNorthWest(IHex hex);

        IHex MoveSouthEast(IHex hex);

        IHex MoveSouthWest(IHex hex);

        IHex MoveWest(IHex hex);
    }
}
