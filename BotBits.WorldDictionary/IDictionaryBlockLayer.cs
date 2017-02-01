using System.Collections.Generic;

namespace BotBits.WorldDictionary
{
    public interface IDictionaryBlockLayer<TId, TBlock, TItem>
        : IEnumerable<IBlockQuery<TId, TItem>>
        where TId : struct
        where TBlock : struct
        where TItem : struct
    {
        int Count { get; }
        IBlockQuery<TBlock, TItem> this[TBlock block] { get; }
        IBlockQuery<TId, TItem> this[TId id] { get; }

        IEnumerable<IBlockQuery<TId, TItem>> GroupedById { get; }
        IEnumerable<IBlockQuery<TBlock, TItem>> GroupedByBlock { get; }
        IEnumerable<KeyValuePair<TId, IEnumerable<IBlockQuery<TBlock, TItem>>>> GroupedByIdThenByBlock { get; }
    }
}