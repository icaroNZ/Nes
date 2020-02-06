/*using System;
using System.Collections.Generic;
using System.Linq;
using PixelEngine;
using static PixelEngine.Pixel.Presets;

namespace Nes
{
    public class Demo_6502 : Game  
    {
        
        private Bus _nes;
        private Cartridge _cartridge;
        private bool _emulationRun = false;
        private float _residualTime = 0.0f;
        private Dictionary<ushort, string> _asm;
        private ushort _selectedPalette = 0x00;

        private static void Main(string[] args)
        {
            Demo_6502 demo = new Demo_6502();

            demo.Construct(128, 48, 8, 8);
            demo.Start();
        }

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
                DrawText(new Point(ramX, ramY), offSet, White);
                ramY += 1;
            }
        }

        private void DrawCpu(int x, int y)
        {
            DrawText(new Point(x, y), "Status:", White);
            
            DrawText(new Point(x + 8 , y), "N", _nes._cpu.GetFlag(CPU6502.Flags6502.N) == 1 ?  Green  : Red );
            DrawText(new Point(x + 10 , y), "V", _nes._cpu.GetFlag(CPU6502.Flags6502.V) == 1 ? Green  : Red );
            DrawText(new Point(x + 12 , y), "-", _nes._cpu.GetFlag(CPU6502.Flags6502.U) == 1 ? Green  : Red );
            DrawText(new Point(x + 14, y), "B", _nes._cpu.GetFlag(CPU6502.Flags6502.B) == 1 ?  Green  : Red );
            DrawText(new Point(x + 16, y), "D", _nes._cpu.GetFlag(CPU6502.Flags6502.D) == 1 ?  Green  : Red );
            DrawText(new Point(x + 18, y), "I", _nes._cpu.GetFlag(CPU6502.Flags6502.I) == 1 ?  Green  : Red );
            DrawText(new Point(x + 20, y), "Z", _nes._cpu.GetFlag(CPU6502.Flags6502.Z) == 1 ?  Green  : Red );
            DrawText(new Point(x + 22, y), "C", _nes._cpu.GetFlag(CPU6502.Flags6502.C) == 1 ?  Green  : Red );
            
            DrawText(new Point(x, y + 2), "PC: $"    + ToHex(_nes._cpu.Pc),White);
            DrawText(new Point(x, y + 4), "A: $"     + ToHex(_nes._cpu.A) + $" [ {_nes._cpu.A} ]", White);
            DrawText(new Point(x, y + 6), "X: $"     + ToHex(_nes._cpu.X) + $" [ {_nes._cpu.X} ]", White);
            DrawText(new Point(x, y + 8), "Y: $"     + ToHex(_nes._cpu.Y) + $" [ {_nes._cpu.Y} ]", White);
            DrawText(new Point(x, y + 10), "Stack: $" + ToHex(_nes._cpu.StackPointer), White);
        }

        private void DrawCode(int x, int y, int lines)
        {
            var temp = _nes._cpu.MapLines.Where(x => x.Key > 128 - lines && x.Key < 128 + lines);
            var i = 0;
            foreach (var b in temp)
            {
                DrawText(new Point(x, y + i), b.Value, White);
                i++;
            };
        }

        public override void OnUpdate(float elapsed)
        {
            if (GetKey(Key.P).Pressed)
            {
                _selectedPalette++;
                _selectedPalette &= 0x07;
            }

            if (_emulationRun)
            {
                if (_residualTime > 0)
                {
                    _residualTime -= elapsed;
                }
                else
                {
                    _residualTime += (1 / 60) - elapsed;

                    do
                    {
                        _nes.Clock();
                    } while (!_nes._ppu.FrameComplete);

                    _nes._ppu.FrameComplete = false;
                }
            }
            Render();

            if (!GetKey(Key.Space).Pressed) return;
            
            do
            {
                _nes._cpu.Clock();

            } 
            while (!_nes._cpu.Complete());


        }
        
        public  void Render()
        {
            DrawRam(2, 2, 0x0000, 16, 16);
            DrawRam(2, 20, 0x8000, 16, 16);
            DrawCpu(95,2);
            DrawCode(95,15, 26);
            DrawSprite(new Point(516, 348), _nes._ppu.GetPatternTable(0, _selectedPalette));
            DrawSprite(new Point(548, 348), _nes._ppu.GetPatternTable(1, _selectedPalette));
            DrawSprite(new Point(0, 0), _nes._ppu.GetScreen());
            DrawText(new Point(1,37), "SPACE = Step Instruction    R = RESET    I = IRQ    N = NMI", Grey);
        }
    }
}*/