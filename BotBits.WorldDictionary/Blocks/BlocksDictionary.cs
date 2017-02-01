using System;

namespace BotBits.WorldDictionary
{
    public sealed class BlocksDictionary : Package<BlocksDictionary>, IReadOnlyWorldDictionary<BlocksItem>
    {
        private BlocksAreaDictionary _blocksAreaDictionary;

        [Obsolete("Invalid to use \"new\" on this class. Use the static .Of(BotBits) method instead.", true)]
        public BlocksDictionary()
        {
            this.InitializeFinish += this.BlocksDictionary_InitializeFinish;
        }

        public int Width => this._blocksAreaDictionary.Width;
        public int Height => this._blocksAreaDictionary.Height;

        public IBlockQuery<ForegroundBlock, BlocksItem> this[ForegroundBlock block] => this.Foreground[block];
        public IBlockQuery<Foreground.Id, BlocksItem> this[Foreground.Id id] => this.Foreground[id];
        public IBlockQuery<BackgroundBlock, BlocksItem> this[BackgroundBlock block] => this.Background[block];
        public IBlockQuery<Background.Id, BlocksItem> this[Background.Id id] => this.Background[id];

        public IDictionaryBlockLayer<Foreground.Id, ForegroundBlock, BlocksItem> Foreground => this._blocksAreaDictionary.Foreground;
        public IDictionaryBlockLayer<Background.Id, BackgroundBlock, BlocksItem> Background => this._blocksAreaDictionary.Background;

        private void BlocksDictionary_InitializeFinish(object sender, EventArgs e)
        {
            this._blocksAreaDictionary = new BlocksAreaDictionary(Blocks.Of(this.BotBits), this.BotBits);
        }
    }
}