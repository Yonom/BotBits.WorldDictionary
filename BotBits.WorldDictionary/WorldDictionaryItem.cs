using System;

namespace BotBits.WorldDictionary
{
    public struct WorldDictionaryItem : IBlockSettable<ForegroundBlock, BackgroundBlock>
    {
        private readonly IWorldDictionary<WorldDictionaryItem> _worldDictionary;
        private readonly ForegroundBlock? _foreground;
        private readonly BackgroundBlock? _background;

        public Layer Layer
        {
            get
            {
                if (this._foreground.HasValue) return Layer.Foreground;
                if (this._background.HasValue) return Layer.Background;
                throw new NotSupportedException("WorldDictionaryItem is in an invalid state.");
            }
        }

        public int X { get; }
        public int Y { get; }

        public ForegroundBlock Foreground
        {
            get
            {
                if (!this._foreground.HasValue) throw new NotSupportedException("Tried to access the wrong layer on WorldDictionaryItem.");
                return this._foreground.Value;
            }
            set
            {
                if (this.Layer != Layer.Foreground) throw new NotSupportedException("Tried to access the wrong layer on WorldDictionaryItem.");
                this._worldDictionary.Update(new Point(this.X, this.Y), this.Foreground, value);
            }
        }

        public BackgroundBlock Background
        {
            get
            {
                if (!this._background.HasValue) throw new NotSupportedException("Tried to access the wrong layer on WorldDictionaryItem.");
                return this._background.Value;
            }
            set
            {
                if (this.Layer != Layer.Background) throw new NotSupportedException("Tried to access the wrong layer on WorldDictionaryItem.");
                this._worldDictionary.Update(new Point(this.X, this.Y), this.Background, value);
            }
        }

        public WorldDictionaryItem(IWorldDictionary<WorldDictionaryItem> worldDictionary, ForegroundBlock foreground, int x, int y)
        {
            this._worldDictionary = worldDictionary;
            this._foreground = foreground;
            this._background = null;
            this.X = x;
            this.Y = y;
        }

        public WorldDictionaryItem(IWorldDictionary<WorldDictionaryItem> worldDictionary, BackgroundBlock background, int x, int y)
        {
            this._worldDictionary = worldDictionary;
            this._foreground = null;
            this._background = background;
            this.X = x;
            this.Y = y;
        }

        public void Set(ForegroundBlock block)
        {
            this.Foreground = block;
        }

        public void Set(BackgroundBlock block)
        {
            this.Background = block;
        }
    }
}