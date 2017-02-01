namespace BotBits.WorldDictionary
{
    public static class ReadOnlyWorldDictionaryExtensions
    {
        public static World ToWorld<T>(this IReadOnlyWorldDictionary<T> worldDictionary) where T : struct
        {
            var world = new World(worldDictionary.Width, worldDictionary.Height);
            foreach (var block in worldDictionary.Foreground.GroupedByBlock)
                foreach (var location in block.Locations)
                    world.Foreground[location] = block.Key;
            foreach (var block in worldDictionary.Background.GroupedByBlock)
                foreach (var location in block.Locations)
                    world.Background[location] = block.Key;
            return world;
        }

        public static ReadOnlyWorldDictionary ToReadOnlyWorldDictionary(this IReadOnlyWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea, IBlockFilter filter)
        {
            return new ReadOnlyWorldDictionary(worldArea, filter);
        }

        public static ReadOnlyWorldDictionary ToReadOnlyWorldDictionary(this IWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea, IBlockFilter filter)
        {
            return new ReadOnlyWorldDictionary(worldArea, filter);
        }

        public static ReadOnlyWorldDictionary ToReadOnlyWorldDictionary(this IReadOnlyWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea)
        {
            return new ReadOnlyWorldDictionary(worldArea);
        }

        public static ReadOnlyWorldDictionary ToReadOnlyWorldDictionary(this IWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea)
        {
            return new ReadOnlyWorldDictionary(worldArea);
        }
    }
}