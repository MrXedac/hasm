using System;
using System.IO;

namespace HASM.Opcodes
{
    public class DEC : Opcode
    {
        public DEC()
        {
        }

        public override void Parse(string line, BinaryWriter output)
        {
            string destReg = line.Split(' ')[1];
            int regNum = int.Parse(destReg.Substring(1));
            output.Write((sbyte)0x3);
            output.Write((sbyte)regNum);
        }
    }
}
