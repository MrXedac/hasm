using System;
using System.IO;

namespace HASM.Structures
{
    public class Data
    {
        public Int32 offset;
        public byte[] data;
        public string identifier;
        public DataKind kind;

        public Data(string id, Int32 offset, byte[] data, DataKind kind)
        {
            this.offset = offset;
            this.data = data;
            this.identifier = id;
            this.kind = kind;
        }

        public void Write(BinaryWriter outp)
        {
            if (this.kind != DataKind.LABEL)
            {
                foreach (byte b in data)
                {
                    outp.Write((sbyte)b);
                }
                if (this.kind == DataKind.STRING)
                    outp.Write((sbyte)'\0');
            }
        }
    }
}
