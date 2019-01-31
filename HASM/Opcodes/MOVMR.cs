using System;
using System.Collections.Generic;
using System.IO;

namespace HASM.Opcodes
{
    public class MOVMR : Opcode
    {
        public MOVMR()
        {
        }

        public override void Parse(string line, BinaryWriter output, int data_seg_start, LinkedList<Structures.Data> data, LinkedList<Structures.ForwardRef> fref)
        {
            string destReg = line.Split(' ')[1];
            int regNum = int.Parse(destReg.Substring(1));
            string srcReg = line.Split(' ')[2];
            int srcNum = int.Parse(srcReg.Substring(1));
            output.Write((sbyte)0x10);
            output.Write((sbyte)regNum);
            output.Write((sbyte)srcNum);
        }
    }
}
