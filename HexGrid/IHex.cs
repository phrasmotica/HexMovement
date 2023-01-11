namespace HexGrid
{
    public interface IHex
    {
        int Row { get; }

        int Col { get; }

        int Q { get; }

        int R { get; }

        int S { get; }

        Terrain Terrain { get; set; }

        (int, int) EastNeighbour();

        (int, int) SouthEastNeighbour();

        (int, int) SouthWestNeighbour();

        (int, int) WestNeighbour();

        (int, int) NorthWestNeighbour();

        (int, int) NorthEastNeighbour();
    }
}
