using System;
using System.IO;

namespace HASM.Opcodes
{
    public class MOV : Opcode
    {
        public MOV()
        {
        }

        public override void Parse(String line, BinaryWriter output){
            string destReg = line.Split(' ')[1];
            int regNum = int.Parse(destReg.Substring(1));
            string litteral = line.Split(' ')[2];
            int abs = int.Parse(litteral.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
            int p1, p2, p3, p4;
            p1 = (abs >> 24) & 0xFF;
            p2 = (abs >> 16) & 0xFF;
            p3 = (abs >> 8) & 0xFF;
            p4 = abs & 0xFF;
            output.Write((sbyte)0x1);
            output.Write((sbyte)regNum);
            output.Write((sbyte)p1);
            output.Write((sbyte)p2);
            output.Write((sbyte)p3);
            output.Write((sbyte)p4);
        }
    }
}
