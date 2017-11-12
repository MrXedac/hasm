using System;
using System.IO;

namespace HASM.Opcodes
{
    public class ADD : Opcode
    {
        public ADD()
        {
        }

        public override void Parse(string line, BinaryWriter output)
        {
            string destReg = line.Split(' ')[1];
            int regNum = int.Parse(destReg.Substring(1));
            string srcReg = line.Split(' ')[2];
            int srcNum = int.Parse(srcReg.Substring(1));
            output.Write((sbyte)0x6);
            output.Write((sbyte)regNum);
            output.Write((sbyte)srcNum);
        }
    }
}
