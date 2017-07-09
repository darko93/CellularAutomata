using System;

namespace CellularAutomata
{
    [Flags]
    public enum FhpParticleCellState : byte
    {
        None = 0x0, // 0
        Northeast = 0x1, // 1
        East = 0x2, // 2        
        Southeast = 0x4, // 4
        Southwest = 0x8, // 8
        West = 0x10, // 16
        Northwest = 0x20, // 32
        Rest = 0x40, // 64
        Wall = 0x80, // 128
    }
}
