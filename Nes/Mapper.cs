namespace Nes
{ 
    public abstract class Mapper
    {
        internal readonly byte _prgBanks;
        internal readonly byte _charBanks;

        protected Mapper(byte prgBanks, byte charBanks)
        {
            _prgBanks = prgBanks;
            _charBanks = charBanks;
        }

        public virtual bool cpuMapRead(ushort address, ref int mappedAddress)
        {
            return false;
        }
        
        public virtual bool cpuMapWrite(ushort address, ref int mappedAddress, ushort data)
        {
            return false;
        }        
        
        public virtual bool ppuMapRead(ushort address, ref int mappedAddress)
        {
            return false;
        }        
        
        public virtual bool ppuMapWrite(ushort address, ref int mappedAddress)
        {
            return false;
        }
    }
}