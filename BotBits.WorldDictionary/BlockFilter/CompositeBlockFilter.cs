using System;

namespace BotBits.WorldDictionary
{
    public class CompositeBlockFilter : IBlockFilter
    {
        private readonly CompositeBlockFilter _innerFilter;
        private readonly IBlockFilter _newFilter;

        public CompositeBlockFilter(CompositeBlockFilter innerFilter, IBlockFilter newFilter)
        {
            this._innerFilter = innerFilter;
            this._newFilter = newFilter;
        }

        public CompositeBlockFilter(IBlockFilter filter)
            :this(null, filter)
        {
        }

        public bool ShouldIndex(Foreground.Id id, ForegroundBlock? block)
        {
            return this._innerFilter?.ShouldIndex(id, block) != false && 
                this._newFilter.ShouldIndex(id, block);
        }

        public bool ShouldIndex(Background.Id id, BackgroundBlock? block)
        {
            return this._innerFilter?.ShouldIndex(id, block) != false &&
                this._newFilter.ShouldIndex(id, block);
        }
    }
}
