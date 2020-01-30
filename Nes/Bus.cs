using System;
using System.Xml.Linq;

namespace Nes
{
    public class Bus
    {
        public Olc6502 Cpu { get; set; }
        public byte[] Ram { get; set; }

        public Bus()
        {
            Cpu = new Olc6502();
            Cpu.ConnectBus(this);
            
            Ram = new byte[64 * 1024];
            for (var i = 0; i < Ram.Length -1 ; i++)
            {
                Ram[i] = 0;
            }
        }

        public void Write(ushort address, byte data)
        {
            if (address <= 0xFFFF)
            {
                Ram[address] = data;
            }
        }

        public byte Read(ushort address, bool bReadOnly = false)
        {
            if (address <= 0xFFFF)
            {
                return Ram[address];
            }
            return 1;
        }
        
    }
}