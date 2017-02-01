namespace BotBits.WorldDictionary
{
    public interface IWorldDictionary<TItem> : IReadOnlyWorldDictionary<TItem>
        where TItem : struct
    {
        void Update(Point point, ForegroundBlock oldBlock, ForegroundBlock newBlock);
        void Update(Point point, BackgroundBlock oldBlock, BackgroundBlock newBlock);
        bool TryUpdate(Point point, ForegroundBlock oldBlock, ForegroundBlock newBlock);
        bool TryUpdate(Point point, BackgroundBlock oldBlock, BackgroundBlock newBlock);
    }
}