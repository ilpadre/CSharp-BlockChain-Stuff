using System;
using System.Text;
using Base58Check;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Block58Check.Test
{
    [TestClass]
    public class Base58CheckTest_Encode
    {

        [TestMethod]
        public void CanEncodeHello()
        {
            var bytes = Encoding.ASCII.GetBytes("hello");

            var b58str = Base58Check.Base58Check.Encode(bytes);
            Assert.AreEqual("Cn8eVZg", b58str);
        }

        [TestMethod]
        public void CanDecodeHello()
        {
            var b58 = Base58Check.Base58Check.Decode("Cn8eVZg");
            Assert.AreEqual("hello", Encoding.ASCII.GetString(b58));
        }
        
        [TestMethod]
        public void CanEncodeHelloWithOneLeadingZero()
        {
            var bytesWithZero = new byte[6];
            bytesWithZero[0] = 0;
            bytesWithZero[1] = Convert.ToByte('h');
            bytesWithZero[2] = Convert.ToByte('e');
            bytesWithZero[3] = Convert.ToByte('l');
            bytesWithZero[4] = Convert.ToByte('l');
            bytesWithZero[5] = Convert.ToByte('o');


            var b58str = Base58Check.Base58Check.Encode(bytesWithZero);
            Assert.AreEqual("1Cn8eVZg", b58str);
        }

        [TestMethod]
        public void CanDecodeHelloWithOneLeadingZero()
        {
            var b58 = Base58Check.Base58Check.Decode("1Cn8eVZg");
            var bytes = new byte[5];
            for (int i = 1; i < b58.Length; i++)
            {
                bytes[i - 1] = b58[i];
            }
            Assert.AreEqual("hello", Encoding.ASCII.GetString(bytes));
        }

        [TestMethod]
        public void CanEncodeHelloWithTenLeadingZero()
        {
            var bytesWithZero = new byte[15];
            for (int i = 0; i < 10; i++)
            {
                bytesWithZero[i] = 0;
            }
            bytesWithZero[10] = Convert.ToByte('h');
            bytesWithZero[11] = Convert.ToByte('e');
            bytesWithZero[12] = Convert.ToByte('l');
            bytesWithZero[13] = Convert.ToByte('l');
            bytesWithZero[14] = Convert.ToByte('o');


            var b58str = Base58Check.Base58Check.Encode(bytesWithZero);
            Assert.AreEqual("1111111111Cn8eVZg", b58str);
        }

        [TestMethod]
        public void CanDecodeHelloWithTenLeadingZero()
        {
            var b58 = Base58Check.Base58Check.Decode("1111111111Cn8eVZg");
            var bytes = new byte[5];
            for (int i = 10; i < b58.Length; i++)
            {
                bytes[i - 10] = b58[i];
            }
            Assert.AreEqual("hello", Encoding.ASCII.GetString(bytes));
        }

        [TestMethod]
        [Ignore]
        public void CanEncodePrivateKey()
        {
            var key = Encoding.ASCII.GetBytes("E9873D79C6D87DC0FB6A5778633389F4453213303DA61F20BD67FC233AA33262");
            var expected = "5Kb8kLf9zgWQnogidDA76MzPL6TsZZY36hWXMssSzNydYXYB9KF";
            var b58str = Base58Check.Base58Check.Encode(key);
            Assert.AreEqual(expected, b58str);
        }

    }
}
