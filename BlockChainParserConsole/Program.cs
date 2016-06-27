using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockChain;

namespace BlockChainParserConsole
{
    class Program
    {
        const string blockPath = @"E:\Bitcoin\blocks";
        static void Main(string[] args)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var parser = new Parser(); 
            var blocks = parser.ParseHeader(blockPath);
            //var blocks = parser.Parse(blockPath);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;



            Console.WriteLine("Number of Blocks: " + blocks.Count() + " Time elapsed: " + elapsedMs/1000.0);

            Console.ReadLine();
        }
    }
}
