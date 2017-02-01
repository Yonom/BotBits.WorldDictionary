using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BotBits.Events;

namespace BotBits.WorldDictionary
{
    public class BlocksAreaDictionary : IReadOnlyWorldDictionary<BlocksItem>, IDisposable
    {
        private readonly BotBitsClient _client;
        private readonly IBlockAreaEnumerable _blockArea;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public BlocksAreaDictionary(IBlockAreaEnumerable blockArea, BotBitsClient client, IBlockFilter filter)
        {
            this._client = client;
            this._blockArea = blockArea;

            var fgGen = new ForegroundDictionaryLayerGenerator<BlocksItem>((p, b) => this._blockArea.At(p.X, p.Y));
            var bgGen = new BackgroundDictionaryLayerGenerator<BlocksItem>((p, b) => this._blockArea.At(p.X, p.Y));
            this.InternalForeground = new DictionaryBlockLayer<Foreground.Id, ForegroundBlock, BlocksItem>(fgGen, filter);
            this.InternalBackground = new DictionaryBlockLayer<Background.Id, BackgroundBlock, BlocksItem>(bgGen, filter);

            this.Reindex();
            
            EventLoader.Of(this._client).Load(this);
        }

        public BlocksAreaDictionary(IBlockAreaEnumerable blockArea, BotBitsClient client) : this(blockArea, client, DefaultBlockFilter.Value)
        {
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private DictionaryBlockLayer<Foreground.Id, ForegroundBlock, BlocksItem> InternalForeground { get; }
        private DictionaryBlockLayer<Background.Id, BackgroundBlock, BlocksItem> InternalBackground { get; }
        public IDictionaryBlockLayer<Foreground.Id, ForegroundBlock, BlocksItem> Foreground => this.InternalForeground;
        public IDictionaryBlockLayer<Background.Id, BackgroundBlock, BlocksItem> Background => this.InternalBackground;

        public IBlockQuery<ForegroundBlock, BlocksItem> this[ForegroundBlock block] => this.Foreground[block];
        public IBlockQuery<Foreground.Id, BlocksItem> this[Foreground.Id id] => this.Foreground[id];
        public IBlockQuery<BackgroundBlock, BlocksItem> this[BackgroundBlock block] => this.Background[block];
        public IBlockQuery<Background.Id, BlocksItem> this[Background.Id id] => this.Background[id];

        private void Reindex()
        {
            this._lock.EnterWriteLock();
            
            this.InternalForeground.Clear();
            this.InternalBackground.Clear();

            this.Width = this._blockArea.Area.Width;
            this.Height = this._blockArea.Area.Height;

            for (var y = 0; y < this.Height; y++)
            {
                for (var x = 0; x < this.Width; x++)
                {
                    this.InternalForeground.Add(this._blockArea.At(x, y).Foreground.Block, new Point(x, y));
                    this.InternalBackground.Add(this._blockArea.At(x, y).Background.Block, new Point(x, y));
                }
            }

            this._lock.ExitWriteLock();
        }

        [EventListener]
        private void On(InitEvent e)
        {
            this.Reindex();
        }

        [EventListener]
        private void On(LoadLevelEvent e)
        {
            this.Reindex();
        }

        [EventListener]
        private void On(ClearEvent e)
        {
            this.Reindex();
        }

        [EventListener]
        private void On(ForegroundPlaceEvent e)
        {
            this._lock.EnterReadLock();

            if (!this.InternalForeground.Remove(e.Old.Block, new Point(e.X, e.Y))) return; // Unknown block!
            this.InternalForeground.Add(e.New.Block, new Point(e.X, e.Y));

            this._lock.ExitReadLock();
        }

        [EventListener]
        private void On(BackgroundPlaceEvent e)
        {
            this._lock.EnterReadLock();
            
            if (!this.InternalBackground.Remove(e.Old.Block, new Point(e.X, e.Y))) return; // Unknown block!
            this.InternalBackground.Add(e.New.Block, new Point(e.X, e.Y));

            this._lock.ExitReadLock();
        }

        public void Dispose()
        {
            EventLoader.Of(this._client).Unload(this);
        }
    }
}
