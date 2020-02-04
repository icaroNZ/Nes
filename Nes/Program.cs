using System;
using PixelEngine;

namespace Nes
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = new Bus();
            var cpu = new CPU6502();
            var cartridge = new Cartridge("nestest.nes");
            //var cartridge = new Cartridge("Super Mario Bros.nes");

            bus.InsertCartridge(cartridge);
            
            var asm = bus._cpu.Disassemble(0x0000, 0xFFFF);
            bus._cpu.Reset();

            
            // var ss =  "A2 0A 8E 00 00 A2 03 8E 01 00 AC 00 00 A9 00 18 6D 01 00 88 D0 FA 8D 02 00 EA EA EA";
            // ushort nOffset = 0x8000;
            //
            // for (var i = 0; i < ss.Length -1; i += 3)
            // {
            //
            //     var temp = "0x"+ ss[i] + ss[i + 1];
            //    var b = (byte) Convert.ToInt32(temp, 16);
            //    bus.CpuRam[nOffset] = b;
            //    Console.WriteLine(temp);
            //    Console.WriteLine(b);
            //    Console.WriteLine(nOffset);
            //    Console.WriteLine(bus.CpuRam[nOffset]);
            //    nOffset++;
            // }
            //


            // var asm = bus._cpu.Disassemble(0x0000, 0xFFFF);
            //bus._cpu.Reset();

            while (true)
            {
                do
                {
                    bus._cpu.Clock();
                } while (!bus._cpu.Complete());
            }  

        }
    }
}   