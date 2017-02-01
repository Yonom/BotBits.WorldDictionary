namespace BotBits.WorldDictionary
{
    internal interface IDictionaryLayerGenerator<out TId, in TBlock, out TItem>
        where TId : struct
        where TBlock : struct
        where TItem : struct
    {
        TItem GenerateItem(Point point, TBlock block);
        TId GetId(TBlock block);
    }
}