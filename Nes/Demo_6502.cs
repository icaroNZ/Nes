using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ConsoleGameEngine;

namespace Nes
{
    public class Demo_6502 : ConsoleGame  
    {
        
        private Bus _nes;
        private Cartridge _cartridge;
        private bool _emulationRun = false;
        private float _residualTime = 0.0f;
        private Dictionary<ushort, string> _asm;

        public string ToHex(int number)
        {
            return $"0x{number:x2}";
        }
        
        public void Start()
        {

            
            _nes = new Bus();
            _cartridge = new Cartridge("nestest.nes");
            _nes.InsertCartridge(_cartridge);
            
             var asm = _nes._cpu.Disassemble(0x0000, 0xFFFF);
             _nes._cpu.Reset();
        }

        private void DrawRam(int x, int y, ushort address, int rows, int cols)
        {
            var ramX = x;
            var ramY = y;
            for (var row = 0; row < rows; row++)
            {
                var offSet = "$" + ToHex(address) + ":";
                for (var col = 0; col < cols; col++)
                {
                    offSet += " " + ToHex(_nes.CpuRead(address, true));
                    address++;
                }
                Engine.WriteText(new Point(ramX, ramY), offSet, 7);
                ramY += 1;
            }
        }

        private void DrawCpu(int x, int y)
        {
            Engine.WriteText(new Point(x, y), "Status:", 7);
            
            Engine.WriteText(new Point(x + 8 , y), "N", _nes._cpu.GetFlag(CPU6502.Flags6502.N) == 1 ?  10  : 4 );
            Engine.WriteText(new Point(x + 10 , y), "V", _nes._cpu.GetFlag(CPU6502.Flags6502.V) == 1 ? 10  : 4 );
            Engine.WriteText(new Point(x + 12 , y), "-", _nes._cpu.GetFlag(CPU6502.Flags6502.U) == 1 ? 10  : 4 );
            Engine.WriteText(new Point(x + 14, y), "B", _nes._cpu.GetFlag(CPU6502.Flags6502.B) == 1 ?  10  : 4 );
            Engine.WriteText(new Point(x + 16, y), "D", _nes._cpu.GetFlag(CPU6502.Flags6502.D) == 1 ?  10  : 4 );
            Engine.WriteText(new Point(x + 18, y), "I", _nes._cpu.GetFlag(CPU6502.Flags6502.I) == 1 ?  10  : 4 );
            Engine.WriteText(new Point(x + 20, y), "Z", _nes._cpu.GetFlag(CPU6502.Flags6502.Z) == 1 ?  10  : 4 );
            Engine.WriteText(new Point(x + 22, y), "C", _nes._cpu.GetFlag(CPU6502.Flags6502.C) == 1 ?  10  : 4 );
            
            Engine.WriteText(new Point(x, y + 2), "PC: $"    + ToHex(_nes._cpu.Pc),7);
            Engine.WriteText(new Point(x, y + 4), "A: $"     + ToHex(_nes._cpu.A) + $" [ {_nes._cpu.A} ]", 7);
            Engine.WriteText(new Point(x, y + 6), "X: $"     + ToHex(_nes._cpu.X) + $" [ {_nes._cpu.X} ]", 7);
            Engine.WriteText(new Point(x, y + 8), "Y: $"     + ToHex(_nes._cpu.Y) + $" [ {_nes._cpu.Y} ]", 7);
            Engine.WriteText(new Point(x, y + 10), "Stack: $" + ToHex(_nes._cpu.StackPointer), 7);
        }

        private void DrawCode(int x, int y, int lines)
        {
            var temp = _nes._cpu.MapLines.Where(x => x.Key > 128 - lines && x.Key < 128 + lines);
            var i = 0;
            foreach (var b in temp)
            {
                Engine.WriteText(new Point(x, y + i), b.Value, 7);
                i++;
            };
        }
        

        private static void Main(string[] args)
        {
            new Demo_6502().Construct(128, 48, 8, 8, FramerateMode.MaxFps);
        }
        
        public override void Create()
        {
            Engine.SetPalette(Palettes.Default);
            Engine.Borderless();

            TargetFramerate = 15;
            Start();
        }

        public override void Update()
        {
            Engine.ClearBuffer();


             if (Engine.GetKey(ConsoleKey.Spacebar))
             {
                do
                {
                    _nes._cpu.Clock();

                } 
                while (!_nes._cpu.Complete());
             }      
      

        }
        
        

        public override void Render()
        {
            Engine.ClearBuffer();
            DrawRam(2, 2, 0x0000, 16, 16);
            DrawRam(2, 20, 0x8000, 16, 16);
            DrawCpu(95,2);
            DrawCode(95,15, 26);
            Engine.WriteText(new Point(1,37), "SPACE = Step Instruction    R = RESET    I = IRQ    N = NMI", 123);
            for (var i = 0; i < 16; i++)
            {
                Engine.WriteText(new Point(1 + i * 3,40), " " + i.ToString() + " ", i);
            }
            Engine.DisplayBuffer();          
        }
    }
}