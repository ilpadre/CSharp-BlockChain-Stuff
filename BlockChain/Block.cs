using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public class Block
    {
        private static DateTime _epochBaseDate = new DateTime(1970, 1, 1);

        public string fileName { get; set; }

        public BlockHeader header { get; set; }

        public uint HeaderLength { get; set; }
        public long TransactionCount { get; set; }

        public uint BlockNumber { get; set; }

        public Block()
        {
            header = new BlockHeader();
        }

        public List<Transaction> Transactions = new List<Transaction>();
    }

    public class BlockHeader
    {
        public uint VersionNumber { get; set; }
        public byte[] PreviousBlockHash { get; set; }
        public byte[] MerkleRoot { get; set; }
        public DateTime TimeStamp { get; set; }
        public uint LockTime { get; set; }
        public long Size { get; set; }
        public uint Nonce { get; set; }
        public uint Bits { get; set; }
    }
}
