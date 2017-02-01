using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotBits.WorldDictionary
{
    internal class AggregateBlockQuery<TKey, TBlock, TItem, TPointSet> : IBlockQuery<TKey, TItem>
        where TKey : struct
        where TBlock : struct
        where TItem : struct
        where TPointSet : IPointSet
    {
        private readonly IDictionaryLayerGenerator<TKey, TBlock, TItem> _generator;
        private readonly IEnumerable<KeyValuePair<TBlock, TPointSet>> _sets;

        public AggregateBlockQuery(TKey key, IEnumerable<KeyValuePair<TBlock, TPointSet>> sets, IDictionaryLayerGenerator<TKey, TBlock, TItem> generator)
        {
            this.Key = key;
            this._sets = sets;
            this._generator = generator;
        }

        public int Count => this._sets.Sum(s => s.Value.Count);

        public TKey Key { get; }

        public TItem At(Point point)
        {
            var block = this._sets.Where(s => s.Value.Contains(point)).Select(s => s.Key).Cast<TBlock?>().FirstOrDefault();
            if (!block.HasValue) throw new KeyNotFoundException("The given block id does not exist at the given point.");
            return this._generator.GenerateItem(point, block.Value);
        }

        public IEnumerable<Point> Locations => this._sets.SelectMany(s => s.Value);

        public IEnumerator<TItem> GetEnumerator()
        {
            return this._sets.SelectMany(s => s.Value.Select(v => this._generator.GenerateItem(v, s.Key))).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Contains(Point point)
        {
            return this._sets.Any(s => s.Value.Contains(point));
        }
    }
}