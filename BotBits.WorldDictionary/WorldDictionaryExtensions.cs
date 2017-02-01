namespace BotBits.WorldDictionary
{
    public static class WorldDictionaryExtensions
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

        public static WorldDictionary ToWorldDictionary(this IReadOnlyWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea)
        {
            return new WorldDictionary(worldArea);
        }

        public static WorldDictionary ToWorldDictionary(this IWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea)
        {
            return new WorldDictionary(worldArea);
        }

        public static void Update<T>(this IWorldDictionary<T> worldDictionary, int x, int y, ForegroundBlock oldBlock, ForegroundBlock newBlock) where T : struct
        {
            worldDictionary.Update(new Point(x, y), oldBlock, newBlock);
        }

        public static void Update<T>(this IWorldDictionary<T> worldDictionary, int x, int y, BackgroundBlock oldBlock, BackgroundBlock newBlock) where T : struct
        {
            worldDictionary.Update(new Point(x, y), oldBlock, newBlock);
        }

        public static bool TryUpdate<T>(this IWorldDictionary<T> worldDictionary, int x, int y, ForegroundBlock oldBlock, ForegroundBlock newBlock) where T : struct
        {
            return worldDictionary.TryUpdate(new Point(x, y), oldBlock, newBlock);
        }

        public static bool TryUpdate<T>(this IWorldDictionary<T> worldDictionary, int x, int y, BackgroundBlock oldBlock, BackgroundBlock newBlock) where T : struct
        {
            return worldDictionary.TryUpdate(new Point(x, y), oldBlock, newBlock);
        }
    }
}