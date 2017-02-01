using System.Collections.Generic;

namespace BotBits.WorldDictionary
{
    public interface IBlockQuery<out TKey, out TItem>
        : IEnumerable<TItem>
        where TKey : struct
        where TItem : struct
    {
        IEnumerable<Point> Locations { get; }
        int Count { get; }
        TKey Key { get; }
        bool Contains(Point point);

        TItem At(Point point);
    }
}