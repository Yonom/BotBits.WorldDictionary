using System.Collections.Generic;
using System.Xml;

namespace BotBits.WorldDictionary
{
    public interface IBlockQuery<out TKey, out TItem> 
        : IEnumerable<TItem>
        where TKey : struct 
        where TItem : struct
    {
        IEnumerable<Point> Locations { get; }
        bool Contains(Point item);
        int Count { get; }
        TKey Key { get; }

        TItem At(Point point);
    }
}