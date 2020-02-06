
using System;
using PixelEngine;

namespace Nes
{
    public class PPU2c02
    {
        private Cartridge _cartridge;
        private Control _control;
        private Mask _mask;
        private Status _status;
        private int _cycle;
        private int _scanLine;
        private bool _frameComplete;
        private byte _fineX;
        
        private  byte [,] _tablePattern;
        private  byte [] _tablePalette; // Unnecessary for NES emulator

        private Sprite[] _spritePatternTable;

        private ushort _addressLatch = 0x00;
        private ushort _ppuDataBuffer = 0x00;
        private ushort _ppuAddress = 0x0000;
        
        private Pixel[] _palScreen;
        private Sprite _screen;
        private Sprite[] _nameTable;
        private Sprite[] _patternTable; // Unnecessary for NES emulator

        private LoopyRegister _vRamAddress;
        private LoopyRegister _tRamAddress;
        
        public bool FrameComplete = false;

        public PPU2c02()
        {
	        var _screen = new Sprite(256, 240);
	        var _nameTable = new Sprite[2]
	        {
		        new Sprite(256, 240),
		        new Sprite(256, 240)
	        };           
	        var _patternTable = new Sprite[2]
	        {
		        new Sprite(128, 128),
		        new Sprite(128, 128)
	        };
	        
	        _tablePalette = new byte[32];

	        _tablePattern = new byte[2, 4096];
	        _spritePatternTable = new Sprite[2]
	        {
		        new Sprite(128, 128),
		        new Sprite(128, 128)
	        };

	        var _palScreen = new Pixel[0x40]
	        {
		        new Pixel(84, 84, 84),
		        new Pixel(0, 30, 116),
		        new Pixel(8, 16, 144),
		        new Pixel(48, 0, 136),
		        new Pixel(68, 0, 100),
		        new Pixel(92, 0, 48),
		        new Pixel(84, 4, 0),
		        new Pixel(60, 24, 0),
		        new Pixel(32, 42, 0),
		        new Pixel(8, 58, 0),
		        new Pixel(0, 64, 0),
		        new Pixel(0, 60, 0),
		        new Pixel(0, 50, 60),
		        new Pixel(0, 0, 0),
		        new Pixel(0, 0, 0),
		        new Pixel(0, 0, 0),

		        new Pixel(152, 150, 152),
		        new Pixel(8, 76, 196),
		        new Pixel(48, 50, 236),
		        new Pixel(92, 30, 228),
		        new Pixel(136, 20, 176),
		        new Pixel(160, 20, 100),
		        new Pixel(152, 34, 32),
		        new Pixel(120, 60, 0),
		        new Pixel(84, 90, 0),
		        new Pixel(40, 114, 0),
		        new Pixel(8, 124, 0),
		        new Pixel(0, 118, 40),
		        new Pixel(0, 102, 120),
		        new Pixel(0, 0, 0),
		        new Pixel(0, 0, 0),
		        new Pixel(0, 0, 0),

		        new Pixel(236, 238, 236),
		        new Pixel(76, 154, 236),
		        new Pixel(120, 124, 236),
		        new Pixel(176, 98, 236),
		        new Pixel(228, 84, 236),
		        new Pixel(236, 88, 180),
		        new Pixel(236, 106, 100),
		        new Pixel(212, 136, 32),
		        new Pixel(160, 170, 0),
		        new Pixel(116, 196, 0),
		        new Pixel(76, 208, 32),
		        new Pixel(56, 204, 108),
		        new Pixel(56, 180, 204),
		        new Pixel(60, 60, 60),
		        new Pixel(0, 0, 0),
		        new Pixel(0, 0, 0),

		        new Pixel(236, 238, 236),
		        new Pixel(168, 204, 236),
		        new Pixel(188, 188, 236),
		        new Pixel(212, 178, 236),
		        new Pixel(236, 174, 236),
		        new Pixel(236, 174, 212),
		        new Pixel(236, 180, 176),
		        new Pixel(228, 196, 144),
		        new Pixel(204, 210, 120),
		        new Pixel(180, 222, 120),
		        new Pixel(168, 226, 144),
		        new Pixel(152, 226, 180),
		        new Pixel(160, 214, 228),
		        new Pixel(160, 162, 160),
		        new Pixel(0, 0, 0),
		        new Pixel(0, 0, 0),
	        };
        }

        public void ConnectCartridge(Cartridge cartridge)
        {
            _cartridge = cartridge;
        }

        public void Clock()
        {
	        var random = new Random();
	        _screen[_cycle - 1, _scanLine] = _palScreen[random.Next(0,0x40)];
	        //Double check
	        _cycle++;
	        if (_cycle < 341) return;
	        _cycle = 0;
	        _scanLine++;
	        if (_scanLine < 261) return;
	        _scanLine = -1;
	        _frameComplete = true;
        }

        public byte CpuRead(ushort address, bool readOnly = false)
        {
	        ushort data = 0x00;
	        if (readOnly)
	        {
		        switch (address)
		        {

			        case 0x0000: // Control
				        data = _control.reg;
				        break;
			        case 0x0001: // Mask
				        data = _mask.reg;
				        break;
			        case 0x0002: // Status
				        data = _status.reg;
				        break;
			        case 0x0003: // OAM Address
				        break;
			        case 0x0004: // OAM Data
				        break;
			        case 0x0005: // Scroll
				        break;
			        case 0x0006: // PPU Address
				        break;
			        case 0x0007: // PPU Data
				        break;
		        }
	        }
	        else
	        {
		        switch (address)
		        {
			        case 0x0000: // Control
				        break;
			        case 0x0001: // Mask
				        break;
			        case 0x0002: // Status
				        data = (ushort) ((_status.reg & 0xE0) | (_ppuDataBuffer & 0x1F));
				        _status.SetFlag(Status.Flags.VerticalBlank, false);
				        _addressLatch = 0;
				        break;
			        case 0x0003: // OAM Address
				        break;
			        case 0x0004: // OAM Data
				        break;
			        case 0x0005: // Scroll
				        break;
			        case 0x0006: // PPU Address
				        break;
			        case 0x0007: // PPU Data
				        data = _ppuDataBuffer;
				        _ppuDataBuffer = PpuRead(_vRamAddress.reg);

				        if (_vRamAddress.reg > 0x3F00)
				        {
					        data = _ppuDataBuffer;
				        }

				        var inclementModeFlagValue = _control.GetFlag(Control.Flags.IncrementMode);
				        if (inclementModeFlagValue)
				        {
					        _vRamAddress.reg += 1;
				        }
				        else
				        {
					        _vRamAddress.reg += 32;
				        }

				        break;
		        }
	        }

	        return 0;
        }

        public void CpuWrite(ushort address, byte data)
        {
            switch (address)
            {
                case 0x0000: // Control
	                _control.reg = data;
	                _tRamAddress.SetFlag(LoopyRegister.Flags.NameTableX, _control.GetFlag(Control.Flags.NameTableX));
	                _tRamAddress.SetFlag(LoopyRegister.Flags.NameTableY, _control.GetFlag(Control.Flags.NameTableY));
	                break;
                case 0x0001: // Mask
	                _mask.reg = data;
	                break;
                case 0x0002: // Status
                    break;
                case 0x0003: // OAM Address
                    break;
                case 0x0004: // OAM Data
                    break;
                case 0x0005: // Scroll
	                if (_addressLatch == 0)
	                {
		                _fineX =  (byte) (data & 0x07);
		                _tRamAddress.SetCoarseX((byte) (data >> 3));
		                _ppuAddress = (ushort) ((_ppuAddress & 0xFF00) | data << 8); 
		                _addressLatch = 1;
	                }
	                else
	                {
		                _tRamAddress.SetFineY( (byte) (data & 0x07));
		                _tRamAddress.SetCoarseY((byte)(data >> 3));
		                _addressLatch = 0;
	                }
                    break;
                case 0x0006: // PPU Address
	                if (_addressLatch == 0)
	                {
		                _tRamAddress.reg = (ushort) ((data & 0x3F) << 8 | _tRamAddress.reg & 0x00FF);
		                _addressLatch = 1;
	                }
	                else
	                {
		                _tRamAddress.reg = (ushort) ( _tRamAddress.reg & 0xFF00 | data);
		                _vRamAddress = _tRamAddress;
		                _addressLatch = 0;
	                }
	                break;
                case 0x0007: // PPU Data
	                PpuWrite(_vRamAddress.reg, data);
	                var inclementModeFlagValue = _control.GetFlag(Control.Flags.IncrementMode);
	                if (inclementModeFlagValue)
	                {
		                _vRamAddress.reg += 1;
	                }
	                else
	                {
		                _vRamAddress.reg += 32;
	                }
                    break;
            }
        }
        
        public byte PpuRead(ushort address, bool readOnly = false)
        {
            byte data = 0x00;
            address &= 0x3FFF;
            
            if (_cartridge.CpuWrite(address, data))// Possible remove
            {
                
            } else if ( address <= 0x1FFF ) //Pattern memory
            {
	            data = _tablePattern[(address & 0x1000) >> 12, address & 0x0FFF];
            } else if ( address >= 0x2000 &&  address <= 0x3EFF ) //Name table memory
            {
	            
            } else if ( address >= 0x3F00 &&  address <= 0x3FFF ) //Palette memory
            {
	            address &= 0x001f;
	            if (address == 0x0010) address = 0x0000;
	            if (address == 0x0014) address = 0x0004;
	            if (address == 0x0018) address = 0x0008;
	            if (address == 0x001C) address = 0x000C;
	            data = _tablePalette[address];
            }
            
            return data;
        }

        public void PpuWrite(ushort address, byte data)
        {
            address &= 0x3FFF;

            if (_cartridge.CpuWrite(address, data))// Possible remove
            {
                
            } else if ( address <= 0x1FFF ) //Pattern memory
            {
	            _tablePattern[(address & 0x1000) >> 12, address & 0x0FFF] = data;
            } else if ( address >= 0x2000 &&  address <= 0x3EFF ) //Name table memory
            {
	            
            } else if ( address >= 0x3F00 &&  address <= 0x3FFF ) //Palette memory
            {
	            address &= 0x001f;
	            if (address == 0x0010) address = 0x0000;
	            if (address == 0x0014) address = 0x0004;
	            if (address == 0x0018) address = 0x0008;
	            if (address == 0x001C) address = 0x000C;
	            _tablePalette[address] = data;

            }
        }
        
        public Sprite GetPatternTable(ushort i, ushort palette)
        {
            for (ushort titleY = 0; titleY < 16; titleY++)
            {
                for (ushort titleX = 0; titleX < 16; titleX++)
                {
                    var offSet = titleY * 256 + titleX * 16;
                    for (ushort row = 0; row < 8; row++)
                    {
                        ushort title_lsb = PpuRead((ushort) (i * 0x1000 + offSet + row));
                        ushort title_msb = PpuRead((ushort) (i * 0x1000 + offSet + row + 8));
                        
                        for (ushort col = 0; col < 8; col++)
                        {
                            ushort pixel = (ushort) ((title_lsb & 0x01) + (title_msb & 0x01));
                            title_lsb >>= 1;
                            title_msb >>= 1;
                            var s = new Sprite(2,2);
                            
                            _spritePatternTable[i][titleX * 8 + (7 - col), titleY * 8 + row] =
                                GetColorFromPaletteRam(palette, pixel);

                        }
                    }
                }
            }   
            return _spritePatternTable[i];

        }

        private Pixel GetColorFromPaletteRam(ushort palette, in ushort pixel)
        {
            return _palScreen[PpuRead((ushort) (0x3F00 + (palette << 2) + pixel))];
        }
        
        public Sprite GetScreen()
        {
	        return _screen;
        }

        public Sprite GetNameTable(byte index)
        {
	        return _nameTable[index];
        }        
        
        public Sprite GetPatternTable(byte index)
        {
	        return _patternTable[index];
        }

    }
}


public struct Control
{
	public byte reg;

	public enum Flags
	{
		NameTableX = (1 << 0),
		NameTableY = (1 << 1),
		IncrementMode = (1 << 2),
		PatternSprite = (1 << 3),
		PatternBackground = (1 << 4),
		SpriteSize = (1 << 5),
		SlaveMode = (1 << 6),
		EnableNmi = (1 << 7)
	}

	public bool GetFlag(Flags f)
	{
		return (reg & (byte) f) == (byte) f;
	}
}

public struct Status
{
	public byte reg;
	public enum Flags
	{
		Unused1 = (1 << 0),
		Unused2 = (1 << 1),
		Unused3 = (1 << 2),
		Unused4 = (1 << 3),
		Unused5 = (1 << 4),
		SpriteOverflow = (1 << 5),
		SpriteZeroHit = (1 << 6),
		VerticalBlank = (1 << 7)
	}
	
	public byte GetFlag(Flags f)
	{
		return (byte) ((reg & (byte)f) == 1 ? 1 : 0);
	}
	
	public void SetFlag(Flags f, bool v)
	{
		if (v)
		{
			reg |= (byte) f;
		}
		else
		{
			reg &= (byte) ~f;
		}	
	}
}

public struct Mask
{
	public byte reg;
	private enum Flags
	{
		GrayScale = (1 << 0),
		RenderBackgroundLeft = (1 << 1),
		RenderSpritesLeft = (1 << 2),
		RenderBackGround = (1 << 3),
		RenderSprites = (1 << 4),
		EnhanceRed = (1 << 5),
		EnhanceGreen = (1 << 6),
		EnhanceBlue = (1 << 7)
	}
	
	private byte GetFlag(Flags f)
	{
		return (byte) ((reg & (byte)f) == 1 ? 1 : 0);
	}
}

public struct LoopyRegister
{
	public ushort reg;
	public enum Flags
	{
		CoarseX1 = (1 << 0),
		CoarseX3 = (1 << 2),
		CoarseX2 = (1 << 1),
		CoarseX4 = (1 << 3),
		CoarseX5 = (1 << 4),
		CoarseY1 = (1 << 5),
		CoarseY2 = (1 << 6),
		CoarseY3 = (1 << 7),
		CoarseY4 = (1 << 8),
		CoarseY5 = (1 << 9),
		NameTableX = (1 << 10),
		NameTableY = (1 << 11),
		FineY1 = (1 << 12),
		FineY2 = (1 << 13),
		FineY3 = (1 << 14),
		Unused = (1 << 15)
	}

	public byte GetCoarseX()
	{
		var result = (byte) LoopyRegister.Flags.CoarseX1;
		result <<=  (byte) LoopyRegister.Flags.CoarseX2;
		result <<=  (byte) LoopyRegister.Flags.CoarseX3;
		result <<=  (byte) LoopyRegister.Flags.CoarseX4;
		result <<=  (byte) LoopyRegister.Flags.CoarseX5;
		return result;
	}

	public void SetCoarseX(byte value)
	{
		reg &= 0xFFE0;
		reg |= (ushort) (value & 0x1F);
	}

	public void SetCoarseY(byte value)
	{
		reg &= 0xFC1F;
		reg |= (ushort) (value & 0x3E0);
	}
	public void SetFineY(byte value)
	{
		reg &= 0x8FFF;
		value <<= 12;
		reg |= (ushort) (value & 0x7000);
	}
	
	public void SetFlag(Flags f, bool v)
	{
		if (v)
		{
			reg |= (byte) f;
		}
		else
		{
			reg &= (byte) ~f;
		}	
	}
	
	private byte GetFlag(Flags f)
	{
		return (byte) ((reg & (byte)f) == 1 ? 1 : 0);
	}
}

