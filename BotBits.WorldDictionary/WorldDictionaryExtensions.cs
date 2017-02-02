namespace BotBits.WorldDictionary
{
    public static class WorldDictionaryExtensions
    {
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