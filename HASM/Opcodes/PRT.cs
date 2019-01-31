using System;
using System.Collections.Generic;
using System.IO;

namespace HASM.Opcodes
{
    public class PRT : Opcode
    {
        public PRT()
        {
        }

        public override void Parse(string line, BinaryWriter output, int data_seg_start, LinkedList<Structures.Data> data, LinkedList<Structures.ForwardRef> fref)
        {
            string destReg = line.Split(' ')[1];
            int regNum = int.Parse(destReg.Substring(1));
            output.Write((sbyte)0xA);
            output.Write((sbyte)regNum);
        }
    }
}
