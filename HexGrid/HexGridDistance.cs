namespace HexGrid
{
    public class HexGridDistance
    {
        /// <summary>
        /// Returns the distance between two hexes in a double-width coordinate system
        /// in terms of the number of hexes, NOT the coordinate distance.
        ///
        /// Taken from https://www.redblobgames.com/grids/hexagons/#distances-doubled.
        /// </summary>
        public static int ComputeDistanceDoubleWidth(Hex start, Hex end)
        {
            var distX = Math.Abs(start.Col - end.Col);
            var distY = Math.Abs(start.Row - end.Row);

            return distY + Math.Max(0, (distX - distY) / 2);
        }
    }
}
