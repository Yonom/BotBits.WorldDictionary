using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading;

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
            

            var dic = new World(1000, 1000).ToWorldDictionary();
            Test(dic);
        }

        public static void Test(WorldDictionary dic)
        {
            var sws = Stopwatch.StartNew();
            for (var i = 0; i < 10; i++)
            dic.Foreground.GroupedByBlock.SetMany(Foreground.Basic.Blue);
            sws.Stop();

            Console.WriteLine(dic.Foreground[new ForegroundBlock(Foreground.Empty)].Count);
        }
    }
}
