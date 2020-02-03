using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace Nes
{
    public class Cartridge
    {

        private List<byte> _PRGMemory;
        private List<byte> _CHRMemory;
        private byte _mapperId = 0;
        private byte _PRGBanks = 0;
        private byte _CHRBanks = 0;
        private CartridgeHeader _cartridgeHeader;
        private byte _fileType = 1;
        private Mapper _mapper;
        
            

        public Cartridge(string fileName)
        {
            byte[] fileBytes = File.ReadAllBytes(fileName);
            _cartridgeHeader = new CartridgeHeader(fileBytes[..11]);
            _mapperId = (byte) (((_cartridgeHeader.Mapper2 >> 4) << 4) | (_cartridgeHeader.Mapper1 >> 4));
            _PRGMemory = new List<byte>();
            _CHRMemory = new List<byte>();

            if (_fileType == 0)
            {
            } else if (_fileType == 1)
            {
                _PRGBanks = _cartridgeHeader.PrgRomChunks;
                var startFrom = 16;
                var prgEnd = _PRGBanks * 16384 + startFrom;
                _PRGMemory.AddRange(fileBytes[startFrom..prgEnd]);
                
                if (_cartridgeHeader.ChrRomChunks > 0)
                {
                    _CHRBanks = _cartridgeHeader.ChrRomChunks;
                    startFrom =  prgEnd - 1;
                    var chrEnd = _CHRBanks * 8192 + startFrom;

                    _CHRMemory.AddRange(fileBytes[startFrom..chrEnd]);
                    
                }

            } else if (_fileType == 2)
            {
                
            }

            switch (_mapperId)
            {
                case  0:
                    _mapper = new Mapper_000(_PRGBanks, _CHRBanks);
                    break;
            }
        }
        public bool CpuRead(ushort address, ref byte data)
        {
            var mappedAddress = 0;
            if (!_mapper.cpuMapRead(address, ref mappedAddress)) return false;
            data = _PRGMemory[mappedAddress];
            return true;
        }

        public bool CpuWrite(ushort address, byte data)
        {
            var mappedAddress = 0;
            if (!_mapper.cpuMapWrite(address, ref mappedAddress, data)) return false;
            _PRGMemory[mappedAddress] = data;
            return true;        
        }
        
        public bool PpuRead(ushort address, ref byte data)
        {
            var mappedAddress = 0;
            if (!_mapper.ppuMapRead(address, ref mappedAddress)) return false;
            data = _CHRMemory[mappedAddress];
            return true; 
        }

        public bool PpuWrite(ushort address, byte data)
        {
            var mappedAddress = 0;
            if (!_mapper.ppuMapWrite(address, ref mappedAddress)) return false;
            data = _CHRMemory[mappedAddress];
            return true;        } 
    }
}

public struct CartridgeHeader
{
    public CartridgeHeader(byte[] c)
    {
        Name = new char[] {(char) c[0], (char) c[1], (char) c[2], (char) c[3]};
        PrgRomChunks = (byte) c[4];
        ChrRomChunks = (byte) c[5];
        Mapper1 = (byte) c[6];
        Mapper2 = (byte) c[7];
        PrgRamSize = (byte) c[8];
        TvSystem1 = (byte) c[9];
        TvSystem2 = (byte) c[10];
        
    }
    public char[] Name;
    public byte PrgRomChunks { get; set; }
    public byte ChrRomChunks { get; set; }
    public byte Mapper1 { get; set; }
    public byte Mapper2 { get; set; }
    public byte PrgRamSize { get; set; }
    public byte TvSystem1 { get; set; }
    public byte TvSystem2 { get; set; }
}