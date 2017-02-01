using System;

namespace BotBits.WorldDictionary
{
    internal class ForegroundDictionaryLayerGenerator<TItem> : IDictionaryLayerGenerator<Foreground.Id, ForegroundBlock, TItem>
        where TItem : struct
    {
        private readonly Func<Point, ForegroundBlock, TItem> _itemGenerator;

        public ForegroundDictionaryLayerGenerator(Func<Point, ForegroundBlock, TItem> itemGenerator)
        {
            this._itemGenerator = itemGenerator;
        }

        public TItem GenerateItem(Point point, ForegroundBlock block)
        {
            return this._itemGenerator(point, block);
        }

        public Foreground.Id GetId(ForegroundBlock block)
        {
            return block.Id;
        }

        public ForegroundBlock GenerateBlock(Foreground.Id id)
        {
            try
            {
                return new ForegroundBlock(id);
            }
            catch (ArgumentException ex)
            {
                throw new NotSupportedException("There is no Block assosicated with this query.", ex);
            }
        }
    }
}