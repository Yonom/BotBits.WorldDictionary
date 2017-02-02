using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotBits.WorldDictionary
{
    internal class BlockNestedQuery<TId, TBlock, TItem> : IBlockNestedQuery<TId, TBlock, TItem>, IBlockQuery<TId, TItem>
        where TId : struct
        where TBlock : struct
        where TItem : struct
    {
        private readonly IDictionaryLayerGenerator<TId, TBlock, TItem> _generator;
        private readonly IBlockQuery<TBlock, TItem>[] _sets;

        public BlockNestedQuery(TId key, IBlockQuery<TBlock, TItem>[] sets, IDictionaryLayerGenerator<TId, TBlock, TItem> generator)
        {
            this.Key = key;
            this._sets = sets;
            this._generator = generator;
        }

        public int Count => this._sets.Sum(s => s.Count);
        public int BlockCount => this.Count;
        public int QueryCount => this._sets.Length;

        public TId Key { get; }

        public TItem At(Point point)
        {
            var block = this._sets.Where(s => s.Contains(point)).Select(s => s.Key).Cast<TBlock?>().FirstOrDefault();
            if (!block.HasValue) throw new KeyNotFoundException("The given block id does not exist at the given point.");
            return this._generator.GenerateItem(point, block.Value);
        }

        public IEnumerable<Point> Locations => this._sets.SelectMany(s => s.Locations);

        public IEnumerator<IBlockQuery<TBlock, TItem>> GetEnumerator()
        {
            return ((IEnumerable<IBlockQuery<TBlock, TItem>>)this._sets).GetEnumerator();
        }

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            return this._sets.SelectMany(s => s).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IBlockQuery<TId, TItem> ToQuery()
        {
            return this;
        }

        public bool Contains(Point point)
        {
            return this._sets.Any(s => s.Contains(point));
        }
    }
}