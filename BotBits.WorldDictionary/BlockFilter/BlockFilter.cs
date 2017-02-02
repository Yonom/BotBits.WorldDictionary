namespace BotBits.WorldDictionary
{
    public static class BlockFilter
    {
        public static CompositeBlockFilter All => new CompositeBlockFilter((id, b) => true, (id, b) => true);
        public static CompositeBlockFilter ForegroundOnly => new CompositeBlockFilter((id, b) => true, (id, b) => false);
        public static CompositeBlockFilter BackgroundOnly => new CompositeBlockFilter((id, b) => false, (id, b) => true);
        public static CompositeBlockFilter None => new CompositeBlockFilter((id, b) => false, (id, b) => false);
    }
}