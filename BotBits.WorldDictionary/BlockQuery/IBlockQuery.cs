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

    public interface IBlockNestedQuery<out TKey, out TNestedKey, out TItem>
        : IEnumerable<IBlockQuery<TNestedKey, TItem>>
        where TKey : struct
        where TItem : struct 
        where TNestedKey : struct
    {
        TKey Key { get; }
        IEnumerable<Point> Locations { get; }
        bool Contains(Point point);
        TItem At(Point point);

        int QueryCount { get; }
        int BlockCount { get; }

        IBlockQuery<TKey, TItem> ToQuery();
    }
}