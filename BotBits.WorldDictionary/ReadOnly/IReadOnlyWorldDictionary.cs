namespace BotBits.WorldDictionary
{
    public interface IReadOnlyWorldDictionary<TItem>
        where TItem : struct
    {
        int Width { get; }
        int Height { get; }

        IDictionaryBlockLayer<Foreground.Id, ForegroundBlock, TItem> Foreground { get; }
        IDictionaryBlockLayer<Background.Id, BackgroundBlock, TItem> Background { get; }

        IBlockQuery<ForegroundBlock, TItem> this[ForegroundBlock block] { get; }
        IBlockQuery<Foreground.Id, TItem> this[Foreground.Id id] { get; }
        IBlockQuery<BackgroundBlock, TItem> this[BackgroundBlock block] { get; }
        IBlockQuery<Background.Id, TItem> this[Background.Id id] { get; }
    }
}