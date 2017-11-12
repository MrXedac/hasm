using System;
using System.IO;

namespace HASM.Opcodes
{
    public class INC : Opcode
    {
        public INC()
        {
        }

        public override void Parse(string line, BinaryWriter output){
            string destReg = line.Split(' ')[1];
            int regNum = int.Parse(destReg.Substring(1));
            output.Write((sbyte)0x2);
            output.Write((sbyte)regNum);
        }
    }
}
