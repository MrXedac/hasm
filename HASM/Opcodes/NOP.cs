using System;
using System.IO;

namespace HASM.Opcodes
{
    public class NOP : Opcode
    {
        public NOP()
        {
        }

        public override void Parse(string line, BinaryWriter output)
        {
            output.Write((sbyte)0x0);
        }
    }
}
