using System;
using System.Collections.Generic;
using System.IO;

namespace HASM.Opcodes
{
    public class CALL : Opcode
    {
        public CALL()
        {
        }

        public override void Parse(String line, BinaryWriter output, int data_seg_start, LinkedList<Structures.Data> data, LinkedList<Structures.ForwardRef> fref)
        {
            bool failed = true;
            string litteral = line.Split(' ')[1];
            int p1 = 0, p2 = 0, p3 = 0, p4 = 0;
            try
            {
                int abs = int.Parse(litteral.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
                p1 = (abs >> 24) & 0xFF;
                p2 = (abs >> 16) & 0xFF;
                p3 = (abs >> 8) & 0xFF;
                p4 = abs & 0xFF;

            }
            catch (Exception)
            {
                /* Couldn't parse litteral - maybe it's an identifier ? */
                foreach (Structures.Data d in data)
                {
                    if (d.identifier.Equals(litteral))
                    {
                        // Console.WriteLine("It's an identifier at offset " + (d.offset + data_seg_start));
                        int offset = d.offset;
                        p1 = (offset >> 24) & 0xFF;
                        p2 = (offset >> 16) & 0xFF;
                        p3 = (offset >> 8) & 0xFF;
                        p4 = offset & 0xFF;
                        failed = false;
                        break;
                    }
                }

                if (failed)
                {
                    /* Forward reference ? */
                    Console.WriteLine("/!\\ Potential forward reference to " + litteral + " at address " + (output.BaseStream.Position + 1));

                    /* Add forward reference to parse later */
                    fref.AddLast(new Structures.ForwardRef(litteral, (int)output.BaseStream.Position + 1));
                    failed = false; /* Do not fail _yet_ */
                }
            }
            if (failed)
            {
                Console.WriteLine("Parse error: unknown identifier or litteral " + litteral);
                Environment.Exit(-1);
            }
            else
            {
                output.Write((sbyte)0x5);
                output.Write((sbyte)p1);
                output.Write((sbyte)p2);
                output.Write((sbyte)p3);
                output.Write((sbyte)p4);
            }
        }
    }
}
