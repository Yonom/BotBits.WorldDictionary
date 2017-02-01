using System;

namespace BotBits.WorldDictionary
{
    public interface IBlockFilter<in TId, TBlock> where TId : struct where TBlock : struct
    {
        bool ShouldIndex(TId id, TBlock? block);
    }

    public interface IBlockFilter : IBlockFilter<Foreground.Id, ForegroundBlock>, IBlockFilter<Background.Id, BackgroundBlock>
    {
    }


    internal class DefaultBlockFilter : IBlockFilter
    {
        public static DefaultBlockFilter Value { get; } = new DefaultBlockFilter();

        private DefaultBlockFilter()
        {
            
        }

        public bool ShouldIndex(Foreground.Id id, ForegroundBlock? block)
        {
            return true;
        }

        public bool ShouldIndex(Background.Id id, BackgroundBlock? block)
        {
            return true;
        }
    }

    public class WorldDictionary : IWorldDictionary<WorldDictionaryItem>
    {
        public WorldDictionary(IReadOnlyWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea, IBlockFilter filter)
        {
            if (worldArea.Area.Width > ushort.MaxValue || worldArea.Area.Height > ushort.MaxValue)
                throw new NotSupportedException($"WorldDictionary only supports worlds that are {ushort.MaxValue} wide or tall at maximum.");
            var fgGen = new ForegroundDictionaryLayerGenerator<WorldDictionaryItem>((p, b) => new WorldDictionaryItem(this, b, p.X, p.Y));
            var bgGen = new BackgroundDictionaryLayerGenerator<WorldDictionaryItem>((p, b) => new WorldDictionaryItem(this, b, p.X, p.Y));
            this.InternalForeground = new DictionaryBlockLayer<Foreground.Id, ForegroundBlock, WorldDictionaryItem>(fgGen, filter);
            this.InternalBackground = new DictionaryBlockLayer<Background.Id, BackgroundBlock, WorldDictionaryItem>(bgGen, filter);

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

        public WorldDictionary(IWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea, IBlockFilter filter)
            : this(worldArea.ToReadOnlyWorldAreaEnumerable(), filter)
        {
        }

        public WorldDictionary(IReadOnlyWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea)
            : this(worldArea, null)
        {
        }

        public WorldDictionary(IWorldAreaEnumerable<ForegroundBlock, BackgroundBlock> worldArea) 
            : this(worldArea, null)
        {
        }

        public int Width { get; }
        public int Height { get; }
         
        private DictionaryBlockLayer<Foreground.Id, ForegroundBlock, WorldDictionaryItem> InternalForeground { get; }
        private DictionaryBlockLayer<Background.Id, BackgroundBlock, WorldDictionaryItem> InternalBackground { get; }
        public IDictionaryBlockLayer<Foreground.Id, ForegroundBlock, WorldDictionaryItem> Foreground => this.InternalForeground;
        public IDictionaryBlockLayer<Background.Id, BackgroundBlock, WorldDictionaryItem> Background => this.InternalBackground;

        public IBlockQuery<ForegroundBlock, WorldDictionaryItem> this[ForegroundBlock block] => this.Foreground[block];
        public IBlockQuery<Foreground.Id, WorldDictionaryItem> this[Foreground.Id id] => this.Foreground[id];
        public IBlockQuery<BackgroundBlock, WorldDictionaryItem> this[BackgroundBlock block] => this.Background[block];
        public IBlockQuery<Background.Id, WorldDictionaryItem> this[Background.Id id] => this.Background[id];

        public void Update(Point point, ForegroundBlock oldBlock, ForegroundBlock newBlock)
        {
            if (!this.TryUpdate(point, oldBlock, newBlock))
            {
                throw new InvalidOperationException("Unable to find the given oldBlock at the given location.");
            }
        }

        public void Update(Point point, BackgroundBlock oldBlock, BackgroundBlock newBlock)
        {
            if (!this.TryUpdate(point, oldBlock, newBlock))
            {
                throw new InvalidOperationException("Unable to find the given oldBlock at the given location.");
            }
        }

        public bool TryUpdate(Point point, ForegroundBlock oldBlock, ForegroundBlock newBlock)
        {
            if (!this.InternalForeground.Remove(oldBlock, point)) return false;

            this.InternalForeground.Add(newBlock, point);
            return true;
        }

        public bool TryUpdate(Point point, BackgroundBlock oldBlock, BackgroundBlock newBlock)
        {
            if (!this.InternalBackground.Remove(oldBlock, point))
                return false;

            this.InternalBackground.Add(newBlock, point);
            return true;
        }
    }
}
