using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Base58Check
{
    public static class Util
    {
        public static bool IsSpace(char c)
        {
            switch (c)
            {
                case ' ':
                case '\t':
                case '\n':
                case '\v':
                case '\f':
                case '\r':
                    return true;
            }
            return false;
        }

        public static byte[] ComputeSha256(string ofwhat)
        {
            UTF8Encoding utf8 = new UTF8Encoding(false);
            return ComputeSha256(utf8.GetBytes(ofwhat));
        }


        public static byte[] ComputeSha256(byte[] ofwhat)
        {
            var sha256 = new SHA256Managed();
            byte[] rv = sha256.ComputeHash(ofwhat);
            return rv;
        }

        public static byte[] ComputeDoubleSha256(string ofwhat)
        {
            UTF8Encoding utf8 = new UTF8Encoding(false);
            return ComputeDoubleSha256(utf8.GetBytes(ofwhat));
        }

        public static byte[] ComputeDoubleSha256(byte[] ofwhat)
        {
            var sha256 = new SHA256Managed();
            byte[] rv1 = sha256.ComputeHash(ofwhat);
            byte[] rv2 = sha256.ComputeHash(rv1);
            return rv2;
        }

        //public static string ByteArrayToBase58Check(byte[] ba)
        //{

        //    byte[] bb = new byte[ba.Length + 4];
        //    Array.Copy(ba, bb, ba.Length);
        //    var bcsha256a = new SHA256Managed();           
        //    byte[] thehash1 = bcsha256a.ComputeHash(ba);
        //    byte[] thehash2 = bcsha256a.ComputeHash(thehash1);
        //    for (int i = 0; i < 4; i++) bb[ba.Length + i] = thehash2[i];
        //    return Utils.FromByteArray(bb);
        //}


        //public static byte[] Base58CheckToByteArray(string base58)
        //{

        //    bool IgnoreChecksum = false;
        //    if (base58.EndsWith("?"))
        //    {
        //        IgnoreChecksum = true;
        //        base58 = base58.Substring(0, base58.Length - 1);
        //    }

        //    byte[] bb = Base58.ToByteArray(base58);
        //    if (bb == null || bb.Length < 4) return null;

        //    if (IgnoreChecksum == false)
        //    {
        //        Sha256Digest bcsha256a = new Sha256Digest();
        //        bcsha256a.BlockUpdate(bb, 0, bb.Length - 4);

        //        byte[] checksum = new byte[32];  //sha256.ComputeHash(bb, 0, bb.Length - 4);
        //        bcsha256a.DoFinal(checksum, 0);
        //        bcsha256a.BlockUpdate(checksum, 0, 32);
        //        bcsha256a.DoFinal(checksum, 0);

        //        for (int i = 0; i < 4; i++)
        //        {
        //            if (checksum[i] != bb[bb.Length - 4 + i]) return null;
        //        }
        //    }

        //    byte[] rv = new byte[bb.Length - 4];
        //    Array.Copy(bb, 0, rv, 0, bb.Length - 4);
        //    return rv;
        //}

        public static string ByteArrayToString(byte[] ba)
        {
            return ByteArrayToString(ba, 0, ba.Length);
        }

        public static string ByteArrayToString(byte[] ba, int offset, int count)
        {
            string rv = "";
            int usedcount = 0;
            for (int i = offset; usedcount < count; i++, usedcount++)
            {
                rv += String.Format("{0:X2}", ba[i]) + " ";
            }
            return rv;
        }

        public static byte[] HexStringToBytes(string source, bool testingForValidHex = false)
        {
            List<byte> bytes = new List<byte>();
            bool gotFirstChar = false;
            byte accum = 0;

            foreach (char c in source.ToCharArray())
            {
                if (c == ' ' || c == '-' || c == ':')
                {
                    // if we got a space, then accept it as the end if we have 1 character.
                    if (gotFirstChar)
                    {
                        bytes.Add(accum);
                        accum = 0;
                        gotFirstChar = false;
                    }
                }
                else if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))
                {
                    // get the character's value
                    byte v = (byte)(c - 0x30);
                    if (c >= 'A' && c <= 'F') v = (byte)(c + 0x0a - 'A');
                    if (c >= 'a' && c <= 'f') v = (byte)(c + 0x0a - 'a');

                    if (gotFirstChar == false)
                    {
                        gotFirstChar = true;
                        accum = v;
                    }
                    else {
                        accum <<= 4;
                        accum += v;
                        bytes.Add(accum);
                        accum = 0;
                        gotFirstChar = false;
                    }
                }
                else {
                    if (testingForValidHex) return null;
                }
            }
            if (gotFirstChar) bytes.Add(accum);
            return bytes.ToArray();
        }

        public static byte[] GetHexBytes(string source, int minimum)
        {
            byte[] hex = HexStringToBytes(source);
            if (hex == null) return null;
            // assume leading zeroes if we're short a few bytes
            if (hex.Length > (minimum - 6) && hex.Length < minimum)
            {
                byte[] hex2 = new byte[minimum];
                Array.Copy(hex, 0, hex2, minimum - hex.Length, hex.Length);
                hex = hex2;
            }
            // clip off one overhanging leading zero if present
            if (hex.Length == minimum + 1 && hex[0] == 0)
            {
                byte[] hex2 = new byte[minimum];
                Array.Copy(hex, 1, hex2, 0, minimum);
                hex = hex2;

            }

            return hex;
        }

        public static bool ArrayEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            return ArrayEqual(a, 0, b, 0, Math.Max(a.Length, b.Length));
        }

        public static bool ArrayEqual(byte[] a, int startA, byte[] b, int startB, int length)

        {
            if (a == null && b == null)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            var alen = a.Length - startA;
            var blen = b.Length - startB;

            if (alen < length || blen < length)
                return false;

            for (int ai = startA, bi = startB; ai < startA + length; ai++, bi++)
            {
                if (a[ai] != b[bi])
                    return false;
            }
            return true;
        }

        internal static byte[] BigIntegerToBytes(BigInteger num)
        {
            if (num == 0)
                //Positive 0 is represented by a null-length vector
                return new byte[0];

            bool isPositive = true;
            if (num < 0)
            {
                isPositive = false;
                num *= -1;
            }
            var array = num.ToByteArray();
            if (!isPositive)
                array[array.Length - 1] |= 0x80;
            return array;
        }

        internal static BigInteger BytesToBigInteger(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                return BigInteger.Zero;
            data = data.ToArray();
            var positive = (data[data.Length - 1] & 0x80) == 0;
            if (!positive)
            {
                data[data.Length - 1] &= unchecked((byte)~0x80);
                return new BigInteger(data);
            }
            return new BigInteger(data);
        }

        public static string ExceptionToString(Exception exception)
        {
            Exception ex = exception;
            StringBuilder stringBuilder = new StringBuilder(128);
            while (ex != null)
            {
                stringBuilder.Append(ex.GetType().Name);
                stringBuilder.Append(": ");
                stringBuilder.Append(ex.Message);
                stringBuilder.AppendLine(ex.StackTrace);
                ex = ex.InnerException;
                if (ex != null)
                {
                    stringBuilder.Append(" ---> ");
                }
            }
            return stringBuilder.ToString();
        }

        public static void Shuffle<T>(T[] arr, int seed)
        {
            Random rand = new Random(seed);
            for (int i = 0; i < arr.Length; i++)
            {
                var fromIndex = rand.Next(arr.Length);
                var from = arr[fromIndex];

                var toIndex = rand.Next(arr.Length);
                var to = arr[toIndex];

                arr[toIndex] = from;
                arr[fromIndex] = to;
            }
        }

        public static void Shuffle<T>(T[] arr)
        {
            Random rand = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                var fromIndex = rand.Next(arr.Length);
                var from = arr[fromIndex];

                var toIndex = rand.Next(arr.Length);
                var to = arr[toIndex];

                arr[toIndex] = from;
                arr[fromIndex] = to;
            }
        }

        public static byte[] ToBytes(uint value, bool littleEndian)
        {
            if (littleEndian)
            {
                return new byte[]
                {
                    (byte)value,
                    (byte)(value >> 8),
                    (byte)(value >> 16),
                    (byte)(value >> 24),
                };
            }
            else
            {
                return new byte[]
                {
                    (byte)(value >> 24),
                    (byte)(value >> 16),
                    (byte)(value >> 8),
                    (byte)value,
                };
            }
        }

        public static byte[] ToBytes(ulong value, bool littleEndian)
        {
            if (littleEndian)
            {
                return new byte[]
                {
                    (byte)value,
                    (byte)(value >> 8),
                    (byte)(value >> 16),
                    (byte)(value >> 24),
                    (byte)(value >> 32),
                    (byte)(value >> 40),
                    (byte)(value >> 48),
                    (byte)(value >> 56),
                };
            }
            else
            {
                return new byte[]
                {
                    (byte)(value >> 56),
                    (byte)(value >> 48),
                    (byte)(value >> 40),
                    (byte)(value >> 32),
                    (byte)(value >> 24),
                    (byte)(value >> 16),
                    (byte)(value >> 8),
                    (byte)value,
                };
            }
        }

        public static uint ToUInt32(byte[] value, int index, bool littleEndian)
        {
            if (littleEndian)
            {
                return value[index]
                       + ((uint)value[index + 1] << 8)
                       + ((uint)value[index + 2] << 16)
                       + ((uint)value[index + 3] << 24);
            }
            else
            {
                return value[index + 3]
                       + ((uint)value[index + 2] << 8)
                       + ((uint)value[index + 1] << 16)
                       + ((uint)value[index + 0] << 24);
            }
        }

        public static int ToInt32(byte[] value, int index, bool littleEndian)
        {
            return unchecked((int)ToUInt32(value, index, littleEndian));
        }

        public static uint ToUInt32(byte[] value, bool littleEndian)
        {
            return ToUInt32(value, 0, littleEndian);
        }

        internal static ulong ToUInt64(byte[] value, bool littleEndian)
        {
            if (littleEndian)
            {
                return value[0]
                       + ((ulong)value[1] << 8)
                       + ((ulong)value[2] << 16)
                       + ((ulong)value[3] << 24)
                       + ((ulong)value[4] << 32)
                       + ((ulong)value[5] << 40)
                       + ((ulong)value[6] << 48)
                       + ((ulong)value[7] << 56);
            }
            else
            {
                return value[7]
                    + ((ulong)value[6] << 8)
                    + ((ulong)value[5] << 16)
                    + ((ulong)value[4] << 24)
                    + ((ulong)value[3] << 32)
                       + ((ulong)value[2] << 40)
                       + ((ulong)value[1] << 48)
                       + ((ulong)value[0] << 56);
            }
        }

        public static int GetHashCode(byte[] array)
        {
            unchecked
            {
                if (array == null)
                {
                    return 0;
                }
                int hash = 17;
                for (int i = 0; i < array.Length; i++)
                {
                    hash = hash * 31 + array[i];
                }
                return hash;
            }
        }

        internal static byte[] SafeSubarray(this byte[] array, int offset, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (offset < 0 || offset > array.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 0 || offset + count > array.Length)
                throw new ArgumentOutOfRangeException("count");

            var data = new byte[count];
            Buffer.BlockCopy(array, offset, data, 0, count);
            return data;
        }

        internal static byte[] SafeSubarray(this byte[] array, int offset)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (offset < 0 || offset > array.Length)
                throw new ArgumentOutOfRangeException("offset");

            var count = array.Length - offset;
            var data = new byte[count];
            Buffer.BlockCopy(array, offset, data, 0, count);
            return data;
        }
    }

}
