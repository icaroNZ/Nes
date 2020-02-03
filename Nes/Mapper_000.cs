namespace Nes
{
    public class Mapper_000 : Mapper
    {
        public Mapper_000(byte prgBanks, byte charBanks) : base(prgBanks, charBanks)
        {
        }
        public override bool cpuMapRead(ushort address, ref int mappedAddress)
        {
            if (address < 0x8000 || address > 0xFFFF) return false;
            
            mappedAddress = address & (_prgBanks > 1 ? 0x7FFF : 0x3FFF);
            return true;

        }
        
        public override bool cpuMapWrite(ushort address, ref int mappedAddress, ushort data)
        {
            if (address < 0x8000 || address > 0xFFFF) return false;
            
            mappedAddress = address & (_prgBanks > 1 ? 0x7FFFF : 0x3FFF);
            return true;
        }        
        
        public override bool ppuMapRead(ushort address, ref int mappedAddress)
        {
            mappedAddress = address;
            return address <= 0x1FFF;
        }        
        
        public override bool ppuMapWrite(ushort address, ref int mappedAddress)
        {
            if (_charBanks != 0) return false;
            mappedAddress = address;
            return true;

        }
    }
}