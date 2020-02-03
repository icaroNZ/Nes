using System;
using System.Collections.Generic;
using PixelEngine;

namespace Nes
{
    public class Demo_6502 : Game
    {
        private Bus _nes;
        private Cartridge _cartridge;
        private bool _emulationRun = false;
        private float _residualTime = 0.0f;
        private Dictionary<ushort, string> _asm;
        
        
        public Demo_6502()
        {
            _nes = new Bus();
            _cartridge = new Cartridge("nestest.nes");
            _asm = _nes._cpu.Disassemble(0x0000, 0xFFFF);
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
                    offSet += Convert.ToString(_nes.CpuRead(address, true), 16);
                }
            }
        }
    }
}