using System;
using System.Resources;
using System.Xml.Linq;

namespace Nes
{
    public class Bus
    {
        public byte[] CpuRam { get; set; }

        public CPU6502 _cpu;
        public PPU2c02 _ppu;
        private Cartridge _cartridge;
        private int _systemClockCounter = 0;

        public Bus()
        {
            
            _cpu = new CPU6502();
            _ppu = new PPU2c02();
            _cpu.ConnectBus(this);
            // Ram 2Kb
            CpuRam = new byte[2048];
            for (var i = 0; i < CpuRam.Length -1 ; i++)
            {
                CpuRam[i] = 0;
            }
        }

        public void CpuWrite(ushort address, byte data)
        {
            if (_cartridge.CpuWrite(address, data))// Possible remove
            {
                
            }
            else if (address <= 0x1FFF)
            { 
                CpuRam[address & 0x07FF] = data;
            }
            else if (address >= 0x2000 && address <= 0x3FFF)
            {
                _ppu.CpuWrite((ushort) (address & 0x0007), data);
            }
        }

        public byte CpuRead(ushort address, bool bReadOnly = false)
        {
            byte data = 0x00;
            
            if (_cartridge.CpuRead(address, ref data))// Possible remove
            {
                
            }
            else if (address <= 0x1FFF)
            {
                data = CpuRam[address & 0x07FF];
            }
            else if (address >= 0x2000 && address <= 0x3FFF)
            {
                data = _ppu.CpuRead((ushort) (address & 0x0007));
            }
            return data;
        }

        public void InsertCartridge(Cartridge cartridge)
        {
            _cartridge = cartridge; 
            _ppu.ConnectCartridge(cartridge);
        }

        public void Reset()
        {
            _cpu.Reset();
            _systemClockCounter = 0;
        }

        public void Clock()
        {
            _ppu.Clock();  
            if (_systemClockCounter % 3 == 0)
            {
                _cpu.Clock();
            }

            _systemClockCounter++;
        }
        
        
        
    }
}