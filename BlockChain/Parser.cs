using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public class Parser
    {
        private readonly long _position;

        public List<Block> Parse(string blocksPath)
        {
            List<Block> blocks = new List<Block>();
            uint blockNumber = 0;
            DirectoryInfo di = new DirectoryInfo(blocksPath);
            List<FileSystemInfo> files = di.GetFileSystemInfos().Where(x => x.Name.StartsWith("blk"))
                .Where(x => x.Extension == ".dat").ToList();
            var orderedFiles = files.OrderBy(f => f.Name);
            foreach (var file in orderedFiles)
            {
                using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        while (ReadMagic(reader))
                        {
                            var block = ReadBlock(reader);
                            block.BlockNumber = ++blockNumber;
                            block.fileName = file.Name;
                            blocks.Add(block);
                        }
                    }
                }
            }
            return blocks;
        }


        public List<Block> ParseHeader(string blocksPath)
        {
            List<Block> blocks = new List<Block>();
            uint blockNumber = 0;
            DirectoryInfo di = new DirectoryInfo(blocksPath);
            List<FileSystemInfo> files = di.GetFileSystemInfos().Where(x => x.Name.StartsWith("blk"))
                .Where(x => x.Extension == ".dat").ToList();
            var orderedFiles = files.OrderBy(f => f.Name);
            foreach (var file in orderedFiles)
            {
                using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        while (ReadMagic(reader))
                        {
                            var block = ReadBlockHeader(reader);
                            block.BlockNumber = ++blockNumber;
                            block.fileName = file.Name;
                            blocks.Add(block);
                        }
                    }
                }
            }
            return blocks;
        }

        private Block ReadBlockHeader(BinaryReader reader)
        {
            var block = new Block();
            //block.MagicId = magic;
            block.HeaderLength = reader.ReadUInt32();
            //reader.BaseStream.Seek(block.HeaderLength, SeekOrigin.Current);
            //return block;
            block.header.VersionNumber = reader.ReadUInt32();
            block.header.PreviousBlockHash = reader.ReadHashAsByteArray();
            block.header.MerkleRoot = reader.ReadHashAsByteArray();
            block.header.TimeStamp = Util.UnixTimeStampToDateTime(reader.ReadUInt32());
            block.header.Bits = reader.ReadUInt32();
            block.header.Nonce = reader.ReadUInt32();

            var transactionCount = reader.ReadVarInt();
            block.TransactionCount = transactionCount;
            return block;
        }

        private Block ReadBlock(BinaryReader reader)
        {
            //var magic = reader.ReadUInt32();
            //if(magic != 3652501241) throw new Exception("el numero magico no coincide");

            var block = new Block();
            //block.MagicId = magic;
            block.HeaderLength = reader.ReadUInt32();
            //reader.BaseStream.Seek(block.HeaderLength, SeekOrigin.Current);
            //return block;
            block.header.VersionNumber = reader.ReadUInt32();
            block.header.PreviousBlockHash = reader.ReadHashAsByteArray();
            block.header.MerkleRoot = reader.ReadHashAsByteArray();
            block.header.TimeStamp = Util.UnixTimeStampToDateTime(reader.ReadUInt32());
            block.header.Bits = reader.ReadUInt32();
            block.header.Nonce = reader.ReadUInt32();

            var transactionCount = reader.ReadVarInt();
            block.TransactionCount = transactionCount;
            block.Transactions = new List<Transaction>();

            for (var ti = 0; ti < transactionCount; ti++)
            {
                var t = new Transaction();
                t.VersionNumber = reader.ReadUInt32();

                var inputCount = reader.ReadVarInt();
                t.Inputs = new Input[inputCount];

                for (var ii = 0; ii < inputCount; ii++)
                {
                    var input = new Input();
                    input.TransactionHash = reader.ReadHashAsByteArray();
                    input.TransactionIndex = reader.ReadUInt32();
                    input.Script = reader.ReadStringAsByteArray();
                    string something = Encoding.ASCII.GetString(input.Script);
                    input.SequenceNumber = reader.ReadUInt32();
                    t.Inputs[ii] = input;
                }

                var outputCount = reader.ReadVarInt();
                t.Outputs = new Output[outputCount];

                for (var oi = 0; oi < outputCount; oi++)
                {
                    var output = new Output();
                    output.Value = reader.ReadUInt64();
                    output.Script = reader.ReadStringAsByteArray();
                    t.Outputs[oi] = output;
                }
                block.header.LockTime = reader.ReadUInt32();
                block.Transactions.Add(t);
            }

            return block;
        }

        private bool ReadMagic(BinaryReader reader)
        {
            try
            {
                ini:
                //var magicNumber = reader.ReadUInt32();
                while (true)
                {
                    var magic = reader.ReadUInt32();
                    if (magic == 3652501241)
                    {
                        return true;
                    }
                }


                return true;

//                byte b0 = reader.ReadByte();
//                if (b0 != 0xF9) goto ini;
//                b0 = reader.ReadByte();
//                if (b0 != 0xbe) goto ini;
//                b0 = reader.ReadByte();
//                if (b0 != 0xb4) goto ini;
//                b0 = reader.ReadByte();
//                if (b0 != 0xd9) goto ini;
//                return true;
            }
            catch (EndOfStreamException)
            {
                return false;
            }
        }


        public void SaveFirstBlock(string blocksPath)
        {
            byte[] block;
            DirectoryInfo di = new DirectoryInfo(blocksPath);
            List<FileSystemInfo> files = di.GetFileSystemInfos().Where(x => x.Name.StartsWith("blk"))
                .Where(x => x.Extension == ".dat").ToList();
            var firstFile = files.OrderBy(f => f.Name).FirstOrDefault();

            using (var stream = new FileStream(firstFile.FullName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    block = ReadFirstBlock(reader);
                }
            }

            using (var stream = new FileStream("block0.bin", FileMode.Create, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(block);
                }
            }
        }

        private byte[] ReadFirstBlock(BinaryReader reader)
        {
            var magic = BitConverter.GetBytes(reader.ReadUInt32());
            var blockLength = reader.ReadUInt32();
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            var block = reader.ReadBytes(Convert.ToInt32(blockLength));
            return block;
        }
    }
}