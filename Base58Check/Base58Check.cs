using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Base58Check
{
    public class Base58Check
    {
        const string b58ConversionTable = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        public static string Encode(byte[] addrBytes)
        {
            var addrremain = addrBytes.Aggregate<byte, BigInteger>(0, (current, t) => current * 256 + t);
            var big0 = new BigInteger(0);
            var big58 = new BigInteger(58);

            string rv = "";

            while (addrremain.CompareTo(big0) > 0)
            {
                BigInteger remainder = addrremain % big58;
                int d = Convert.ToInt32(remainder.ToString());
                addrremain = addrremain / big58;
                rv = b58ConversionTable.Substring(d, 1) + rv;
            }

            // handle leading zeroes
            foreach (byte b in addrBytes)
            {
                if (b != 0) break;
                rv = "1" + rv;

            }
            return rv;
        }

        public static byte[] Decode(string base58)
        {
            var bi2 = new BigInteger(0);

            foreach (char c in base58)
            {
                if (b58ConversionTable.IndexOf(c) != -1)
                {
                    bi2 *= 58;
                    bi2 += b58ConversionTable.IndexOf(c);
                }
                else {
                    return null;
                }
            }

            byte[] bb = bi2.ToByteArray().Reverse().ToArray(); // Make it big endian

            // interpret leading '1's as leading zero bytes
            foreach (char c in base58)
            {
                if (c != '1') break;
                byte[] bbb = new byte[bb.Length + 1];
                Array.Copy(bb, 0, bbb, 1, bb.Length);
                bb = bbb;
            }

            return bb;
        }
    }
}
