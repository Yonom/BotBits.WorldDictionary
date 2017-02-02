using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotBits.WorldDictionary
{

    internal class BlockQuery<TId, TKey, TItem, TPointSet> : IBlockQuery<TKey, TItem>
        where TId : struct
        where TKey : struct
        where TItem : struct
        where TPointSet : IPointSet
    {
        private readonly IDictionaryLayerGenerator<TId, TKey, TItem> _generator;
        private readonly TPointSet _set;

        public BlockQuery(TKey key, TPointSet set, IDictionaryLayerGenerator<TId, TKey, TItem> generator)
        {
            this.Key = key;
            this._set = set;
            this._generator = generator;
        }

        public TKey Key { get; }

        public TItem At(Point point)
        {
            if (!this.Contains(point)) throw new KeyNotFoundException("The given block does not exist at the given point.");
            return this._generator.GenerateItem(point, this.Key);
        }

        public int Count => this._set.Count;

        public IEnumerable<Point> Locations => this._set;

        public IEnumerator<TItem> GetEnumerator()
        {
            return this._set.Select(p => this._generator.GenerateItem(p, this.Key)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Contains(Point point)
        {
            return this._set.Contains(point);
        }
    }
}