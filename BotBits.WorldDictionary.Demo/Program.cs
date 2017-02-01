using System;
using System.Diagnostics;

namespace BotBits.WorldDictionary.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bot = new BotBitsClient();

            // Dictionary
            BlocksDictionaryExtension.LoadInto(bot);

            // Events
            EventLoader
                .Of(bot)
                .LoadStatic<Program>();

            // Login
            Login.Of(bot)
                .AsGuest()
                .CreateJoinRoom("PW01");

            Console.WriteLine($"This world contains {BlocksDictionary.Of(bot)[Foreground.Coin.GoldDoor].Count} gold coin doors.");
        }
    }
}