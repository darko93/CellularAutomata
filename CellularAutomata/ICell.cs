using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    interface ICell<TCellState>
    {
        TCellState State { get; set; }
        ColorValues GetColorValues();
    }
}
