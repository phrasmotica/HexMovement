namespace HexGrid
{
    public class HexGridPath
    {
        /// <summary>
        /// Returns the cost of movement along the given path, recursively.
        /// </summary>
        public static List<int> ComputeCosts(List<Hex> path)
        {
            if (path.Count < 2) 
            {
                return new List<int>();
            }

            var cost = ComputeCost(path[0], path[1]);
            var remainingHexes = path.Skip(1).ToList();

            return ComputeCosts(remainingHexes).Prepend(cost).ToList();
        }

        /// <summary>
        /// Returns the shortest path between two hexes in a double-width coordinate system grid.
        /// Will account for the grid having wrapping enabled.
        /// </summary>
        public static List<Hex> ComputeWrappedPath(HexGrid grid, Hex start, Hex end)
        {
            var path = ComputePath(grid, start, end);

            if (grid.WrapMovement)
            {
                // We'll bring the candidate (rightmost) hex "around" the edges of the grid,
                // and find the shortest path to each of these wrapped positions.
                // Not sure why we don't need to ensure the candidate is also the bottom-most
                // hex, but this seems to be working!
                var (candidateHex, otherHex) = start.Col > end.Col ? (start, end) : (end, start);

                var candidateDistances = new[]
                {
                    path,

                    // wrapped around the west edge
                    ComputePath(grid, otherHex, grid.HexAt(candidateHex.Row, candidateHex.Col - grid.Width)),

                    // wrapped around the north edge
                    ComputePath(grid, otherHex, grid.HexAt(candidateHex.Row - grid.Height, candidateHex.Col)),

                    // wrapped around both west and north edges
                    ComputePath(grid, otherHex, grid.HexAt(candidateHex.Row - grid.Height, candidateHex.Col - grid.Width)),
                };

                return candidateDistances.MinBy(p => p.Count)!;
            }

            return path;
        }

        /// <summary>
        /// Returns the shortest path between two hexes in a grid using the A* algorithm.
        ///
        /// Taken from https://www.redblobgames.com/grids/hexagons/#pathfinding.
        /// </summary>
        private static List<Hex> ComputePath(HexGrid grid, Hex start, Hex end)
        {
            var frontier = new PriorityQueue<Hex, int>();
            frontier.Enqueue(start, 0);

            var cameFrom = new Dictionary<Hex, Hex?>
            {
                [start] = null,
            };

            var costSoFar = new Dictionary<Hex, int>
            {
                [start] = 0,
            };

            Hex current;

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

            return BuildPath(cameFrom, start, end);
        }

        /// <summary>
        /// Returns the cost of moving from one hex to another. Moving to a hill costs 2, else costs 1.
        /// </summary>
        private static int ComputeCost(Hex current, Hex next) => (current.Terrain, next.Terrain) switch
        {
            (_, Terrain.Hill) => 2,
            (_, _) => 1,
        };

        /// <summary>
        /// Computes the taxicab distance between the two hexes, as a heuristic for the A* algorithm.
        /// 
        /// Taken from https://www.redblobgames.com/pathfinding/a-star/introduction.html#greedy-best-first.
        /// </summary>
        private static int ComputeHeuristic(HexGrid grid, Hex a, Hex b)
        {
            return HexGridDistance.ComputeWrappedDistance(grid, a, b);
        }

        /// <summary>
        /// Returns a list of hexes in the shortest path between the start and the end hexes, given
        /// the dictionary of connections.
        ///
        /// Taken from https://www.redblobgames.com/pathfinding/a-star/introduction.html#breadth-first-search.
        /// </summary>
        private static List<Hex> BuildPath(Dictionary<Hex, Hex?> cameFrom, Hex start, Hex end)
        {
            var current = end;

            var path = new List<Hex>();

            while (current != start)
            {
                path.Add(current!);
                current = cameFrom[current!];
            }

            return path.Reverse<Hex>().ToList();
        }
    }
}
