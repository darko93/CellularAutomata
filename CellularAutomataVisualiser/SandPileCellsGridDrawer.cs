using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using CellularAutomata;

namespace CellularAutomataVisualiser
{
    class SandPileCellsGridDrawer : CustomColorsCellsGridDrawer<SandPileCellState>
    {
        private static Dictionary<SandPileCellState, Color> Colors
        {
            get
            {
                Dictionary<SandPileCellState, Color> colors = new Dictionary<SandPileCellState, Color>();
                Color airColor = Color.FromArgb(ColorValues.Air.ToInt());
                Color sandColor = Color.FromArgb(ColorValues.Sand.ToInt());
                Color wallColor = Color.FromArgb(ColorValues.Wall.ToInt());
                colors.Add(SandPileCellState.Empty, airColor);
                colors.Add(SandPileCellState.SandGrain, sandColor);
                colors.Add(SandPileCellState.Wall, wallColor);
                return colors;
            }
        }

        public SandPileCellsGridDrawer(Dictionary<SandPileCellState, Color> colors) 
            : base(colors) { }

        public SandPileCellsGridDrawer()
            : base (Colors) { }
    }
}
