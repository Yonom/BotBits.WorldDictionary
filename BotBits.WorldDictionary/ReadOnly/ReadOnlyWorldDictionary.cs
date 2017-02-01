using System;

namespace BotBits.WorldDictionary
{
    public class ReadOnlyWorldDictionary : IReadOnlyWorldDictionary<ReadOnlyWorldDictionaryItem>
    {
        public ReadOnlyWorldDictionary(IReadOnlyWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea, IBlockFilter filter)
        {
            if (worldArea.Area.Width > ushort.MaxValue || worldArea.Area.Height > ushort.MaxValue)
                throw new NotSupportedException($"WorldDictionary only supports worlds that are {ushort.MaxValue} wide or tall at maximum.");
            var fgGen = new ForegroundDictionaryLayerGenerator<ReadOnlyWorldDictionaryItem>((p, b) => new ReadOnlyWorldDictionaryItem(b, p.X, p.Y));
            var bgGen = new BackgroundDictionaryLayerGenerator<ReadOnlyWorldDictionaryItem>((p, b) => new ReadOnlyWorldDictionaryItem(b, p.X, p.Y));
            this.InternalForeground = new DictionaryBlockLayer<Foreground.Id, ForegroundBlock, ReadOnlyWorldDictionaryItem, ReadOnlyPointSet>(fgGen, filter);
            this.InternalBackground = new DictionaryBlockLayer<Background.Id, BackgroundBlock, ReadOnlyWorldDictionaryItem, ReadOnlyPointSet>(bgGen, filter);

            this.Width = worldArea.Area.Width;
            this.Height = worldArea.Area.Height;

            for (var y = 0; y < this.Height; y++)
            {
                for (var x = 0; x < this.Width; x++)
                {
                    this.InternalForeground.Add(worldArea.At(x, y).Foreground, new Point(x, y));
                    this.InternalBackground.Add(worldArea.At(x, y).Background, new Point(x, y));
                }
            }
        }

        public ReadOnlyWorldDictionary(IWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea, IBlockFilter filter)
            : this(worldArea.ToReadOnlyWorldAreaEnumerable(), filter)
        {
        }

        public ReadOnlyWorldDictionary(IReadOnlyWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea)
            : this(worldArea, DefaultBlockFilter.Value)
        {
        }

        public ReadOnlyWorldDictionary(IWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea)
            : this(worldArea, DefaultBlockFilter.Value)
        {
        }

        public int Width { get; }
        public int Height { get; }

        private DictionaryBlockLayer<Foreground.Id, ForegroundBlock, ReadOnlyWorldDictionaryItem, ReadOnlyPointSet> InternalForeground { get; }
        private DictionaryBlockLayer<Background.Id, BackgroundBlock, ReadOnlyWorldDictionaryItem, ReadOnlyPointSet> InternalBackground { get; }
        public IDictionaryBlockLayer<Foreground.Id, ForegroundBlock, ReadOnlyWorldDictionaryItem> Foreground => this.InternalForeground;
        public IDictionaryBlockLayer<Background.Id, BackgroundBlock, ReadOnlyWorldDictionaryItem> Background => this.InternalBackground;

        public IBlockQuery<ForegroundBlock, ReadOnlyWorldDictionaryItem> this[ForegroundBlock block] => this.Foreground[block];
        public IBlockQuery<Foreground.Id, ReadOnlyWorldDictionaryItem> this[Foreground.Id id] => this.Foreground[id];
        public IBlockQuery<BackgroundBlock, ReadOnlyWorldDictionaryItem> this[BackgroundBlock block] => this.Background[block];
        public IBlockQuery<Background.Id, ReadOnlyWorldDictionaryItem> this[Background.Id id] => this.Background[id];
    }
}