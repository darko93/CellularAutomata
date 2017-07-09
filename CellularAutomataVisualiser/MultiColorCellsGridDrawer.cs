using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CellularAutomata;
using System.Drawing;

namespace CellularAutomataVisualiser
{
    class MultiColorCellsGridDrawer
    {
        private SolidBrush brush = new SolidBrush(Color.Transparent);

        private ICellularAutomaton cellularAutomaton;
        private int cellWidth;
        private int cellHeight;

        public void SetCellularAutomaton(ICellularAutomaton cellularAutomaton) =>
            this.cellularAutomaton = cellularAutomaton;

        public void SetCellWidth(int cellWidth) =>
            this.cellWidth = cellWidth;

        public void SetCellHeight(int cellHeight) =>
            this.cellHeight = cellHeight;

        public MultiColorCellsGridDrawer(ICellularAutomaton cellularAutomaton, int cellWidth, int cellHeight)
        {
            this.cellularAutomaton = cellularAutomaton;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
        }
        
        public void Draw(Graphics graphics)
        {
            for (int cellX = 0; cellX < cellularAutomaton.Width; cellX++)
            {
                for (int cellY = 0; cellY < cellularAutomaton.Height; cellY++)
                {
                    ColorValues cellColorValues = cellularAutomaton.GetColorValues(cellX, cellY);
                    Color cellColor = Color.FromArgb(cellColorValues.ToArgb());
                    brush.Color = cellColor;
                    Rectangle cellRectangle = new Rectangle(cellX * cellWidth, cellY * cellHeight, cellWidth, cellHeight);
                    graphics.FillRectangle(brush, cellRectangle);
                }
            }
        }
    }
}
