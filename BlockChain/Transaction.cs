using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public class Transaction
    {
        public uint VersionNumber;
        public Input[] Inputs;
        public Output[] Outputs;
    }

    public class Input
    {
        public byte[] TransactionHash;
        public uint TransactionIndex;
        public byte[] Script;
        public uint SequenceNumber;
    }

    public class Output
    {
        public ulong Value;
        public byte[] Script;
    }

}
