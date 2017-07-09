using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using CellularAutomata;

namespace CellularAutomataVisualiser
{
    class CellsGridDrawer
    {
        public CellularAutomata.Point UpperLeftBound { get; private set; }
        public CellularAutomata.Point LowerRightBound { get; private set; }
        public int CellSize { get; set; }
        public ICellularAutomaton CellularAutomaton { get; set; }

        private SolidBrush solidBrush = new SolidBrush(Color.Transparent);

        public CellsGridDrawer(ICellularAutomaton cellularAutomaton, int cellSize, CellularAutomata.Point upperLeftBound, CellularAutomata.Point lowerRightBound)
        {
            CellularAutomaton = cellularAutomaton;
            CellSize = cellSize;
            UpperLeftBound = upperLeftBound;
            LowerRightBound = lowerRightBound;
        }

        public void Draw(Graphics graphics)
        {
            for (int x = UpperLeftBound.X; x <= LowerRightBound.X; x++)
            {
                for (int y = UpperLeftBound.Y; y <= LowerRightBound.Y; y++)
                {
                    ColorValues cellXYColorValues = CellularAutomaton.GetColorValues(x, y);
                    Color cellXYColor = Color.FromArgb(cellXYColorValues.ToArgb());
                    solidBrush.Color = cellXYColor;
                    graphics.FillRectangle(solidBrush, (x - UpperLeftBound.X) * CellSize, (y - UpperLeftBound.Y) * CellSize, CellSize, CellSize);
                }
            }
        }
    }
}
