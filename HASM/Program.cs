using System;
using System.Collections.Generic;
using System.IO;

namespace HASM
{
    class MainClass
    {
        private static Dictionary<string, Opcode> lexer;

        private static void InitializeParser()
        {
            lexer = new Dictionary<string, Opcode>();
            lexer.Add("MOV", new Opcodes.MOV());
            lexer.Add("INC", new Opcodes.INC());
            lexer.Add("DEC", new Opcodes.DEC());
            lexer.Add("NOP", new Opcodes.NOP());
            lexer.Add("ADD", new Opcodes.ADD());
            lexer.Add("SUB", new Opcodes.SUB());
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
                string opcode = line.Split(' ')[0].ToUpper();
                lexer[opcode].Parse(line, outp);
                // Console.WriteLine(line.Split(' ')[0]);
            }
            sr.Close();
            outp.Close();
            Console.WriteLine("Done.");
        }
    }
}
