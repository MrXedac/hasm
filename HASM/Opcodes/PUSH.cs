using System;
using System.Collections.Generic;
using System.IO;

namespace HASM.Opcodes
{
    public class PUSH : Opcode
    {
        public PUSH()
        {
        }

        public override void Parse(String line, BinaryWriter output, int data_seg_start, LinkedList<Structures.Data> data)
        {
            string litteral = line.Split(' ')[1];
            int abs = int.Parse(litteral.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
            int p1, p2, p3, p4;
            p1 = (abs >> 24) & 0xFF;
            p2 = (abs >> 16) & 0xFF;
            p3 = (abs >> 8) & 0xFF;
            p4 = abs & 0xFF;
            output.Write((sbyte)0x8);
            output.Write((sbyte)p1);
            output.Write((sbyte)p2);
            output.Write((sbyte)p3);
            output.Write((sbyte)p4);
        }
    }
}
