using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class SandPile : ICellularAutomaton, IStatedCellularAutomaton<SandPileCellState>
    {        
        private SandPileCell[][] cellsGrid = null;

        private enum Parity : byte { Even, Odd }
        private Parity stepParity = Parity.Even;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private const int borderThickness = 1;

        private static Randomizer randomizer = Randomizer.Instance;

        public Fraction NeighboringRemainAtRestProbability { get; set; } = new Fraction(1, 2);

        private ColorMode sandColorMode = ColorMode.Uniform;

        public ColorMode SandColorMode
        {
            get { return sandColorMode; }
            set { SetSandColorMode(value); }
        }

        private void SetSandColorMode(ColorMode sandColorMode)
        {
            foreach (SandPileCell[] cells in cellsGrid)
                foreach (SandPileCell cell in cells)
                    cell.SetSandColorMode(sandColorMode);
            this.sandColorMode = sandColorMode;
        }

        public SandPile(int width, int height, ColorMode colorMode)
        {
            Reinitialize(width, height, colorMode);
        }

        // Creates SandPile instance with specified sand color and sets SandColorMode to SlightlyDifferent.
        public SandPile(int width, int height, ColorValues sandColor)
        {
            SetColorValues(SandPileCellState.Sand, sandColor);
            Reinitialize(width, height, ColorMode.SlightlyDifferent);
        }
        public void Reinitialize(int width, int height, ColorMode sandColorMode)
        {
            int extendedWidth = width + borderThickness * 2;
            int extendedHeight = height + borderThickness * 2;

            cellsGrid = new SandPileCell[extendedWidth][];
            for (int x = 0; x < extendedWidth; x++)
            {
                cellsGrid[x] = new SandPileCell[extendedHeight];
                for (int y = 0; y < extendedHeight; y++)
                {
                    SandPileCellState state;
                    if (x == 0 || y == 0 || x == extendedWidth - 1 || y == extendedHeight - 1)
                        state = SandPileCellState.Wall;
                    else
                        state = SandPileCellState.Air;
                    cellsGrid[x][y] = new SandPileCell(state, sandColorMode);
                }
            }
            Width = width;
            Height = height;
            this.sandColorMode = sandColorMode;
        }

        public void Reinitialize() =>
            Reinitialize(Width, Height, sandColorMode);

        public void Reinitialize(int width, int height) =>
            Reinitialize(width, height, sandColorMode);


        public ColorValues GetColorValues(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].GetColorValues();

        public ColorValues[] GetCellsColors()
        {
            if (sandColorMode == ColorMode.Uniform)
                return SandPileCell.GetColors();
            else // if (sandColorMode == ColorMode.SlightlyDifferent)
            {
                SortedSet<ColorValues> colorValues = new SortedSet<ColorValues>(new ColorValuesComparer());
                foreach (SandPileCell[] cells in cellsGrid)
                    foreach (SandPileCell cell in cells)
                        colorValues.Add(cell.GetColorValues());
                return colorValues.ToArray();
            }
        }

        public SandPileCellState GetCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State;

        public void SetCellState(int x, int y, SandPileCellState cellState) =>
            cellsGrid[x + borderThickness][y + borderThickness].State = cellState;

        public ColorValues GetColorValues(SandPileCellState cellState) =>
            SandPileCell.GetColorValues(cellState);

        public void SetColorValues(SandPileCellState cellState, ColorValues colorValues)
        {
            SandPileCell.SetColorValues(cellState, colorValues);
            if (cellState == SandPileCellState.Sand && SandColorMode == ColorMode.SlightlyDifferent)
            {
                foreach (SandPileCell[] cells in cellsGrid)
                    foreach (SandPileCell cell in cells)
                        cell.ReinitializeDeviatedSandColor();
            }
        }

        private void UpdateBlock(int x, int y)
        {
            SandPileCell cellXY = cellsGrid[x][y];
            SandPileCell cellX1Y = cellsGrid[x + 1][y];
            SandPileCell cellXY1 = cellsGrid[x][y + 1];
            SandPileCell cellX1Y1 = cellsGrid[x + 1][y + 1];

            bool ruleApplied = false;

            if (cellXY.State == SandPileCellState.Air && cellX1Y.State == SandPileCellState.Sand)
            {
                if (cellXY1.State == SandPileCellState.Air)
                {
                    if (cellX1Y1.State == SandPileCellState.Air)
                    {
                        cellX1Y.FallTo(cellX1Y1); // 1
                        ruleApplied = true;
                    }
                    else if (cellX1Y1.State == SandPileCellState.Sand)
                    {
                        cellX1Y.FallTo(cellXY1); // 2
                        ruleApplied = true;
                    }
                }
                else if (cellXY1.State == SandPileCellState.Sand && cellX1Y1.State == SandPileCellState.Air)
                {
                    cellX1Y.FallTo(cellX1Y1); // 3
                    ruleApplied = true;
                }
            }
            else if (cellXY.State == SandPileCellState.Sand)
            {
                if (cellX1Y.State == SandPileCellState.Air)
                {
                    if (cellXY1.State == SandPileCellState.Air)
                    {
                        cellXY.FallTo(cellXY1); // 4, 5
                        ruleApplied = true;
                    }
                    else if (cellXY1.State == SandPileCellState.Sand && cellX1Y1.State == SandPileCellState.Air)
                    {
                        cellXY.FallTo(cellX1Y1); // 6
                        ruleApplied = true;
                    }
                }
                if (cellX1Y.State == SandPileCellState.Sand)
                {
                    if (cellXY1.State == SandPileCellState.Air)
                    {
                        if (cellX1Y1.State == SandPileCellState.Air)
                        {
                            if (!randomizer.BernoulliTrialSuccess(NeighboringRemainAtRestProbability))
                            {
                                cellXY.FallTo(cellXY1); // 7
                                cellX1Y.FallTo(cellX1Y1);
                                ruleApplied = true;
                            }
                        }
                        else if (cellX1Y1.State == SandPileCellState.Sand)
                        {
                            cellXY.FallTo(cellXY1); // 8
                            ruleApplied = true;
                        }
                    }
                    else if (cellXY1.State == SandPileCellState.Sand)
                    {
                        if (cellX1Y1.State == SandPileCellState.Air)
                        {
                            cellX1Y.FallTo(cellX1Y1); // 9
                            ruleApplied = true;
                        }
                    }
                }
            }

            if (!ruleApplied)
            {
                if (cellX1Y.State == SandPileCellState.Sand && cellX1Y1.State == SandPileCellState.Air
                    && (cellXY.State == SandPileCellState.Wall || cellXY1.State == SandPileCellState.Wall))
                    cellX1Y.FallTo(cellX1Y1); // 10
                if (cellXY.State == SandPileCellState.Sand && cellXY1.State == SandPileCellState.Air
                    && (cellX1Y.State == SandPileCellState.Wall || cellX1Y1.State == SandPileCellState.Wall))
                    cellXY.FallTo(cellXY1); // 11
            }
        }

        public void NextStep()
        {
            int startRowColumn;
            if (stepParity == Parity.Even)
            {
                stepParity = Parity.Odd;
                startRowColumn = 0;
            }
            else // if (stepParity == StepParity.Odd)
            {
                stepParity = Parity.Even;
                startRowColumn = 1;
            }

            int extendedWidthLessOne = Width + borderThickness * 2 - 1;
            int extendedHeightLessOne = Height + borderThickness * 2 - 1;

            for (int x = startRowColumn; x < extendedWidthLessOne; x += 2)
                for (int y = startRowColumn; y < extendedHeightLessOne; y += 2)
                    UpdateBlock(x, y);
        }
    }
}
