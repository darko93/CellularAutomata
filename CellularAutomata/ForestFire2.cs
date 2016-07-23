using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class ForestFire2 : ICellularAutomaton, IStatedCellularAutomaton<ForestFireCellState>
    {
        private ForestFireCell[][] cellsGrid = null;
        private ForestFireCell[][] newCellsGrid = null;

        private Randomizer randomizer = Randomizer.Instance;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Neighborhood Neighborhood { get; set; } = Neighborhood.vonNeumann;
        public Fraction GrowProbability { get; set; } = new Fraction(1, 200);
        public Fraction SpontanBurnProbability { get; set; } = new Fraction(1, 200000);
        public Fraction BurnFromNeighborProbability { get; set; } = new Fraction(1, 3);
        public int PercentageInitialTreeDensity { get; set; } = 0;
        public int BurnStepsAmount
        {
            get { return ForestFireCell.TotalBurnStepsAmount; }
            set { ForestFireCell.TotalBurnStepsAmount = value; }
        }

        private const int borderThickness = 1;

        public ForestFire2(int width, int height, int percentageInitialTreeDensity)
        {
            Reinitialize(width, height, percentageInitialTreeDensity);
            BurnStepsAmount = 5;
        }

        public void Reinitialize(int width, int height, int percentageInitialTreeDensity)
        {
            int extendedWidth = width + 2;
            int extendedHeight = height + 2;
            cellsGrid = new ForestFireCell[extendedWidth][];
            newCellsGrid = new ForestFireCell[extendedWidth][];
            for (int x = 0; x < extendedWidth; x++)
            {
                cellsGrid[x] = new ForestFireCell[extendedHeight];
                newCellsGrid[x] = new ForestFireCell[extendedHeight];
                for (int y = 0; y < extendedHeight; y++)
                {
                    cellsGrid[x][y] = new ForestFireCell(ForestFireCellState.Empty);
                    newCellsGrid[x][y] = new ForestFireCell(ForestFireCellState.Empty);
                }
            }
            if (percentageInitialTreeDensity > 0)
                SetRandomCellsState(percentageInitialTreeDensity, ForestFireCellState.Tree);
            Width = width;
            Height = height;
            PercentageInitialTreeDensity = percentageInitialTreeDensity;
        }

        public void Reinitialize() =>
            Reinitialize(Width, Height, PercentageInitialTreeDensity);

        public void Reinitialize(int width, int height) =>
            Reinitialize(width, height, PercentageInitialTreeDensity);

        public ForestFireCellState GetCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State;

        public void SetCellState(int x, int y, ForestFireCellState cellState) =>
            cellsGrid[x + borderThickness][y + borderThickness].State = cellState;

        public ColorValues GetColorValues(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].GetColorValues();

        public void SetColorValues(ForestFireCellState cellState, ColorValues colorValues) =>
            ForestFireCell.SetColorValues(cellState, colorValues);

        private void SetRandomCellsState(int cellsPercentage, ForestFireCellState cellState, Point leftBottomBound, Point rightTopBound)
        {
            Point[] randomPoints = randomizer.GetDistinctRandomPoints(cellsPercentage, leftBottomBound, rightTopBound);

            foreach (Point point in randomPoints)
                cellsGrid[point.X + borderThickness][point.Y + borderThickness].State = cellState;
        }

        public void SetRandomCellsState(int cellsPercentage, ForestFireCellState cellState)
        {
            Point leftBottomBound = new Point(0, 0);
            Point rightTopBound = new Point(Width - 1, Height - 1);
            SetRandomCellsState(cellsPercentage, cellState, leftBottomBound, rightTopBound);
        }

        public void SetRandomCellsState(int cellsPercentage, ForestFireCellState cellState, Point location, int width, int height)
        {
            double halfWidth = width * 0.5;
            int leftBoundX = location.X - (int)Math.Floor(halfWidth);
            int rightBoundX = location.X + (int)Math.Ceiling(halfWidth);

            double halfHeight = height * 0.5;
            int bottomBoundY = location.Y - (int)Math.Floor(halfHeight);
            int topBoundY = location.Y + (int)Math.Floor(halfHeight);

            Point leftBottomBound = new Point(leftBoundX, bottomBoundY);
            Point rightTopBound = new Point(rightBoundX, topBoundY);

            SetRandomCellsState(cellsPercentage, cellState, leftBottomBound, rightTopBound);
        }

        public ColorValues[] GetCellsColors() =>
            ForestFireCell.GetColors();

        private int GetOrthogonalNeighborsOfStateAmount(int cellX, int cellY, ForestFireCellState cellState)
        {
            int neighborsAmount = 0;
            // W
            if (cellsGrid[cellX - 1][cellY].State == cellState)
                neighborsAmount++;
            // E
            if (cellsGrid[cellX + 1][cellY].State == cellState)
                neighborsAmount++;
            // N
            if (cellsGrid[cellX][cellY - 1].State == cellState)
                neighborsAmount++;
            // S
            if (cellsGrid[cellX][cellY + 1].State == cellState)
                neighborsAmount++;
            return neighborsAmount;
        }

        private int GetDiagonalNeighborsOfStateAmount(int cellX, int cellY, ForestFireCellState cellState)
        {
            int neighborsAmount = 0;
            // NW
            if (cellsGrid[cellX - 1][cellY - 1].State == cellState)
                neighborsAmount++;
            // NE
            if (cellsGrid[cellX + 1][cellY - 1].State == cellState)
                neighborsAmount++;
            // SE
            if (cellsGrid[cellX + 1][cellY + 1].State == cellState)
                neighborsAmount++;
            // SW
            if (cellsGrid[cellX - 1][cellY + 1].State == cellState)
                neighborsAmount++;
            return neighborsAmount;
        }

        private int GetNeighborsOfStateAmount(int cellX, int cellY, ForestFireCellState cellState)
        {
            int neighborsOfStateAmount = GetOrthogonalNeighborsOfStateAmount(cellX, cellY, cellState);
            if (Neighborhood == Neighborhood.Moore)
                neighborsOfStateAmount += GetDiagonalNeighborsOfStateAmount(cellX, cellY, cellState);
            return neighborsOfStateAmount;
        }

        private bool StartToBurn(int cellX, int cellY)
        {
            if (randomizer.BernoulliTrialSuccess(SpontanBurnProbability))
                return true;
            int burningNeighborsAmount = GetNeighborsOfStateAmount(cellX, cellY, ForestFireCellState.BurningTree);
            return randomizer.AtLeastOneSuccessInBernoulliProcess(BurnFromNeighborProbability, burningNeighborsAmount);
        }

        private void SwapCellsGrid()
        {
            ForestFireCell[][] tempCellsGrid = cellsGrid;
            cellsGrid = newCellsGrid;
            newCellsGrid = tempCellsGrid;
        }

        public void NextStep()
        {
            int lastX = borderThickness + Width;
            int lastY = borderThickness + Height;

            for (int x = borderThickness; x < lastX; x++)
            {
                for (int y = borderThickness; y < lastY; y++)
                {
                    ForestFireCell cellXY = cellsGrid[x][y];
                    ForestFireCell newCellXY = newCellsGrid[x][y];

                    switch (cellXY.State)
                    {
                        case ForestFireCellState.Empty:
                            {
                                if (randomizer.BernoulliTrialSuccess(GrowProbability))
                                    newCellXY.State = ForestFireCellState.Tree;
                                else
                                    newCellXY.State = ForestFireCellState.Empty; // Needed when cellXY has just stopped burn, because newCellXY (old) was a burning tree 1 step left.
                            }
                            break;
                        case ForestFireCellState.Tree:
                            {
                                if (StartToBurn(x, y))
                                    newCellXY.StartToBurn();
                                else
                                    newCellXY.State = ForestFireCellState.Tree; // Needed when cellXY has just grown, because newCellXY (old) was an empty cell.
                            }
                            break;
                        case ForestFireCellState.BurningTree:
                            newCellXY.BurnOneStep(cellXY.BurnStepsAmount);
                            break;
                    }
                }
            }
            SwapCellsGrid();
        }
    }
}
