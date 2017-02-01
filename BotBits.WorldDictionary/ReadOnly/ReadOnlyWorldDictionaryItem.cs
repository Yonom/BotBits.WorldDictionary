using System;

namespace BotBits.WorldDictionary
{
    public struct ReadOnlyWorldDictionaryItem
    {
        private readonly ForegroundBlock? _foreground;
        private readonly BackgroundBlock? _background;

        public Layer Layer
        {
            get
            {
                if (this._foreground.HasValue)
                    return Layer.Foreground;
                if (this._background.HasValue)
                    return Layer.Background;
                throw new NotSupportedException("ReadOnlyWorldDictionaryItem is in an invalid state.");
            }
        }

        public int X { get; }
        public int Y { get; }

        public ForegroundBlock Foreground
        {
            get
            {
                if (!this._foreground.HasValue)
                    throw new NotSupportedException("Tried to access the wrong layer on ReadOnlyWorldDictionaryItem.");
                return this._foreground.Value;
            }
        }

        public BackgroundBlock Background
        {
            get
            {
                if (!this._background.HasValue)
                    throw new NotSupportedException("Tried to access the wrong layer on ReadOnlyWorldDictionaryItem.");
                return this._background.Value;
            }
        }

        public ReadOnlyWorldDictionaryItem(ForegroundBlock foreground, int x, int y)
        {
            this._foreground = foreground;
            this._background = null;
            this.X = x;
            this.Y = y;
        }

        public ReadOnlyWorldDictionaryItem(BackgroundBlock background, int x, int y)
        {
            this._foreground = null;
            this._background = background;
            this.X = x;
            this.Y = y;
        }
    }
}