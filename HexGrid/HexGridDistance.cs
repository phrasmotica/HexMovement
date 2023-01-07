namespace HexGrid
{
    public class HexGridDistance
    {
        /// <summary>
        /// Returns the distance between two hexes in a double-width coordinate system grid, in
        /// terms of the number of hexes. Will account for the grid having wrapping enabled.
        /// </summary>
        public static int ComputeWrappedDistanceDoubleWidth(HexGrid grid, Hex start, Hex end)
        {
            var distance = ComputeDistanceDoubleWidth(start.Row, start.Col, end.Row, end.Col);

            if (grid.WrapMovement)
            {
                // We'll bring the candidate (rightmost) hex "around" the edges of the grid,
                // and find the shortest distance to each of these wrapped positions.
                // Not sure why we don't need to ensure the candidate is also the bottom-most
                // hex, but this seems to be working!
                var (candidateHex, otherHex) = start.Col > end.Col ? (start, end) : (end, start);

                var candidateDistances = new[]
                {
                    distance,

                    // wrapped around the west edge
                    ComputeDistanceDoubleWidth(otherHex.Row, otherHex.Col, candidateHex.Row, candidateHex.Col - grid.Width),

                    // wrapped around the north edge
                    ComputeDistanceDoubleWidth(otherHex.Row, otherHex.Col, candidateHex.Row - grid.Height, candidateHex.Col),

                    // wrapped around both west and north edges
                    ComputeDistanceDoubleWidth(otherHex.Row, otherHex.Col, candidateHex.Row - grid.Height, candidateHex.Col - grid.Width),
                };

                return candidateDistances.Min();
            }

            return distance;
        }

        /// <summary>
        /// Returns the distance between two positions in a double-width coordinate system
        /// in terms of the number of hexes, NOT the coordinate distance.
        ///
        /// Taken from https://www.redblobgames.com/grids/hexagons/#distances-doubled.
        /// </summary>
        private static int ComputeDistanceDoubleWidth(int startRow, int startCol, int endRow, int endCol)
        {
            var distX = Math.Abs(startCol - endCol);
            var distY = Math.Abs(startRow - endRow);

            return distY + Math.Max(0, (distX - distY) / 2);
        }
    }
}
