namespace HexGrid
{
    public interface IHexGrid
    {
        int Height { get; }

        List<List<Hex>> Rows { get; }

        int Width { get; }

        bool WrapMovement { get; set; }

        bool CanMoveEast(Hex hex, bool doubleWidth);

        bool CanMoveNorthEast(Hex hex);

        bool CanMoveNorthWest(Hex hex);

        bool CanMoveSouthEast(Hex hex);

        bool CanMoveSouthWest(Hex hex);

        bool CanMoveWest(Hex hex, bool doubleWidth);

        List<Hex> GetNeighbours(Hex hex);

        Hex HexAt(int row, int col);

        Hex MoveEast(Hex hex);

        Hex MoveNorthEast(Hex hex);

        Hex MoveNorthWest(Hex hex);

        Hex MoveSouthEast(Hex hex);

        Hex MoveSouthWest(Hex hex);

        Hex MoveWest(Hex hex);
    }
}
