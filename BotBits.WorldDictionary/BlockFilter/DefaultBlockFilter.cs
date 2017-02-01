namespace BotBits.WorldDictionary
{
    internal class DefaultBlockFilter : IBlockFilter
    {
        private DefaultBlockFilter()
        {
        }

        public static DefaultBlockFilter Value { get; } = new DefaultBlockFilter();

        public bool ShouldIndex(Foreground.Id id, ForegroundBlock? block)
        {
            return true;
        }

        public bool ShouldIndex(Background.Id id, BackgroundBlock? block)
        {
            return true;
        }
    }
}