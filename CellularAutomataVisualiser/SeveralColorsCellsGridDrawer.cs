using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using CellularAutomata;

namespace CellularAutomataVisualiser
{
    class SeveralColorsCellsGridDrawer
    {
        private SolidBrush brush = new SolidBrush(Color.Transparent);

        private Dictionary<ColorValues, List<Rectangle>> rectangles = new Dictionary<ColorValues, List<Rectangle>>();

        private ICellularAutomaton cellularAutomaton;

        public ICellularAutomaton CellularAutomaton
        {
            get { return cellularAutomaton; }
            set { SetCellularAutomaton(value); }
        }

        private void SetCellularAutomaton(ICellularAutomaton cellularAutomaton)
        {
            rectangles.Clear();
            ColorValues[] cellsColors = cellularAutomaton.GetCellsColors();
            foreach (ColorValues cellColor in cellsColors)
                rectangles.Add(cellColor, new List<Rectangle>());

            this.cellularAutomaton = cellularAutomaton;
        }

        public int CellWidth { get; set; }
        public int CellHeight { get; set; }

        public SeveralColorsCellsGridDrawer(ICellularAutomaton cellularAutomaton, int cellWidth, int cellHeight)
        {
            SetCellularAutomaton(cellularAutomaton);
            CellWidth = cellWidth;
            CellHeight = cellHeight;
        }

        public void Draw(Graphics graphics)
        {
            foreach (List<Rectangle> specificCollorRectangles in rectangles.Values)
                specificCollorRectangles.Clear();

            for (int cellX = 0; cellX < cellularAutomaton.Width; cellX++)
            {
                for (int cellY = 0; cellY < cellularAutomaton.Height; cellY++)
                {
                    ColorValues cellColorValues = cellularAutomaton.GetColorValues(cellX, cellY);
                    Rectangle cellRectangle = new Rectangle(cellX * CellWidth, cellY * CellHeight, CellWidth, CellHeight);
                    rectangles[cellColorValues].Add(cellRectangle);
                }
            }

            foreach (KeyValuePair<ColorValues, List<Rectangle>> pair in rectangles)
            {
                ColorValues cellColorValues = pair.Key;
                List<Rectangle> specificColorRectangles = pair.Value;

                if (specificColorRectangles.Count > 0)
                {
                    Color cellsColor = Color.FromArgb(cellColorValues.ToArgb());
                    brush.Color = cellsColor;
                    graphics.FillRectangles(brush, specificColorRectangles.ToArray());
                }
            }
        }
    }
}
