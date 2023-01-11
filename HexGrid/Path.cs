namespace HexGrid
{
    public class Path
    {
        public List<IHex> Hexes { get; }

        public int Length => Hexes.Count - 1;

        public Path(List<IHex> hexes)
        {
            Hexes = hexes;
        }

        /// <summary>
        /// Returns the cost of movement along the given path, recursively.
        /// </summary>
        public List<int> ComputeCosts() => ComputeCosts(Hexes);

        private static List<int> ComputeCosts(List<IHex> hexes)
        {
            if (hexes.Count < 2)
            {
                return new List<int>();
            }

            var cost = ComputeCost(hexes[0], hexes[1]);
            var remainingHexes = hexes.Skip(1).ToList();

            return ComputeCosts(remainingHexes).Prepend(cost).ToList();
        }

        /// <summary>
        /// Returns the cost of moving from one hex to another. Moving to a hill costs 2, else costs 1.
        /// </summary>
        private static int ComputeCost(IHex current, IHex next) => (current.Terrain, next.Terrain) switch
        {
            (_, Terrain.Hill) => 2,
            (_, _) => 1,
        };
    }
}
