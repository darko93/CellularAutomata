using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    interface IStatedCellularAutomaton<TCellState>
    {
        TCellState GetCellState(int x, int y);
        void SetCellState(int x, int y, TCellState cellState);
        ColorValues GetColorValues(TCellState cellState);
        void SetColorValues(TCellState cellState, ColorValues colorValues);
    }
}
