using System;

namespace BotBits.WorldDictionary
{
    public class CompositeBlockFilter : IBlockFilter
    {

        private readonly IBlockFilter _innerFilter;
        private readonly Func<Foreground.Id, ForegroundBlock?, bool> _foregroundPredicate;
        private readonly Func<Background.Id, BackgroundBlock?, bool> _backgroundPredicate;

        public CompositeBlockFilter(IBlockFilter innerFilter, Func<Foreground.Id, ForegroundBlock?, bool> foregroundPredicate, Func<Background.Id, BackgroundBlock?, bool> backgroundPredicate)
        {
            this._innerFilter = innerFilter;
            this._foregroundPredicate = foregroundPredicate;
            this._backgroundPredicate = backgroundPredicate;
        }

        public CompositeBlockFilter(Func<Foreground.Id, ForegroundBlock?, bool> foregroundPredicate, Func<Background.Id, BackgroundBlock?, bool> backgroundPredicate)
            :this(null, foregroundPredicate, backgroundPredicate)
        {
        }

        public bool ShouldIndex(Foreground.Id id, ForegroundBlock? block)
        {
            return this._innerFilter?.ShouldIndex(id, block) != false && 
                this._foregroundPredicate(id, block);
        }

        public bool ShouldIndex(Background.Id id, BackgroundBlock? block)
        {
            return this._innerFilter?.ShouldIndex(id, block) != false &&
                this._backgroundPredicate(id, block);
        }
    }
}
