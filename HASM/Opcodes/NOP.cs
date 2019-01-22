using System;
using System.Collections.Generic;
using System.IO;

namespace HASM.Opcodes
{
    public class NOP : Opcode
    {
        public NOP()
        {
        }

        public override void Parse(string line, BinaryWriter output, int data_seg_start, LinkedList<Structures.Data> data)
        {
            output.Write((sbyte)0x0);
        }
    }
}
