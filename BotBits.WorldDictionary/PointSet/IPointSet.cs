using System.Collections.Generic;

namespace BotBits.WorldDictionary
{
    public interface IPointSet : IEnumerable<Point>
    {
        void Add(Point point);
        bool Remove(Point point);
        int Count { get; }
        bool Contains(Point point);
    }
}