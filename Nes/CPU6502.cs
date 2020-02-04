using System;
using System.Collections.Generic;

namespace Nes
{
	public class CPU6502
	{
		private Bus _bus;

		private byte _a = 0x00; // Accumulator
		private byte _x = 0x00; // X Register
		private byte _y = 0x00; // U Register
		private byte _stackPointer = 0x00; //Stack Pointer
		private byte _status = 0x00; // Status Register
		private ushort _pc = 0x0000; //Program Counter

		private byte _fetched = 0x0000;

		private ushort _address_abs = 0x0000;
		private ushort _address_rel = 0x0000;

		private byte _opcode = 00;
		private byte _cycles = 0x00;

		private List<Instruction> _instructions;

		public CPU6502()
		{ 
			_instructions = new List<Instruction>()
			{
				new Instruction() {name = "BRK", operate = BRK, addressMode = IMM, cycles = 7 }, new Instruction() {name =  "ORA", operate = ORA, addressMode = IZX, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 3 }, new Instruction() {name = "ORA", operate = ORA, addressMode =  ZP0, cycles = 3 }, new Instruction() {name = "ASL", operate = ASL, addressMode = ZP0, cycles = 5 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 5 }, new Instruction() {name = "PHP", operate = PHP, addressMode = IMP, cycles = 3 }, new Instruction() {name = "ORA", operate = ORA, addressMode = IMM, cycles = 2 }, new Instruction() {name = "ASL", operate = ASL, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "ORA", operate = ORA, addressMode = ABS, cycles = 4 }, new Instruction() {name = "ASL", operate = ASL, addressMode = ABS, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 6 },
				new Instruction() {name = "BPL", operate = BPL, addressMode = REL, cycles = 2 }, new Instruction() {name =  "ORA", operate = ORA, addressMode = IZY, cycles = 5 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "ORA", operate = ORA, addressMode =  ZPX, cycles = 4 }, new Instruction() {name = "ASL", operate = ASL, addressMode = ZPX, cycles = 6 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 6 }, new Instruction() {name = "CLC", operate = CLC, addressMode = IMP, cycles = 2 }, new Instruction() {name = "ORA", operate = ORA, addressMode = ABY, cycles = 4 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "ORA", operate = ORA, addressMode = ABX, cycles = 4 }, new Instruction() {name = "ASL", operate = ASL, addressMode = ABX, cycles = 7 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 },
				new Instruction() {name = "JSR", operate = JSR, addressMode = ABS, cycles = 6 }, new Instruction() {name =  "AND", operate = AND, addressMode = IZX, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "BIT", operate = BIT, addressMode = ZP0, cycles = 3 }, new Instruction() {name = "AND", operate = AND, addressMode =  ZP0, cycles = 3 }, new Instruction() {name = "ROL", operate = ROL, addressMode = ZP0, cycles = 5 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 5 }, new Instruction() {name = "PLP", operate = PLP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "AND", operate = AND, addressMode = IMM, cycles = 2 }, new Instruction() {name = "ROL", operate = ROL, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "BIT", operate = BIT, addressMode = ABS, cycles = 4 }, new Instruction() {name = "AND", operate = AND, addressMode = ABS, cycles = 4 }, new Instruction() {name = "ROL", operate = ROL, addressMode = ABS, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 6 },
				new Instruction() {name = "BMI", operate = BMI, addressMode = REL, cycles = 2 }, new Instruction() {name =  "AND", operate = AND, addressMode = IZY, cycles = 5 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "AND", operate = AND, addressMode =  ZPX, cycles = 4 }, new Instruction() {name = "ROL", operate = ROL, addressMode = ZPX, cycles = 6 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 6 }, new Instruction() {name = "SEC", operate = SEC, addressMode = IMP, cycles = 2 }, new Instruction() {name = "AND", operate = AND, addressMode = ABY, cycles = 4 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "AND", operate = AND, addressMode = ABX, cycles = 4 }, new Instruction() {name = "ROL", operate = ROL, addressMode = ABX, cycles = 7 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 },
				new Instruction() {name = "RTI", operate = RTI, addressMode = IMP, cycles = 6 }, new Instruction() {name =  "EOR", operate = EOR, addressMode = IZX, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 3 }, new Instruction() {name = "EOR", operate = EOR, addressMode =  ZP0, cycles = 3 }, new Instruction() {name = "LSR", operate = LSR, addressMode = ZP0, cycles = 5 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 5 }, new Instruction() {name = "PHA", operate = PHA, addressMode = IMP, cycles = 3 }, new Instruction() {name = "EOR", operate = EOR, addressMode = IMM, cycles = 2 }, new Instruction() {name = "LSR", operate = LSR, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "JMP", operate = JMP, addressMode = ABS, cycles = 3 }, new Instruction() {name = "EOR", operate = EOR, addressMode = ABS, cycles = 4 }, new Instruction() {name = "LSR", operate = LSR, addressMode = ABS, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 6 },
				new Instruction() {name = "BVC", operate = BVC, addressMode = REL, cycles = 2 }, new Instruction() {name =  "EOR", operate = EOR, addressMode = IZY, cycles = 5 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "EOR", operate = EOR, addressMode =  ZPX, cycles = 4 }, new Instruction() {name = "LSR", operate = LSR, addressMode = ZPX, cycles = 6 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 6 }, new Instruction() {name = "CLI", operate = CLI, addressMode = IMP, cycles = 2 }, new Instruction() {name = "EOR", operate = EOR, addressMode = ABY, cycles = 4 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "EOR", operate = EOR, addressMode = ABX, cycles = 4 }, new Instruction() {name = "LSR", operate = LSR, addressMode = ABX, cycles = 7 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 },
				new Instruction() {name = "RTS", operate = RTS, addressMode = IMP, cycles = 6 }, new Instruction() {name =  "ADC", operate = ADC, addressMode = IZX, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 3 }, new Instruction() {name = "ADC", operate = ADC, addressMode =  ZP0, cycles = 3 }, new Instruction() {name = "ROR", operate = ROR, addressMode = ZP0, cycles = 5 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 5 }, new Instruction() {name = "PLA", operate = PLA, addressMode = IMP, cycles = 4 }, new Instruction() {name = "ADC", operate = ADC, addressMode = IMM, cycles = 2 }, new Instruction() {name = "ROR", operate = ROR, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "JMP", operate = JMP, addressMode = IND, cycles = 5 }, new Instruction() {name = "ADC", operate = ADC, addressMode = ABS, cycles = 4 }, new Instruction() {name = "ROR", operate = ROR, addressMode = ABS, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 6 },
				new Instruction() {name = "BVS", operate = BVS, addressMode = REL, cycles = 2 }, new Instruction() {name =  "ADC", operate = ADC, addressMode = IZY, cycles = 5 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "ADC", operate = ADC, addressMode =  ZPX, cycles = 4 }, new Instruction() {name = "ROR", operate = ROR, addressMode = ZPX, cycles = 6 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 6 }, new Instruction() {name = "SEI", operate = SEI, addressMode = IMP, cycles = 2 }, new Instruction() {name = "ADC", operate = ADC, addressMode = ABY, cycles = 4 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "ADC", operate = ADC, addressMode = ABX, cycles = 4 }, new Instruction() {name = "ROR", operate = ROR, addressMode = ABX, cycles = 7 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 },
				new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name =  "STA", operate = STA, addressMode = IZX, cycles = 6 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 6 }, new Instruction() {name = "STY", operate = STY, addressMode = ZP0, cycles = 3 }, new Instruction() {name = "STA", operate = STA, addressMode =  ZP0, cycles = 3 }, new Instruction() {name = "STX", operate = STX, addressMode = ZP0, cycles = 3 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 3 }, new Instruction() {name = "DEY", operate = DEY, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "TXA", operate = TXA, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "STY", operate = STY, addressMode = ABS, cycles = 4 }, new Instruction() {name = "STA", operate = STA, addressMode = ABS, cycles = 4 }, new Instruction() {name = "STX", operate = STX, addressMode = ABS, cycles = 4 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 4 },
				new Instruction() {name = "BCC", operate = BCC, addressMode = REL, cycles = 2 }, new Instruction() {name =  "STA", operate = STA, addressMode = IZY, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 6 }, new Instruction() {name = "STY", operate = STY, addressMode = ZPX, cycles = 4 }, new Instruction() {name = "STA", operate = STA, addressMode =  ZPX, cycles = 4 }, new Instruction() {name = "STX", operate = STX, addressMode = ZPY, cycles = 4 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 4 }, new Instruction() {name = "TYA", operate = TYA, addressMode = IMP, cycles = 2 }, new Instruction() {name = "STA", operate = STA, addressMode = ABY, cycles = 5 }, new Instruction() {name = "TXS", operate = TXS, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 5 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 5 }, new Instruction() {name = "STA", operate = STA, addressMode = ABX, cycles = 5 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 5 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 5 },
				new Instruction() {name = "LDY", operate = LDY, addressMode = IMM, cycles = 2 }, new Instruction() {name =  "LDA", operate = LDA, addressMode = IZX, cycles = 6 }, new Instruction() {name = "LDX", operate = LDX, addressMode = IMM, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 6 }, new Instruction() {name = "LDY", operate = LDY, addressMode = ZP0, cycles = 3 }, new Instruction() {name = "LDA", operate = LDA, addressMode =  ZP0, cycles = 3 }, new Instruction() {name = "LDX", operate = LDX, addressMode = ZP0, cycles = 3 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 3 }, new Instruction() {name = "TAY", operate = TAY, addressMode = IMP, cycles = 2 }, new Instruction() {name = "LDA", operate = LDA, addressMode = IMM, cycles = 2 }, new Instruction() {name = "TAX", operate = TAX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "LDY", operate = LDY, addressMode = ABS, cycles = 4 }, new Instruction() {name = "LDA", operate = LDA, addressMode = ABS, cycles = 4 }, new Instruction() {name = "LDX", operate = LDX, addressMode = ABS, cycles = 4 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 4 },
				new Instruction() {name = "BCS", operate = BCS, addressMode = REL, cycles = 2 }, new Instruction() {name =  "LDA", operate = LDA, addressMode = IZY, cycles = 5 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 5 }, new Instruction() {name = "LDY", operate = LDY, addressMode = ZPX, cycles = 4 }, new Instruction() {name = "LDA", operate = LDA, addressMode =  ZPX, cycles = 4 }, new Instruction() {name = "LDX", operate = LDX, addressMode = ZPY, cycles = 4 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 4 }, new Instruction() {name = "CLV", operate = CLV, addressMode = IMP, cycles = 2 }, new Instruction() {name = "LDA", operate = LDA, addressMode = ABY, cycles = 4 }, new Instruction() {name = "TSX", operate = TSX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 4 }, new Instruction() {name = "LDY", operate = LDY, addressMode = ABX, cycles = 4 }, new Instruction() {name = "LDA", operate = LDA, addressMode = ABX, cycles = 4 }, new Instruction() {name = "LDX", operate = LDX, addressMode = ABY, cycles = 4 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 4 },
				new Instruction() {name = "CPY", operate = CPY, addressMode = IMM, cycles = 2 }, new Instruction() {name =  "CMP", operate = CMP, addressMode = IZX, cycles = 6 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "CPY", operate = CPY, addressMode = ZP0, cycles = 3 }, new Instruction() {name = "CMP", operate = CMP, addressMode =  ZP0, cycles = 3 }, new Instruction() {name = "DEC", operate = DEC, addressMode = ZP0, cycles = 5 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 5 }, new Instruction() {name = "INY", operate = INY, addressMode = IMP, cycles = 2 }, new Instruction() {name = "CMP", operate = CMP, addressMode = IMM, cycles = 2 }, new Instruction() {name = "DEX", operate = DEX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "CPY", operate = CPY, addressMode = ABS, cycles = 4 }, new Instruction() {name = "CMP", operate = CMP, addressMode = ABS, cycles = 4 }, new Instruction() {name = "DEC", operate = DEC, addressMode = ABS, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 6 },
				new Instruction() {name = "BNE", operate = BNE, addressMode = REL, cycles = 2 }, new Instruction() {name =  "CMP", operate = CMP, addressMode = IZY, cycles = 5 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "CMP", operate = CMP, addressMode =  ZPX, cycles = 4 }, new Instruction() {name = "DEC", operate = DEC, addressMode = ZPX, cycles = 6 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 6 }, new Instruction() {name = "CLD", operate = CLD, addressMode = IMP, cycles = 2 }, new Instruction() {name = "CMP", operate = CMP, addressMode = ABY, cycles = 4 }, new Instruction() {name = "NOP", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "CMP", operate = CMP, addressMode = ABX, cycles = 4 }, new Instruction() {name = "DEC", operate = DEC, addressMode = ABX, cycles = 7 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 },
				new Instruction() {name = "CPX", operate = CPX, addressMode = IMM, cycles = 2 }, new Instruction() {name =  "SBC", operate = SBC, addressMode = IZX, cycles = 6 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "CPX", operate = CPX, addressMode = ZP0, cycles = 3 }, new Instruction() {name = "SBC", operate = SBC, addressMode =  ZP0, cycles = 3 }, new Instruction() {name = "INC", operate = INC, addressMode = ZP0, cycles = 5 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 5 }, new Instruction() {name = "INX", operate = INX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "SBC", operate = SBC, addressMode = IMM, cycles = 2 }, new Instruction() {name = "NOP", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = SBC, addressMode = IMP, cycles = 2 }, new Instruction() {name = "CPX", operate = CPX, addressMode = ABS, cycles = 4 }, new Instruction() {name = "SBC", operate = SBC, addressMode = ABS, cycles = 4 }, new Instruction() {name = "INC", operate = INC, addressMode = ABS, cycles = 6 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 6 },
				new Instruction() {name = "BEQ", operate = BEQ, addressMode = REL, cycles = 2 }, new Instruction() {name =  "SBC", operate = SBC, addressMode = IZY, cycles = 5 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 8 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "SBC", operate = SBC, addressMode =  ZPX, cycles = 4 }, new Instruction() {name = "INC", operate = INC, addressMode = ZPX, cycles = 6 }, new Instruction() {name = "???", operate =  XXX, addressMode = IMP, cycles = 6 }, new Instruction() {name = "SED", operate = SED, addressMode = IMP, cycles = 2 }, new Instruction() {name = "SBC", operate = SBC, addressMode = ABY, cycles = 4 }, new Instruction() {name = "NOP", operate = NOP, addressMode = IMP, cycles = 2 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 }, new Instruction() {name = "???", operate = NOP, addressMode = IMP, cycles = 4 }, new Instruction() {name = "SBC", operate = SBC, addressMode = ABX, cycles = 4 }, new Instruction() {name = "INC", operate = INC, addressMode = ABX, cycles = 7 }, new Instruction() {name = "???", operate = XXX, addressMode = IMP, cycles = 7 }
			};

		}

		public void ConnectBus(Bus bus)
		{
			_bus = bus;
		}

		public void Reset() // Reset Interrupt - Forces CPU into known state
		{
			_pc = 0xFFFC;
			var (hi, li, hiLo) = Read16();
			_pc = hiLo;

			
			_a = 0;
			_x = 0;
			_y = 0;
			_stackPointer = 0xFD;
			_status = (byte) (0x00 | GetFlag(Flags6502.U)); //??????????????

			//var (hi, li, hiLo) = Read16();
			_address_abs = 0x0000;
			_address_rel = 0x0000;
			_fetched = 0x00;
			_cycles = 8;
		}

		public void Irq() // Interrupt Request - Executes an instruction at a specific location
		{
			if (GetFlag(Flags6502.I) != 0x00) return;
			
			Write(((ushort) (0x1000 + _stackPointer)), (byte) (_pc >> 8 & 0x00FF));
			_stackPointer--;
			Write(((ushort) (0x1000 + _stackPointer)), (byte) (_pc & 0x00FF));
			_stackPointer--;
				
			SetFlag(Flags6502.B, false);
			SetFlag(Flags6502.U, true);
			SetFlag(Flags6502.I, true);
				
			Write(((ushort) (0x1000 + _stackPointer)), _status);
			_status--;

			_pc = 0xFFFE;
			var (hi, li, hiLo) = Read16();
			_pc = hiLo;

			_cycles = 7;
		}

		public void Nmi() // Non-Maskable Interrupt Request - As above, but cannot be disabled
		{
			
				Write(((ushort) (0x1000 + _stackPointer)), (byte) (_pc >> 8 & 0x00FF));
				_stackPointer--;
				Write(((ushort) (0x1000 + _stackPointer)), (byte) (_pc & 0x00FF));
				_stackPointer--;
				
				SetFlag(Flags6502.B, false);
				SetFlag(Flags6502.U, true);
				SetFlag(Flags6502.I, true);
				
				Write(((ushort) (0x1000 + _stackPointer)), _status);
				_status--;

				_pc = 0xFFFA;
				var (hi, li, hiLo) = Read16();
				_pc = hiLo;

				_cycles = 8;
		}

		public void Clock() // Perform one clock cycle's worth of update
		{
			if (_cycles == 0)
			{
				_opcode = Read(_pc);
				SetFlag(Flags6502.U, true);
				_pc++;
				Console.WriteLine(_instructions[_opcode].name);
				var additionalCycle1 = _instructions[_opcode].addressMode();
				var additionalCycle2 = _instructions[_opcode].operate();
				_cycles += (byte)(additionalCycle1 & additionalCycle2);
				SetFlag(Flags6502.U, true);
				Console.WriteLine(
					"A: " + _a.ToString() + 
					" | X: " + _x.ToString() + 
					" | Y: " + _y.ToString() +
					" | Status: " + Convert.ToString(_status, 2));
				Console.WriteLine("Ram: " + _bus.CpuRam[0x0000] + " " + _bus.CpuRam[0x0001]+ " " + _bus.CpuRam[0x0002]+ " " + _bus.CpuRam[0x0004]);
			}

			_cycles--;
			

		}


		/***/

		private byte IMP()
		{
			_fetched = _a;
			return 0;
		}

		private byte IMM()
		{
			_address_abs = _pc++;
			return 0;
		}

		private byte ZP0()
		{
			_address_abs = Read(_pc);
			_pc++;
			_address_abs &= 0x00FF;
			return 0;
		}

		private byte ZPX()
		{
			_address_abs = (ushort) (Read(_pc) + _x);
			_pc++;
			_address_abs &= 0x00FF;
			return 0;		
		}

		private byte ZPY()
		{
			_address_abs = (ushort) (Read(_pc) + _y);
			_pc++;
			_address_abs &= 0x00FF;
			return 0;				
		}

		private byte REL()
		{
			_address_rel = Read(_pc);
			_pc++;
			if ((_address_rel & 0x80) == 0x80)
			{
				_address_rel |= 0xFF00;
			}
			return 0;
		}

		private byte ABS()
		{
			var (hi, lo, hiLo) = Read16();
			_address_abs = hiLo;
			return 0;
		}

		private byte ABX()
		{
			var (hi, lo, hiLo) = Read16();
			_address_abs = (ushort) (hiLo + _x);
			return (_address_abs & 0xFF00) != (hi << 8) ? (byte) 1 : (byte) 0;
		}

		private byte ABY()
		{
			var (hi, lo, hiLo) = Read16();
			_address_abs = (ushort) (hiLo + _y);
			return (_address_abs & 0xFF00) != (hi << 8) ? (byte) 1 : (byte) 0;		
		}

		private byte IND()
		{
			var (hi, lo, hiLo) = Read16();
			var hiAddress = (ushort) (hiLo + 1);
			
			// Simulate bug
			if (hiLo == 0x00FF)
			{
				hiAddress = (ushort) (hiLo & 0xFF00);
			}
			
			_address_abs = (ushort) ((Read(hiAddress) << 8) | Read(hiLo));
			
			return 0;
		}

		private byte IZX()
		{
			var (hi, lo, hiLo) = Read16(_x);
			_address_abs = hiLo;
			return 0;
		}

		private byte IZY()
		{
			var (hi, lo, hiLo) = Read16(0);
			_address_abs = (ushort) (hiLo + _y);
			return (_address_abs & 0xFF00) != (hi << 8) ? (byte) 1 : (byte) 0;		
		}

		/***/

		private byte ADC()
		{
			Fetch();
			var temp = _a + _fetched + GetFlag(Flags6502.C);
			var v = ~((_a ^ _fetched) & (_a ^ temp) & 0x0080) == 1;
			SetFlag(Flags6502.C, temp > 255);
			SetFlag(Flags6502.Z, (temp & 0x00FF) == 0);
			SetFlag(Flags6502.N, (temp & 0x80) == 0x80);
			SetFlag(Flags6502.V, v);
			_a = (byte) (temp & 0x00FF);
			return 1;
		}

		private byte AND()
		{
			Fetch();
			_a &= _fetched;
			SetFlag(Flags6502.Z, _a == 0x00);
			SetFlag(Flags6502.N, (_a & 0x80) == 0x80);
			return 1;
		}

		private byte ASL()
		{
			Fetch();
			var temp = _fetched << 1;
			SetFlag(Flags6502.C, (temp & 0xFF00) > 0);
			SetFlag(Flags6502.Z, (temp & 0x00FF) == 0x00);
			SetFlag(Flags6502.N, (temp & 0x80) == 0x80);
			var value = (byte) (temp & 0x00FF);
			if (_instructions[_opcode].addressMode == IMP)
				_a = value;
			else
				Write(_address_abs, value);
			return 0;
		}

		private byte BCC()
		{ 
			BranchIfClear(Flags6502.C);
			return 0;
		}

		private byte BCS()
		{
			BranchIfSet(Flags6502.C);
			return 0;
		}

		private byte BEQ()
		{
			BranchIfSet(Flags6502.Z);
			return 0;
		}

		private byte BIT()
		{
			return 0;
		}

		private byte BMI()
		{
			BranchIfSet(Flags6502.N);
			return 0;
		}

		private byte BNE()
		{
			BranchIfClear(Flags6502.Z);
			return 0;
		}

		private byte BPL()
		{
			BranchIfClear(Flags6502.N);
			return 0;
		}

		private byte BRK()
		{
			_pc++;
	
			SetFlag(Flags6502.I, true);
			Write((ushort) (0x0100 + _stackPointer), (byte) ((_pc >> 8) & 0x00FF));
			_stackPointer--;
			Write((ushort) (0x0100 + _stackPointer), (byte) (_pc & 0x00FF));
			_stackPointer--;

			SetFlag(Flags6502.B, true);
			Write((ushort) (0x0100 + _stackPointer), _status);
			_stackPointer--;
			SetFlag(Flags6502.B, false);

			_pc = (ushort) (Read(0xFFFE) | Read(0xFFFF) << 8);
			return 0;
		}

		private byte BVC()
		{
			BranchIfClear(Flags6502.V);
			return 0;
		}

		private byte BVS()
		{
			BranchIfSet(Flags6502.V);
			return 0;
		}

		private byte CLC()
		{
			SetFlag(Flags6502.C, false);
			return 0;
		}

		private byte CLD()
		{
			SetFlag(Flags6502.D, false);
			return 0;
		}

		private byte CLI()
		{
			SetFlag(Flags6502.I, false);
			return 0;
		}

		private byte CLV()
		{
			SetFlag(Flags6502.V, false);
			return 0;
		}

		private byte CMP()
		{
			Fetch();
			var temp = _a - _fetched;
			SetFlag(Flags6502.C, _a >= _fetched);
			SetFlag(Flags6502.Z, (temp & 0x00FF) == 0x0000);
			SetFlag(Flags6502.N, (temp & 0x0080) == 0x0080);
			return 1;
		}

		private byte CPX()
		{
			Fetch();
			var temp = _x - _fetched;
			SetFlag(Flags6502.C, _x >= _fetched);
			SetFlag(Flags6502.Z, (temp & 0x00FF) == 0x0000);
			SetFlag(Flags6502.N, (temp & 0x0080) == 0x0080);
			return 0;
		}

		private byte CPY()
		{
			Fetch();
			var temp = _y - _fetched;
			SetFlag(Flags6502.C, _y >= _fetched);
			SetFlag(Flags6502.Z, (temp & 0x00FF) == 0x0000);
			SetFlag(Flags6502.N, (temp & 0x0080) == 0x0080);
			return 0;
		}

		private byte DEC()
		{
			Fetch();
			var temp = _fetched - 1;
			Write(_address_abs, (byte) (temp & 0x00FF));
			SetFlag(Flags6502.Z, (temp & 0x00FF) == 0x0000);
			SetFlag(Flags6502.N, (temp & 0x0080) == 0x0080);
			return 0;		}

		private byte DEX()
		{
			_x--;
			SetFlag(Flags6502.Z, _x == 0x00);
			SetFlag(Flags6502.N, (_x & 0x80) == 0x0080);
			return 0;
		}

		private byte DEY()
		{
			_y--;
			SetFlag(Flags6502.Z, _y == 0x00);
			SetFlag(Flags6502.N, (_y & 0x80) == 0x0080);
			return 0;
		}

		private byte EOR()
		{
			Fetch();
			_a = (byte) (_a ^ _fetched);	
			SetFlag(Flags6502.Z, _a == 0x00);
			SetFlag(Flags6502.N, (_a & 0x80) == 0x0080);
			return 1;		
		}

		private byte INC()
		{
			Fetch();
			var temp = _fetched + 1;
			Write(_address_abs, (byte) (temp & 0x00FF));
			SetFlag(Flags6502.Z, (temp & 0x00FF) == 0x0000);
			SetFlag(Flags6502.N, (temp & 0x0080) == 0x0080);
			return 0;		}

		private byte INX()
		{
			_x++;
			SetFlag(Flags6502.Z, _x == 0x00);
			SetFlag(Flags6502.N, (_x & 0x80) ==0x0080);
			return 0;		
		}

		private byte INY()
		{
			_y++;
			SetFlag(Flags6502.Z, _y == 0x00);
			SetFlag(Flags6502.N, (_y & 0x80) == 0x0080);
			return 0;
		}

		private byte JMP()
		{
			_pc = _address_abs;
			return 0;
		}

		private byte JSR()
		{
			_pc--;

			Write((ushort) (0x0100 + _stackPointer), (byte) ((_pc >> 8) & 0x00FF));
			_stackPointer--;
			Write((ushort) (0x0100 + _stackPointer), (byte) (_pc & 0x00FF));
			_stackPointer--;

			_pc = _address_abs;
			return 0;
		}

		private byte LDA()
		{
			Fetch();
			_a = _fetched;
			SetFlag(Flags6502.Z, _a == 0x00);
			SetFlag(Flags6502.N, (_a & 0x80) == 0x0080);
			return 1;
		}

		private byte LDX()
		{
			Fetch();
			_x = _fetched;
			SetFlag(Flags6502.Z, _x == 0x00);
			SetFlag(Flags6502.B, (_x & 0x80) == 0x0080);
			return 1;		
		}

		private byte LDY()
		{
			Fetch();
			_y = _fetched;
			SetFlag(Flags6502.Z, _y == 0x00);
			SetFlag(Flags6502.B, (_y & 0x80) == 0x0080);
			return 1;	
		}

		private byte LSR()
		{
			return 0;
		}

		private byte NOP()
		{
			return 0;
		}

		private byte ORA()
		{
			return 0;
		}

		private byte PHA()
		{
			Write((ushort) (0x0100 + _stackPointer), _a);
			_stackPointer--;
			return 0;		
		}

		private byte PHP()
		{
			return 0;
		}

		private byte PLA()
		{
			_stackPointer++;
			_a = Read((ushort) (0x0100 + _stackPointer));
			SetFlag(Flags6502.Z, _a == 0x00);
			SetFlag(Flags6502.N, (_a & 0x80) == 0x80);
			return 0;
		}

		private byte PLP()
		{
			return 0;
		}

		private byte ROL()
		{
			return 0;
		}

		private byte ROR()
		{
			return 0;
		}

		private byte RTI()
		{
			_stackPointer++;
			_status = Read((ushort) (0x0100 + _stackPointer));
			_status &= unchecked((byte) ~Flags6502.B); //????????
			_status &= unchecked((byte) ~Flags6502.U); //????????

			_stackPointer++;
			_pc = Read((ushort) (0x0100 + _stackPointer));
			_stackPointer++;
			_pc |= (ushort) (Read((ushort) (0x0100 + _stackPointer)) << 8);
			return 0;
		}

		private byte RTS()
		{
			return 0;
		}

		private byte SBC()
		{
			Fetch();
			var value = _fetched ^ 0x00FF;
			var temp = _a + value + GetFlag(Flags6502.C);
			var v = ~((_a ^ value) & (value ^ temp) & 0x0080) == 1;
			SetFlag(Flags6502.C, (temp & 0xFF00) == 0xFF00);
			SetFlag(Flags6502.Z, (temp & 0x00FF) == 0);
			SetFlag(Flags6502.N, (temp & 0x80) == 0x80);
			SetFlag(Flags6502.V, v);
			_a = (byte) (temp & 0x00FF);
			return 1;
		}

		private byte SEC()
		{
			return 0;
		}

		private byte SED()
		{
			return 0;
		}

		private byte SEI()
		{
			return 0;
		}

		private byte STA()
		{
			return 0;
		}

		private byte STX()
		{
			Write(_address_abs, _x);
			return 0;
		}

		private byte STY()
		{
			return 0;
		}

		private byte TAX()
		{
			return 0;
		}

		private byte TAY()
		{
			return 0;
		}

		private byte TSX()
		{
			return 0;
		}

		private byte TXA()
		{
			return 0;
		}

		private byte TXS()
		{
			return 0;
		}

		private byte TYA()
		{
			return 0;
		}

		// I capture all "unofficial" opcodes with this function. It is
		// functionally identical to a NOP
		private byte XXX()
		{
			return 0;
		}

		private byte Read(ushort address)
		{
			return _bus.CpuRead(address);
		}

		private (byte hi, byte lo, ushort hiLo) Read16(byte offSetBy)
		{
			ushort temp = Read(_pc);
			_pc++;
			
			var lo = Read((ushort) ((temp + offSetBy) & 0x00FF));
			_pc++;
			var hi =  Read((ushort) ((temp + offSetBy + 1) & 0xFF00));
			_pc++;

			var hiLo = (ushort) ((hi << 8) | lo);
			return (hi, lo, hiLo);
		}


		private (byte hi, byte lo, ushort hiLo) Read16()
		{
			var lo = Read(_pc);
			_pc++;
			var hi = Read(_pc);
			_pc++;

			var hiLo = (ushort) ((hi << 8) | lo);
			return (hi, lo, hiLo);
		}

		private void Write(ushort address, byte data)
		{
			_bus.CpuWrite(address, data);
		}

		private byte Fetch()
		{
			if (_instructions[_opcode].addressMode != IMP)
			{
				_fetched = Read(_address_abs);
			}

			return _fetched;
		}

		private void BranchIfSet(Flags6502 f)
		{
			if (GetFlag(f) != 1) return;
			
			_cycles++;
			_address_abs = (ushort) (_pc + _address_rel);
			if ((_address_abs & 0xFF00) != (_pc & 0xFF00))
			{
				_cycles++;
			}

			_pc = _address_abs;
		}

		public bool Complete()
		{
			return _cycles == 0;
		}
		
		private void BranchIfClear(Flags6502 f)
		{
			if (GetFlag(f) != 0) return;
			
			_cycles++;
			_address_abs = (ushort) (_pc + _address_rel);
			if ((_address_abs & 0xFF00) != (_pc & 0xFF00))
			{
				_cycles++;
			}

			_pc = _address_abs;
		}
		

		private void SetFlag(Flags6502 f, bool v)
		{
			if (v)
			{
				_status |= (byte) f;
			}
			else
			{
				_status &= (byte) ~f;
			}
		}

		private byte GetFlag(Flags6502 f)
		{
			return (byte) ((_status & (byte)f) == 1 ? 1 : 0);
		}


		private enum Flags6502
		{
			C = (1 << 0),
			Z = (1 << 1),
			I = (1 << 2),
			D = (1 << 3),
			B = (1 << 4),
			U = (1 << 5),
			V = (1 << 6),
			N = (1 << 7)
		}

		private struct Instruction
		{
			private Instruction(string name, Func<byte> operate, Func<byte> addressMode, byte cyles)
			{
				this.name = name;
				this.operate = operate;
				this.addressMode = addressMode;
				this.cycles = cyles;
			}

			public string name;
			public Func<byte> operate;
			public Func<byte> addressMode;
			public byte cycles;
		}

		public Dictionary<ushort, string> Disassemble(ushort start, ushort stop)
		{
			var address =  start;
			byte value, hi, lo = 0x00;
			var lineAddress = 0;
			var mapLines = new Dictionary<ushort, string>();
			var max = -1;
			while (address <= stop - 1)
			{
				if (max < address)
				{
					max = address;
				}
				else
				{
					break;
				}

				lineAddress = address;
				var inst = "$" + Convert.ToString(address, 16) + ":";
				var opcode = _bus.CpuRead(address, true);
				address++;
				inst += _instructions[opcode].name + " ";
				Console.WriteLine(_instructions[opcode].name);
				if (_instructions[opcode].addressMode == IMP)
				{
					inst += " {IMP}";
				}
				else if (_instructions[opcode].addressMode == IMM)
				{
					value = _bus.CpuRead(address, true); 
					address++;
					inst += "#$" + Convert.ToString(value, 16) + " {IMM}";
				}
				else if (_instructions[opcode].addressMode == ZP0)
				{
					lo = _bus.CpuRead(address, true); 
					address++;
					hi = 0x00;												
					inst += "$" + Convert.ToString(lo, 16) + " {ZP0}";
				}
				else if (_instructions[opcode].addressMode == ZPX)
				{
					lo = _bus.CpuRead(address, true); 
					address++;
					hi = 0x00;														
					inst += "$" + Convert.ToString(lo, 16) + ", X {ZPX}";
				}
				else if (_instructions[opcode].addressMode == ZPY)
				{
					lo = _bus.CpuRead(address, true); 
					address++;
					hi = 0x00;														
					inst += "$" + Convert.ToString(lo, 16) + ", Y {ZPY}";
				}
				else if (_instructions[opcode].addressMode == IZX)
				{
					lo = _bus.CpuRead(address, true); 
					address++;
					hi = 0x00;								
					inst += "($" + Convert.ToString(lo, 16) + ", X) {IZX}";
				}
				else if (_instructions[opcode].addressMode == IZY)
				{
					lo = _bus.CpuRead(address, true); 
					address++;
					hi = 0x00;								
					inst += "($" + Convert.ToString(lo, 16) + "), Y {IZY}";
				}
				else if (_instructions[opcode].addressMode == ABS)
				{
					lo = _bus.CpuRead(address, true); 
					address++;
					hi = _bus.CpuRead(address, true); 
					address++;
					inst += "$" + Convert.ToString((hi << 8) | lo, 16) + " {ABS}";
				}
				else if (_instructions[opcode].addressMode == ABX)
				{
					lo = _bus.CpuRead(address, true); 
					address++;
					hi = _bus.CpuRead(address, true); 
					address++;
					inst += "$" + Convert.ToString((hi << 8) | lo, 16) + ", X {ABX}";
				}
				else if (_instructions[opcode].addressMode == ABY)
				{
					lo = _bus.CpuRead(address, true); 
					address++;
					hi = _bus.CpuRead(address, true); 
					address++;
					inst += "$" + Convert.ToString((hi << 8) | lo, 16)+ ", Y {ABY}";
				}
				else if (_instructions[opcode].addressMode == IND)
				{
					lo = _bus.CpuRead(address, true); 
					address++;
					hi = _bus.CpuRead(address, true); 
					address++;
					inst += "($" + Convert.ToString((hi << 8) | lo, 16) + ") {IND}";
				}
				else if (_instructions[opcode].addressMode == REL)
				{
					value = _bus.CpuRead(address, true); 
					address++;
					inst += "$" + Convert.ToString(value, 16) + " [$" + Convert.ToString(address + value, 16) + "] {REL}";
				}
				mapLines[(ushort) (lineAddress)] = inst;
			}

			return mapLines;
		}
	}
}
