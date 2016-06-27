using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProofOfWork
{
    public class Program
    {
        const string asterisks = "********************************************************";
        public static string FilePath { get; set; }
        public static void Main(string[] args)
        {
            FilePath = "log.txt";
            File.Delete(FilePath);

            string passPhrase = string.Empty;

            while (string.IsNullOrEmpty(passPhrase))
            {
                Console.Write("Enter a pass phrase: ");
                passPhrase = Console.ReadLine();
            }
            int zeros = 0;
            while (true)
            {
                Console.WriteLine();
                var hash = (ProofOfWork(passPhrase, ++zeros));
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        private static string ProofOfWork(string passPhrase, int zeros)
        {
            var hashVal = string.Empty;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (ulong l = 1; l < ulong.MaxValue; l++)
            {
                var txt = passPhrase + l.ToString();
                var bytes = Encoding.UTF8.GetBytes(txt);
                var hashString = new SHA256Managed();
                var hash = hashString.ComputeHash(bytes);
                foreach (var x in hash)
                {
                    hashVal += String.Format("{0:x2}", x);
                }
                //Console.WriteLine(hashVal);
                if (isTheOne(hashVal, zeros))
                {
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    var msg = string.Format("Leading Zeros: {0}; Nonce: {1}; Input Phrase: {2}; Elapsed time: {3} seconds; hash value = {4}", zeros, l, txt, elapsedMs/60, hashVal);
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        sw.WriteLine(msg);
                    }
                    Console.WriteLine(asterisks);
                    Console.WriteLine(msg);
                    Console.WriteLine(asterisks);
                    break;
                }

                hashVal = string.Empty;
            }
            return hashVal;
        }

        private static bool isTheOne(string hash, int zeros)
        {
            var leadingZeros = 0;
            for (int i = 0; i < hash.Length; i++)
            {
                if (hash.ElementAt(i) == '0')
                    leadingZeros++;
                else
                    break;
            }
            if (leadingZeros >= zeros)
                return true;
            else
                return false;

        }
    }
}
