﻿using System;

namespace BotBits.WorldDictionary
{
    public sealed class BlocksDictionaryExtension : Extension<BlocksDictionaryExtension>
    {
        [Obsolete("Invalid to use \"new\" on this class. Use the static LoadInto method instead.", true)]
        public BlocksDictionaryExtension()
        {
        }

        public static bool LoadInto(BotBitsClient client)
        {
            return LoadInto(client, null);
        }
    }
}