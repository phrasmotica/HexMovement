namespace HexGrid
{
    public class HexGridDistance
    {
        /// <summary>
        /// Returns the distance between two points in a double-width coordinate system
        /// in terms of the number of hexes, NOT the coordinate distance.
        ///
        /// Taken from https://www.redblobgames.com/grids/hexagons/#distances-doubled.
        /// </summary>
        public static int ComputeDistanceDoubleWidth(int startX, int startY, int endX, int endY)
        {
            var distX = Math.Abs(startX - endX);
            var distY = Math.Abs(startY - endY);

            return distY + Math.Max(0, (distX - distY) / 2);
        }
    }
}
