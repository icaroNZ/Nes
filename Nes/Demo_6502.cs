using System;
using System.Collections.Generic;
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
                var offSet = "$" + Convert.ToString(address, 16) + ":";
                for (var col = 0; col < cols; col++)
                {
                    offSet += " " + Convert.ToString(_nes.CpuRead(address, true), 16);
                    address++;
                }
                Console.WriteLine(offSet);
                Engine.WriteText(new Point(ramX, ramY), offSet, 233);
            }
        }

        private static void Main(string[] args)
        {
            new Demo_6502().Construct(128, 64, 4, 4, FramerateMode.MaxFps);
        }
        
        public override void Create()
        {
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();

            TargetFramerate = 15;
            Start();
        }

        public override void Update()
        {
            Engine.ClearBuffer();


            // if (Engine.GetKey(ConsoleKey.Spacebar))
            // {
                do
                {
                    _nes._cpu.Clock();
                } 
                while (!_nes._cpu.Complete());
            // }      
            DrawRam(2, 2, 0x0000, 16, 16);
            DrawRam(2, 182, 0x8000, 16, 16);
            Engine.WriteText(new Point(10,370), "SPACE = Step Instruction    R = RESET    I = IRQ    N = NMI", 123);
        }
        
        

        public override void Render()
        {
            Engine.ClearBuffer();

            DrawRam(1,1,0,10,10);
            Engine.DisplayBuffer();        }
    }
}