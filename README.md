# BotBits.WorldDictionary
BotBits.WorldDictionary provides a world format optimized for block lookups.

## Setup

Download from NuGet (https://www.nuget.org/packages/BotBits.WorldDictionary)

## Choosing the right implementation

### BlocksDictionary

BlocksDictionary is a WorldDictionary representing the current world. It automatically updates whenever blocks are placed. It can be accessed through ```BlocksDictionary.Of(bot)```.

To enable BlocksDictionary call:

```csharp
BlocksDictionaryExtension.LoadInto(bot);
```

### ReadOnlyWorldDictionary

The simplest WorldDictionary is ReadOnlyWorldDictionary. As the name suggests, you can not change blocks after its creation. This means that there is less overhead during the indexing process and results in less CPU and memory usage (refer to the Performance section for more info). For this reason, ReadOnlyWorldDictionary is preferred over WorldDictionary if the world is static.

To convert a world / area to to a ReadOnlyWorldDictionary, call ```ToReadOnlyWorldDictionary()``` on it.

Note: ReadOnlyWorldDictionary is only a snapshot of the underlying World, it does not update when you update the original world. Consider using WorldDictionary if your world may change.

### WorldDictionary

Works just like ReadOnlyWorldDictionary, however it allows changes through ```Set```, ```Update``` and ```TryUpdate``` methods.

To convert a world / area to to a WorldDictionary, call ```ToWorldDictionary()``` on it.

Note: WorldDictionary is created through a snapshot of the underlying World, it does not automatically update when you update the original world, you will have to update WorldDictionary manually!

## Block queries

WorldDictionary supports two types of queries: by Id and by Block.

By Id: 
```csharp
// All spikes
var query = dictionary[Foreground.Hazard.Spike]; 
```

By Block:
```csharp
// All spikes facing up
var query = dictionary[new ForegroundBlock(Foreground.Hazard.Spike, Morph.Spike.Up)]; 
```

Each query has a ```Key``` containing either an ID or a Block depending on your query method.

### foreach

Similarly, you can iterate over all Ids in the foreground in the following way:

```csharp
foreach (var query in dictionary.Foreground.GroupedById) {
   Foreground.Id id = query.Key;
   // TODO: do something here
}
```

or differentiate between blocks that share the same id but have different settings:

```csharp
foreach (var query in dictionary.Foreground.GroupedByBlock) {
   ForegroundBlock block = query.Key;
   // TODO: do something here
}
```

In some cases, you might want to use ```GroupedByIdThenByBlock```:

```csharp
foreach (var nestedQuery in dictionary.Foreground.GroupedByIdThenByBlock) {
   Foreground.Id id = nestedQuery.Key;
   
   foreach (var query in nestedQuery) {
       ForegroundBlock id = query.Key;
       // TODO: do something here
   }
}
```

By default, iterating over ```dictionary.Foreground``` acts as a shorthand for ```dictionary.Foreground.GroupedById```. So the first foreach in this section could also be written as:
```csharp
foreach (var query in dictionary.Foreground) {
   Foreground.Id id = query.Key;
   // TODO: do something here
}
```

### Count

Serveral counting properties exist.

```csharp
// Number of different block ids used in this world
var idCount = dictionary.Foreground.Count;
// Number of different block configurations used in this world
var blockCount = dictionary.Foreground.GroupedByBlock.Count;
```

```csharp
// Number of empty background blocks
var emptyBackgroundCount = dictionary[Background.Empty].Count;
// Number of gold coin doors accepting 10 coins
var = dictionary.Foreground[new ForegroundBlock(Foreground.Coin.GoldDoor, 10)].Count;
```

```csharp
// Number of unique sign blocks in the world
var uniqueSignCount = dictionary.GetByBlock(Foreground.Sign.Block).QueryCount;
// Number of total sign blocks
var signCount = dictionary.GetByBlock(Foreground.Sign.Block).BlockCount;

var duplicateCount = signCount - uniqueSignCount;
Console.WriteLine($"There are {duplicateCount} duplicate signs in this world!");
```

### LINQ Support

All queries support LINQ.

```csharp
// All portals facing up
var query = dictionary[Foreground.Portal.Normal].Where(q => q.Key.Morph == Morph.Portal.Up); 
// All morphables
var query = dictionary.Foreground.GroupedByBlock.Where(q => q.Key.Type == ForegroundType.Morphable); 
```

#### Set

Calling ```Set```/```SetMany``` on queries from BlocksDictionary and WorldDictionary is supported. WorldDictionary can only Set blocks on the same layer as the query and will throw an exception if you try to access the other layer.

```csharp
// Paint empty spaces in the world with basic blue background
BlocksDictionary.Of(bot)[Foreground.Empty]
	.Where(i => i.Background.Id == Background.Empty)
	.Set(Background.Basic.Blue);

// Replace blue keys with red keys
dictionary[Foreground.Key.Blue].Set(Foreground.Key.Red);
// Clear world background
dictionary.Background.Where(id => id != Background.Empty).SetMany(Background.Empty);
```

## Performance

WorldDictionaries are extraordinary fast at block lookups. However they are slow at things that Worlds are fast at:

- Finding the block at a given location
- Placing lots of blocks

A WorldDictionary can take up to 10 times more memory compared to a World of the same size. Updating a block in a WorldDictionary has some extra overhead and will take up to 10x more time to complete. This means that creating and populating a WorldDictionary takes up to 10x more time than just looping through that world would.

ReadOnlyWorldDictionaries are around 20% faster than WorldDictionaries, with the tradeoff that .Contains and .At functions will become slower operations. These two operators are rarely needed on ReadOnlyWorldDictionaries.
The benefits of using ReadOnlyWorldDictionaries go beyond this however, as they allow for exclusion of unwanted block types through filters (see below) which can decrease the indexing time by up to 95%.

Once indexing is done, you will benefit from very fast lookups: It takes less than a microsecond to query for a wanted block while iterating through every block in the world usually takes dozens of milliseconds. 

The indexing process pays off after around 30-40 queries. However, it must be noted that even if you are going to use less than this amount of queries, WorldDictionary still can improve the responsiveness of your code and also increase code readablity. 

The differences between the two World formats look big on paper, however they only add up to milliseconds in total. WorldDictionary is still a very fast dictionary, so it is best to measure your code's performance before potentially wasting your time applying micro optimizations.


## Filters

ReadOnlyWorldDictionary has an additional feature called BlockFilters. Sometimes, you might only need to index certain blocks. By using a filter, you will reduce the world's indexing time and memory usage by only importing what you need. ```CompositeBlockFilter```s are a powerful tool for building filters that suit your needs.

```csharp
// Only index the foreground layer.
var onlyForeground = BlockFilter.OnlyForeground;
// Only index the foreground layer.
var onlyBackground = BlockFilter.OnlyBackground;
// A filter that accepts all blocks, using this filter is the same as applying no filter at all!
var allFilter = BlockFilter.All;
// Index nothing!
var none= BlockFilter.None;

// Index all blocks, but leave the whitespace
var allWithoutEmpty = BlockFilter.All.ExceptEmpty();

// Index the foreground layer, do not index any portals or empty space
var allWithoutEmpty = BlockFilter.Foreground
    .Except(Foreground.Portal.Normal, Foreground.Portal.Invisible)
    .ExceptEmpty();
```

You can enable a filter by using the additional ```ToReadOnlyWorldDictionary``` overload:
```csharp
var dictionary = world.ToReadOnlyWorldDictionary(filter);
```
When a filter is active, trying to access an unindexed block will result in an exception.

## Misc

### Writing blocks to WorldDictionary

You may use ```Update``` or ```Set``` to update a WorldDictionary:
```csharp
var dictionary = world.ToWorldDictionary();

dictionary.Update(0, 0, Foreground.Empty, Foreground.Basic.Gray);
// or alternatively
dictionary[Foreground.Empty].At(0, 0).Set(Foreground.Basic.Gray);
```

If you aren't sure if a block exists at a given position (eg. due to thread safety issues), use ```TryUpdate```:
```csharp
if (dictionary.TryUpdate(0, 0, Foreground.Empty, Foreground.Basic.Gray)) {
   // Placement successful
}
```

### ToWorld

You may want to convert a WorldDictionary back to a world by calling ```dictionary.ToWorld()```.
