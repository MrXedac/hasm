using System;
using System.Collections.Generic;
using System.IO;
namespace HASM
{
    public class Opcode
    {
        public virtual void Parse(String line, BinaryWriter output, int data_seg_start, LinkedList<Structures.Data> data)
        {
            
        }
    }
}
