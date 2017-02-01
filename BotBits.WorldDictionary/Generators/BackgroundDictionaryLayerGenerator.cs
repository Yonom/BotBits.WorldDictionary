using System;

namespace BotBits.WorldDictionary
{
    internal class BackgroundDictionaryLayerGenerator<TItem> : IDictionaryLayerGenerator<Background.Id, BackgroundBlock, TItem>
        where TItem : struct
    {
        private readonly Func<Point, BackgroundBlock, TItem> _itemGenerator;

        public BackgroundDictionaryLayerGenerator(Func<Point, BackgroundBlock, TItem> itemGenerator)
        {
            this._itemGenerator = itemGenerator;
        }

        public TItem GenerateItem(Point point, BackgroundBlock block)
        {
            return this._itemGenerator(point, block);
        }

        public Background.Id GetId(BackgroundBlock block)
        {
            return block.Id;
        }

        public BackgroundBlock GenerateBlock(Background.Id id)
        {
            try
            {
                return new BackgroundBlock(id);
            }
            catch (ArgumentException ex)
            {
                throw new NotSupportedException("There is no Block assosicated with this query.", ex);
            }
        }
    }
}