using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotBits.WorldDictionary
{
    internal class DictionaryBlockLayer<TId, TBlock, TItem, TPointSet> : IDictionaryBlockLayer<TId, TBlock, TItem>
        where TId : struct
        where TBlock : struct
        where TItem : struct
        where TPointSet : IPointSet, new()
    {
        private readonly IBlockFilter<TId, TBlock> _filter;
        private readonly IDictionaryLayerGenerator<TId, TBlock, TItem> _generator;

        private readonly ConcurrentDictionary<TId, ConcurrentDictionary<TBlock, TPointSet>> _items = new ConcurrentDictionary<TId, ConcurrentDictionary<TBlock, TPointSet>>();

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

                if (this._filter?.ShouldIndex(id, block) == false) throw new NotSupportedException("This Dictionary does not support the given block.");

                var dic = this._items.GetOrAdd(id, _ => new ConcurrentDictionary<TBlock, TPointSet>());
                var res = dic.GetOrAdd(block, _ => new TPointSet());

                return new BlockQuery<TId, TBlock, TItem, TPointSet>(block, res, this._generator);
            }
        }

        public IBlockQuery<TId, TItem> this[TId id] => this.GetByBlock(id).ToQuery();

        public IBlockNestedQuery<TId, TBlock, TItem> GetByBlock(TId id)
        {
            if (this._filter?.ShouldIndex(id, null) == false) throw new NotSupportedException("This Dictionary does not support the given block.");

            var dic = this._items.GetOrAdd(id, _ => new ConcurrentDictionary<TBlock, TPointSet>());

            return new BlockNestedQuery<TId, TBlock, TItem>(id, dic
                .Where(i => i.Value.Any())
                .Select(i => new BlockQuery<TId, TBlock, TItem, TPointSet>(i.Key, i.Value, this._generator))
                .ToArray<IBlockQuery<TBlock, TItem>>(),
                this._generator);
        }

        public IEnumerable<IBlockNestedQuery<TId, TBlock, TItem>> GroupedByIdThenByBlock
        {
            get
            {
                return this._items.Keys
                    .Select(this.GetByBlock)
                    .Where(i => i.Any());
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

        internal void Add(TBlock block, Point point)
        {
            var id = this._generator.GetId(block);

            if (this._filter?.ShouldIndex(id, block) == false) return;

            var dic = this._items.GetOrAdd(id, _ => new ConcurrentDictionary<TBlock, TPointSet>());
            var res = dic.GetOrAdd(block, _ => new TPointSet());

            res.Add(point);
        }

        internal bool Remove(TBlock block, Point point)
        {
            var id = this._generator.GetId(block);

            if (this._filter?.ShouldIndex(id, block) == false) return true;

            ConcurrentDictionary<TBlock, TPointSet> dic;
            if (!this._items.TryGetValue(id, out dic)) return false;

            TPointSet set;
            if (!dic.TryGetValue(block, out set)) return false;
            
            return set.Remove(point);
        }

        internal void Clear()
        {
            this._items.Clear();
        }
    }
}