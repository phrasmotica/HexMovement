namespace HexGrid
{
    /// <summary>
    /// Class for calculating paths between hexes, using the A* algorithm.
    /// 
    /// Taken from https://www.redblobgames.com/grids/hexagons/#pathfinding.
    /// </summary>
    public class AStarPathCalculator : IPathCalculator
    {
        public Path Compute(IHexGrid grid, IHex start, IHex end)
        {
            var frontier = new PriorityQueue<IHex, int>();
            frontier.Enqueue(start, 0);

            var cameFrom = new Dictionary<IHex, IHex?>
            {
                [start] = null,
            };

            var costSoFar = new Dictionary<IHex, int>
            {
                [start] = 0,
            };

            IHex current;

            while (frontier.Count > 0)
            {
                current = frontier.Dequeue();

                if (current == end)
                {
                    break;
                }

                foreach (var next in grid.GetNeighbours(current))
                {
                    var newCost = costSoFar[current] + ComputeCost(current, next);

                    if (!costSoFar.TryGetValue(next, out var nextCost) || newCost < nextCost)
                    {
                        costSoFar[next] = newCost;

                        var priority = newCost + ComputeHeuristic(grid, end, next);
                        frontier.Enqueue(next, priority);

                        cameFrom[next] = current;
                    }
                }
            }

            var hexes = BuildPath(cameFrom, start, end);

            return new Path(hexes);
        }

        /// <summary>
        /// Returns the cost of moving from one hex to another. Moving to a hill costs 2, else costs 1.
        /// </summary>
        private static int ComputeCost(IHex current, IHex next) => (current.Terrain, next.Terrain) switch
        {
            (_, Terrain.Hill) => 2,
            (_, _) => 1,
        };

        /// <summary>
        /// Computes the distance between the two hexes, as a heuristic for the A* algorithm.
        ///
        /// Taken from https://www.redblobgames.com/pathfinding/a-star/introduction.html#greedy-best-first.
        /// </summary>
        private static int ComputeHeuristic(IHexGrid grid, IHex a, IHex b) => grid.ComputeWrappedDistance(a, b);

        private static List<IHex> BuildPath(Dictionary<IHex, IHex?> connections, IHex start, IHex end)
        {
            var current = end;

            var path = new List<IHex>();

            while (current != start)
            {
                path.Add(current!);
                current = connections[current!];
            }

            path = path.Reverse<IHex>().ToList();
            path.Insert(0, start);

            return path;
        }
    }
}
