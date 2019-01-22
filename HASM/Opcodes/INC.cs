using System;
using System.Collections.Generic;
using System.IO;

namespace HASM.Opcodes
{
    public class INC : Opcode
    {
        public INC()
        {
        }

        public override void Parse(string line, BinaryWriter output, int data_seg_start, LinkedList<Structures.Data> data)
        {
            string destReg = line.Split(' ')[1];
            int regNum = int.Parse(destReg.Substring(1));
            output.Write((sbyte)0x2);
            output.Write((sbyte)regNum);
        }
    }
}
