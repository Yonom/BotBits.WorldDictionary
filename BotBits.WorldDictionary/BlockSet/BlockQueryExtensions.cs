namespace BotBits.WorldDictionary
{
    public static class BlockQueryExtensions
    {
        public static TItem At<TKey, TItem>(this IBlockQuery<TKey, TItem> query, int x, int y) where TKey : struct where TItem : struct
        {
            return query.At(new Point(x, y));
        }

        public static bool Contains<TKey, TItem>(this IBlockQuery<TKey, TItem> query, int x, int y) where TKey : struct where TItem : struct
        {
            return query.Contains(new Point(x, y));
        }
    }
}