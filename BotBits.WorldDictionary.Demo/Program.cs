using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BotBits.WorldDictionary.Demo
{
    class Program
    {
        private static void Main(string[] args)
        {
            //var bot = new BotBitsClient();

            //// Dictionary
            //BlocksDictionaryExtension.LoadInto(bot);

            //// Events
            //EventLoader
            //    .Of(bot)
            //    .LoadStatic<Program>();

            //// Login
            //Login.Of(bot)
            //    .AsGuest()
            //    .CreateJoinRoom("PW01");

            var world = new World(1000, 1000);
            var dic = world.ToWorldDictionary();

            var sw = Stopwatch.StartNew();
            world.Set(Foreground.Basic.Blue);
            sw.Stop();

            var sws = Stopwatch.StartNew();
            dic.Foreground.GroupedByBlock.SetMany(Foreground.Basic.Blue);
            sws.Stop();

            Console.WriteLine(dic.Foreground[Foreground.Empty].Count);
        }
    }
}
