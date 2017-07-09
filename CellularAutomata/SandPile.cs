using System.Collections.Generic;
using System.Linq;

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

        public Fraction NeighboringRemainAtRestProbability { get; set; } = new Fraction(1, 2);

        private ColorsMode sandColorMode = ColorsMode.Uniform;

        public ColorsMode SandColorMode
        {
            get { return sandColorMode; }
            set { SetSandColorMode(value); }
        }

        private void SetSandColorMode(ColorsMode sandColorMode)
        {
            foreach (SandPileCell[] cells in cellsGrid)
                foreach (SandPileCell cell in cells)
                    cell.SetSandColorMode(sandColorMode);
            this.sandColorMode = sandColorMode;
        }

        public SandPile(int width, int height, ColorsMode colorMode)
        {
            Reinitialize(width, height, colorMode);
        }

        public SandPile(int width, int height) 
            : this(width, height, ColorsMode.Uniform) { }

        // Creates SandPile instance with specified sand color and sets SandColorMode to SlightlyDifferent.
        public SandPile(int width, int height, ColorValues sandColor)
        {
            SetColorValues(sandColor, SandPileCellState.SandGrain);
            Reinitialize(width, height, ColorsMode.SlightlyDifferent);
        }
        public void Reinitialize(int width, int height, ColorsMode sandColorMode)
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
                        state = SandPileCellState.Empty;
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
            if (sandColorMode == ColorsMode.Uniform)
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

        public void SetCellState(SandPileCellState cellState, int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State = cellState;

        public ColorValues GetColorValues(SandPileCellState cellState) =>
            SandPileCell.GetColorValues(cellState);

        public void SetColorValues(ColorValues colorValues, SandPileCellState cellState)
        {
            SandPileCell.SetColorValues(colorValues, cellState);
            if (cellState == SandPileCellState.SandGrain && SandColorMode == ColorsMode.SlightlyDifferent)
            {
                foreach (SandPileCell[] cells in cellsGrid)
                    foreach (SandPileCell cell in cells)
                        cell.ReinitializeDeviatedSandColor();
            }
        }

        private void UpdateBlock(int x, int y)
        {
            SandPileCell upperLeft = cellsGrid[x][y];
            SandPileCell upperRight = cellsGrid[x + 1][y];
            SandPileCell lowerLeft = cellsGrid[x][y + 1];
            SandPileCell lowerRight = cellsGrid[x + 1][y + 1];

            bool ruleApplied = false;

            if (upperLeft.State == SandPileCellState.Empty && upperRight.State == SandPileCellState.SandGrain)
            {
                if (lowerLeft.State == SandPileCellState.Empty)
                {
                    if (lowerRight.State == SandPileCellState.Empty)
                    {
                        upperRight.FallTo(lowerRight); // 1
                        ruleApplied = true;
                    }
                    else if (lowerRight.State == SandPileCellState.SandGrain)
                    {
                        upperRight.FallTo(lowerLeft); // 2
                        ruleApplied = true;
                    }
                }
                else if (lowerLeft.State == SandPileCellState.SandGrain && lowerRight.State == SandPileCellState.Empty)
                {
                    upperRight.FallTo(lowerRight); // 3
                    ruleApplied = true;
                }
            }
            else if (upperLeft.State == SandPileCellState.SandGrain)
            {
                if (upperRight.State == SandPileCellState.Empty)
                {
                    if (lowerLeft.State == SandPileCellState.Empty)
                    {
                        upperLeft.FallTo(lowerLeft); // 4, 5
                        ruleApplied = true;
                    }
                    else if (lowerLeft.State == SandPileCellState.SandGrain && lowerRight.State == SandPileCellState.Empty)
                    {
                        upperLeft.FallTo(lowerRight); // 6
                        ruleApplied = true;
                    }
                }
                if (upperRight.State == SandPileCellState.SandGrain)
                {
                    if (lowerLeft.State == SandPileCellState.Empty)
                    {
                        if (lowerRight.State == SandPileCellState.Empty)
                        {
                            if (!Randomizer.Instance.BernoulliTrialSuccess(NeighboringRemainAtRestProbability))
                            {
                                upperLeft.FallTo(lowerLeft); // 7
                                upperRight.FallTo(lowerRight);
                                ruleApplied = true;
                            }
                        }
                        else if (lowerRight.State == SandPileCellState.SandGrain)
                        {
                            upperLeft.FallTo(lowerLeft); // 8
                            ruleApplied = true;
                        }
                    }
                    else if (lowerLeft.State == SandPileCellState.SandGrain)
                    {
                        if (lowerRight.State == SandPileCellState.Empty)
                        {
                            upperRight.FallTo(lowerRight); // 9
                            ruleApplied = true;
                        }
                    }
                }
            }

            if (!ruleApplied)
            {
                if (upperRight.State == SandPileCellState.SandGrain && lowerRight.State == SandPileCellState.Empty
                    && (upperLeft.State == SandPileCellState.Wall || lowerLeft.State == SandPileCellState.Wall))
                    upperRight.FallTo(lowerRight); // 10
                if (upperLeft.State == SandPileCellState.SandGrain && lowerLeft.State == SandPileCellState.Empty
                    && (upperRight.State == SandPileCellState.Wall || lowerRight.State == SandPileCellState.Wall))
                    upperLeft.FallTo(lowerLeft); // 11
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
