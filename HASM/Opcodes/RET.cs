using System;
using System.Collections.Generic;
using System.IO;

namespace HASM.Opcodes
{
    public class RET : Opcode
    {
        public RET()
        {
        }

        public override void Parse(string line, BinaryWriter output, int data_seg_start, LinkedList<Structures.Data> data, LinkedList<Structures.ForwardRef> fref)
        {
            output.Write((sbyte)0x11);
        }
    }
}
