namespace HexGrid
{
    public interface IPathCalculator
    {
        Path Compute(IHexGrid grid, IHex start, IHex end);
    }
}
