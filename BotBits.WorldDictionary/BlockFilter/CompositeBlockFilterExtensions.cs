using System.Linq;

namespace BotBits.WorldDictionary
{
    public static class CompositeBlockFilterExtensions
    {
        public static CompositeBlockFilter Exclude(this CompositeBlockFilter filter, params Foreground.Id[] ids)
        {
            return new CompositeBlockFilter(filter, (id, p) => !ids.Contains(id), (id, p) => true);
        }

        public static CompositeBlockFilter Exclude(this CompositeBlockFilter filter, params Background.Id[] ids)
        {
            return new CompositeBlockFilter(filter, (id, p) => true, (id, p) => !ids.Contains(id));
        }

        public static CompositeBlockFilter ExcludeEmpty(this CompositeBlockFilter filter)
        {
            return new CompositeBlockFilter(filter, (id, p) => id != Foreground.Empty, (id, p) => id != Background.Empty);
        }
    }
}