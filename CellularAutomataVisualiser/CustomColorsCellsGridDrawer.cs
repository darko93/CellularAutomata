using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using CellularAutomata;

namespace CellularAutomataVisualiser
{
    class CustomColorsCellsGridDrawer<TCellState>
    {
        private Dictionary<TCellState, Color> colors = null;

        public void SetColors(Dictionary<TCellState, Color> colors)
        {
            if (colors.Count < Enum.GetNames(typeof(TCellState)).Length)
                throw new ArgumentException("Not enough colors in dictionary.");

            this.colors = colors;
        }

        public CustomColorsCellsGridDrawer(Dictionary<TCellState, Color> colors)
        {
            SetColors(colors);
        }
    }
}
