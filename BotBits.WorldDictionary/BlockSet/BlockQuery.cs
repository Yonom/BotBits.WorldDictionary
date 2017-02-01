using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotBits.WorldDictionary
{
    internal class BlockQuery<TId, TKey, TItem> : IBlockQuery<TKey, TItem>
        where TId : struct
        where TKey : struct
        where TItem : struct
    {
        private readonly ConcurrentDictionary<Point, byte> _set;
        private readonly IDictionaryLayerGenerator<TId, TKey, TItem> _generator;

        public BlockQuery(TKey key, ConcurrentDictionary<Point, byte> set, IDictionaryLayerGenerator<TId, TKey, TItem> generator)
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

        public IEnumerable<Point> Locations => this._set.Keys.Select(p => new Point(p.X, p.Y));

        public IEnumerator<TItem> GetEnumerator()
        {
            return this._set.Select(p => this._generator.GenerateItem(new Point(p.Key.X, p.Key.Y), this.Key)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this._set).GetEnumerator();
        }

        public bool Contains(Point point)
        {
            return this._set.ContainsKey(new Point((ushort)point.X, (ushort)point.Y));
        }
    }
}