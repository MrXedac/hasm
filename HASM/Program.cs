using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HASM
{
    class MainClass
    {
        private static Dictionary<string, Opcode> lexer;
        private static LinkedList<Structures.Data> data;
        private static int next_offset = 0;
        private static int data_seg_addr = 0x1000;
        private static LinkedList<string> code;
        private static LinkedList<Structures.ForwardRef> fref;

        private static void InitializeParser()
        {
            lexer = new Dictionary<string, Opcode>();
            lexer.Add("MOV", new Opcodes.MOV());
            lexer.Add("INC", new Opcodes.INC());
            lexer.Add("DEC", new Opcodes.DEC());
            lexer.Add("NOP", new Opcodes.NOP());
            lexer.Add("ADD", new Opcodes.ADD());
            lexer.Add("SUB", new Opcodes.SUB());
            lexer.Add("JMP", new Opcodes.JMP());
            lexer.Add("PUSH", new Opcodes.PUSH());
            lexer.Add("POP", new Opcodes.POP());
            lexer.Add("PRT", new Opcodes.PRT());
            lexer.Add("STOP", new Opcodes.STOP());
            lexer.Add("MOVM", new Opcodes.MOVM());
            lexer.Add("CMP", new Opcodes.CMP());
            lexer.Add("JEQ", new Opcodes.JEQ());

            data = new LinkedList<Structures.Data>();
            code = new LinkedList<string>();
            fref = new LinkedList<Structures.ForwardRef>();
        }

        private static void ParseData(string l)
        {
            string dat = l.TrimStart(".".ToCharArray());
            // Console.WriteLine(dat);
            // string datakind = dat.Split(" ".ToCharArray())[0].ToLower();

            var parts = Regex.Matches(dat, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();
            string datakind = parts[0];

            switch (datakind)
            {
                case "string":
                    string id = parts[1];
                    string str = parts[2].Trim("\"".ToCharArray());

                    /* We have weird things going on due to the regex (probably) screwing up \n and stuff. Fix \n for now. */
                    str = str.Replace("\\n", Environment.NewLine);

                    /* Convert string to byte array */
                    byte[] arr = str.Select(c => (byte)c).ToArray();

                    data.AddLast(new Structures.Data(id, next_offset, arr, Structures.DataKind.STRING));
                    //Console.WriteLine("Add data " + id + " containing " + arr.Length + " at offset " + next_offset);
                    next_offset += arr.Length + 1; /* Add the \0 terminator */
                    break;
                case "int8":
                    break;
                case "int16":
                    break;
                case "int32":
                    break;
                case "raw":
                    break;
                case "dataat":
                    /* Put the datas at a specific address */
                    string addr = parts[1];
                    int abs = int.Parse(addr.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    Console.WriteLine("Data segment will be put at address " + addr);
                    data_seg_addr = abs;
                    break;
                case "label":
                    code.AddLast(l);
                    break;
                default:
                    Console.WriteLine("Unknown data kind " + datakind);
                    break;
            }
        }

        public static void Main(string[] args)
        {
            InitializeParser();
            FileStream src = File.Open(args[0], FileMode.Open);
            StreamReader sr = new StreamReader(src);
            FileStream dst = File.Open(args[1], FileMode.Create);
            BinaryWriter outp = new BinaryWriter(dst);
            String line;
            Console.WriteLine("HASM assembler");
            Console.WriteLine("Assembling " + args[0] + " to " + args[1] + "...");
            while ((line = sr.ReadLine()) != null)
            {
                String l = line.Trim();
                if (l.Length > 0)
                {
                    if (l.StartsWith(";", StringComparison.CurrentCultureIgnoreCase))
                    {
                        /* Comment */
                    }
                    else if (l.StartsWith(".", StringComparison.CurrentCultureIgnoreCase))
                    {
                        /* Data (string, int...) */
                        ParseData(l);
                    }
                    else
                    {
                        /* Queue code for second pass */
                        code.AddLast(l);
                    }
                }
                // Console.WriteLine(line.Split(' ')[0]);
            }

            /* First pass done, we should have all the identifiers and data
             * Now parse code ! */
            foreach (string c in code)
            {
                string opcode = c.Split(' ')[0].ToUpper();

                /* Check for label */
                if(opcode == ".LABEL")
                {
                    string label = c.Split(' ')[1];
                    /* This is a label, not an opcode. Parse it and the accurate address */
                    data.AddLast(new Structures.Data(label, (int)outp.BaseStream.Position, null, Structures.DataKind.LABEL));
                    // Console.WriteLine("Label " + label + " found at address " + (int)outp.BaseStream.Position);
                } else lexer[opcode].Parse(c, outp, data_seg_addr, data, fref);
            }

            /* Let's keep the current position */
            long pos = outp.BaseStream.Position;
            long padsize = data_seg_addr - pos;

            /* Now before we do some padding and write data, let's resolve forward references */
            foreach(Structures.ForwardRef f in fref)
            {
                bool found = false;
                foreach(Structures.Data d in data)
                {
                    if(d.identifier == f.identifier)
                    {
                        /* 
                           Found ref! 
                           Position our writer at the corresponding location and write address
                        */
                        outp.Seek(f.offset, SeekOrigin.Begin);
                        if(d.kind == Structures.DataKind.LABEL)
                        {
                            outp.Write((sbyte)((d.offset >> 24) & 0xFF));
                            outp.Write((sbyte)((d.offset >> 16) & 0xFF));
                            outp.Write((sbyte)((d.offset >> 8) & 0xFF));
                            outp.Write((sbyte)((d.offset) & 0xFF));
                        }
                        else
                        {
                            int addr = d.offset + data_seg_addr;

                            outp.Write((sbyte)((addr >> 24) & 0xFF));
                            outp.Write((sbyte)((addr >> 16) & 0xFF));
                            outp.Write((sbyte)((addr >> 8) & 0xFF));
                            outp.Write((sbyte)((addr) & 0xFF));
                        }
                        Console.WriteLine("/~\\ Resolved forward reference to " + f.identifier + ", at address " + d.offset);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Couldn't resolve reference to " + f.identifier + ". Exiting.");
                    Environment.Exit(-1);
                }
                found = false;
            }

            outp.Seek((int)pos, SeekOrigin.Begin);

            /* Pad */
            for (long i = 0; i < padsize; i++)
                outp.Write((sbyte)0x0);

            /* Now write data in resulting binary */
            foreach(Structures.Data d in data)
            {
                d.Write(outp);
            }

            sr.Close();
            outp.Close();
            Console.WriteLine("Done.");
        }
    }
}
