using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotBits.WorldDictionary
{
    internal class DictionaryBlockLayer<TId, TBlock, TItem> : IDictionaryBlockLayer<TId, TBlock, TItem>
        where TId : struct
        where TBlock : struct
        where TItem : struct
    {
        private readonly IBlockFilter<TId, TBlock> _filter;
        private readonly IDictionaryLayerGenerator<TId, TBlock, TItem> _generator;

        private readonly ConcurrentDictionary<TId, ConcurrentDictionary<TBlock, ConcurrentDictionary<Point, byte>>> _items = new ConcurrentDictionary<TId, ConcurrentDictionary<TBlock, ConcurrentDictionary<Point, byte>>>();

        public DictionaryBlockLayer(IDictionaryLayerGenerator<TId, TBlock, TItem> generator, IBlockFilter<TId, TBlock> filter)
        {
            this._generator = generator;
            this._filter = filter;
        }

        public int Count => this._items.Values.Count(i => i.Count > 0);

        public IBlockQuery<TBlock, TItem> this[TBlock block]
        {
            get
            {
                var id = this._generator.GetId(block);

                if (this._filter.ShouldIndex(id, block) == false) throw new NotSupportedException("This Dictionary does not support the given block.");

                var dic = this._items.GetOrAdd(id, _ => new ConcurrentDictionary<TBlock, ConcurrentDictionary<Point, byte>>());
                var res = dic.GetOrAdd(block, _ => new ConcurrentDictionary<Point, byte>());

                return new BlockQuery<TId, TBlock, TItem>(block, res, this._generator);
            }
        }

        public IBlockQuery<TId, TItem> this[TId id]
        {
            get
            {
                if (this._filter.ShouldIndex(id, null) == false) throw new NotSupportedException("This Dictionary does not support the given block.");

                var dic = this._items.GetOrAdd(id, _ => new ConcurrentDictionary<TBlock, ConcurrentDictionary<Point, byte>>());

                return new AggregateBlockQuery<TId, TBlock, TItem>(id, dic, this._generator);
            }
        }

        public IEnumerable<KeyValuePair<TId, IEnumerable<IBlockQuery<TBlock, TItem>>>> GroupedByIdThenByBlock
        {
            get
            {
                return this._items
                    .Select(i => new KeyValuePair<TId, IEnumerable<IBlockQuery<TBlock, TItem>>>(i.Key, i.Value
                        .Where(v => v.Value.Any())
                        .Select(v => this[v.Key])))
                    .Where(i => i.Value.Any());
            }
        }

        public IEnumerable<IBlockQuery<TBlock, TItem>> GroupedByBlock
        {
            get
            {
                return this._items.Values
                    .SelectMany(i => i.Keys)
                    .Select(i => this[i])
                    .Where(i => i.Any());
            }
        }

        public IEnumerable<IBlockQuery<TId, TItem>> GroupedById
        {
            get
            {
                return this._items.Keys
                    .Select(i => this[i])
                    .Where(i => i.Any());
            }
        }

        public IEnumerator<IBlockQuery<TId, TItem>> GetEnumerator()
        {
            return this.GroupedById.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal bool Add(TBlock block, Point point)
        {
            var id = this._generator.GetId(block);

            //if (this._filter.ShouldIndex(id, block) == false) return false;

            var dic = this._items.GetOrAdd(id, _ => new ConcurrentDictionary<TBlock, ConcurrentDictionary<Point, byte>>());
            var res = dic.GetOrAdd(block, _ => new ConcurrentDictionary<Point, byte>());

            return res.TryAdd(point, default(byte));
        }

        internal bool Remove(TBlock block, Point point)
        {
            var id = this._generator.GetId(block);

            ConcurrentDictionary<TBlock, ConcurrentDictionary<Point, byte>> dic;
            if (!this._items.TryGetValue(id, out dic)) return false;

            ConcurrentDictionary<Point, byte> set;
            if (!dic.TryGetValue(block, out set)) return false;

            byte useless;
            return set.TryRemove(point, out useless);
        }

        internal void Clear()
        {
            this._items.Clear();
        }
    }
}