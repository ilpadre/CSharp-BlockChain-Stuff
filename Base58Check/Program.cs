using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base58Check
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //byte[] AddressBytes = new byte[] { 0x00, 0x01, 0x09, 0x66, 0x77, 0x60, 0x06, 0x95, 0x3D, 0x55, 0x67, 0x43, 0x9E, 0x5E, 0x39, 0xF8, 0x6A, 0x0D, 0x27, 0x3B, 0xEE };
            byte[] AddressBytes = new byte[] { 0x00, 0x01, 0x09, 0x66, 0x77, 0x60, 0x06, 0x95, 0x3D, 0x55, 0x67, 0x43, 0x9E, 0x5E, 0x39, 0xF8, 0x6A, 0x0D, 0x27, 0x3B, 0xEE, 0xD6, 0x19, 0x67, 0xF6};


            var b58EncodedString = Base58Check.Encode(AddressBytes);

            var b58DecodedString = Base58Check.Decode(b58EncodedString);

            Console.WriteLine(Util.ByteArrayToString(AddressBytes));

            Console.WriteLine(b58EncodedString);

            Console.WriteLine(Util.ByteArrayToString(b58DecodedString));


            Console.ReadLine();


        }
    }
}
