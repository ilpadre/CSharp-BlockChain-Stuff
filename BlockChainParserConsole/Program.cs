using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using BlockChain;

namespace BlockChainParserConsole
{
    public class Program
    {
        //const string blockPath = @"C:\Users\ken.baum\Documents\Personal\Projects\Elixir-Blockchain-Parser\blockchain_parser\blocks";
        const string blockPath = @"C:\Users\ken.baum\Documents\BitCoin\blocks";
        static void Main(string[] args)
        {
            List<Block> blocks = null;
            var parser = new Parser();
            var keepGoing = true;
            while (keepGoing)
            {
                DisplayMenu();
                var userChoice = Console.ReadLine();
                switch (userChoice)
                {
                    case "1":
                        blocks = parser.ParseHeader(blockPath);
                        break;
                    case "2":
                        blocks = parser.Parse(blockPath);
                        break;
                    case "3":
                        parser.SaveFirstBlock(blockPath);
                        break;
                    case "4":
                        keepGoing = false;
                        break;
                    default:
                        break;
                }
            }

        }

        private static void DisplayMenu()
        {
            Console.WriteLine("**********************************************");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Parse block headers only");
            Console.WriteLine("2. Parse blocks");
            Console.WriteLine("3. Save first block");
            Console.WriteLine("4. Exit program");
            Console.WriteLine("**********************************************");
        }

        private static void ProcessMenuSelection(string entry)
        {
            List<Block> blocks = null;
            var parser = new Parser();
            switch (entry)
            {
                case "1":
                    blocks = parser.ParseHeader(blockPath);
                    break;
                case "2":
                    blocks = parser.Parse(blockPath);
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}
