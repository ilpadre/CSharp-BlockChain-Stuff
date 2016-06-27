using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public static class Util
    {
        public static long CalculateDifficulty(uint bits)
        {
            //is 0x1b0404cb, the hexadecimal target is
            //0x0404cb * 2**(8*(0x1b - 3)) = 0x00000000000404CB000000000000000000000000000000000000000000000000
            uint p = bits & 0x00FFFFFF;
            uint e = (bits & 0xFF000000) >> 24;
            var dif = p * Math.Pow(2, (8 * (e - 3)));
            return (long)dif;
        }

        public static long ReadVarInt(this BinaryReader reader)
        {
            var t = reader.ReadByte();
            if (t < 0xfd) return t;
            if (t == 0xfd) return reader.ReadInt16();
            if (t == 0xfe) return reader.ReadInt32();
            if (t == 0xff) return reader.ReadInt64();

            throw new InvalidDataException("Reading Var Int");
        }

        public static byte[] ReadStringAsByteArray(this BinaryReader reader)
        {
            var scriptLength = reader.ReadVarInt();
            return reader.ReadBytes((int)scriptLength);
        }

        public static byte[] ReadHashAsByteArray(this BinaryReader reader)
        {
            return reader.ReadBytes(32);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

    public static class HashExtensions
    {
        public static string ToHashString(this byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }
    }



    //        private void ReadHeader()
    //        {
    //            if (_versionNumber > 0) return;
    //            var r = _reader.Value;
    //            r.BaseStream.Position = _position + 4;
    //            _versionNumber = r.ReadUInt32();
    //            _previousBlockHash = r.ReadHashAsByteArray();
    //            _merkleRoot = r.ReadHashAsByteArray();
    //            _timeStamp = _epochBaseDate.AddSeconds(r.ReadUInt32());
    //            _bits = r.ReadUInt32();
    //            _nonce = r.ReadUInt32();
    //            _transactionCount = r.ReadVarInt();
    //        }

    //        public IEnumerable<Transaction> Transactions
    //        {
    //            get
    //            {
    //                var r = _reader.Value;
    //                for (var ti = 0; ti < _transactionCount; ti++)
    //                {
    //                    var t = new Transaction();
    //                    t.VersionNumber = r.ReadUInt32();
    //
    //                    var inputCount = r.ReadVarInt();
    //                    t.Inputs = new Input[inputCount];
    //
    //                    var outputCount = r.ReadVarInt();
    //                    t.Outputs = new Output[outputCount];
    //
    //                    yield return t;
    //                }
    //            }
    //        }
    //
    //        public Block(Stream stream)
    //        {
    //            _position = stream.Position;
    //            _stream = stream;
    //            _reader = new Lazy<BinaryReader>(() => new BinaryReader(stream));
    //        }
}
