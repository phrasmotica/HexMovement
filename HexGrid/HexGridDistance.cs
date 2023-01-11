namespace HexGrid
{
    public class HexGridDistance
    {
        /// <summary>
        /// Returns the distance between two hexes in terms of the number of hexes. Will account for
        /// the grid having wrapping enabled.
        /// </summary>
        public static int ComputeWrappedDistance(IHexGrid grid, IHex start, IHex end)
        {
            var distance = ComputeDistance(start, end);

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
                    ComputeDistance(otherHex, new DoubleWidthHex(candidateHex.Row, candidateHex.Col - grid.Width)),

                    // wrapped around the north edge
                    ComputeDistance(otherHex, new DoubleWidthHex(candidateHex.Row - grid.Height, candidateHex.Col)),

                    // wrapped around both west and north edges
                    ComputeDistance(otherHex, new DoubleWidthHex(candidateHex.Row - grid.Height, candidateHex.Col - grid.Width)),
                };

                return candidateDistances.Min();
            }

            return distance;
        }

        /// <summary>
        /// Returns the distance between two hexes in terms of the number of hexes, NOT the
        /// coordinate distance.
        ///
        /// Taken from https://www.redblobgames.com/grids/hexagons/#distances-axial.
        /// </summary>
        private static int ComputeDistance(IHex start, IHex end) => ComputeDistance(start.Q, start.R, end.Q, end.R);

        /// <summary>
        /// Returns the distance between two hexes in terms of the number of hexes, NOT the
        /// coordinate distance.
        ///
        /// Taken from https://www.redblobgames.com/grids/hexagons/#distances-axial.
        /// </summary>
        private static int ComputeDistance(int startQ, int startR, int endQ, int endR)
        {
            return (Math.Abs(startQ - endQ)
                  + Math.Abs(startQ + startR - endQ - endR)
                  + Math.Abs(startR - endR)) / 2;
        }
    }
}
