using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace HASM
{
    public class Opcode
    {
        public virtual void Parse(String line, BinaryWriter output, int data_seg_start, LinkedList<Structures.Data> data, LinkedList<Structures.ForwardRef> fref)
        {
            
        }
    }
}
