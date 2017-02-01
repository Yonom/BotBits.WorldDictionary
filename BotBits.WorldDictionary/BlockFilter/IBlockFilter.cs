namespace BotBits.WorldDictionary
{
    public interface IBlockFilter<in TId, TBlock> where TId : struct where TBlock : struct
    {
        bool ShouldIndex(TId id, TBlock? block);
    }
}