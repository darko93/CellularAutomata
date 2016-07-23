using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class ForestFire : ICellularAutomaton, IStatedCellularAutomaton<ForestFireCellState>
    {
        private ForestFireCell[][] cellsGrid = null;

        private Randomizer randomizer = Randomizer.Instance;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Neighborhood Neighborhood { get; set; } = Neighborhood.vonNeumann;
        public Fraction TreeGrowProbability { get; set; } = new Fraction(1, 200);
        public Fraction TreeBurnProbability { get; set; } = new Fraction(1, 200000);
        public Fraction BurnFromNeighborProbability { get; set; } = new Fraction(1, 3);
        public int PercentageInitialTreeDensity { get; set; } = 0;
        public int BurnStepsAmount
        {
            get { return ForestFireCell.TotalBurnStepsAmount; }
            set { ForestFireCell.TotalBurnStepsAmount = value; }
        }

        private const int borderThickness = 1;

        public ForestFire(int width, int height)
        {
            Reinitialize(width, height);
            BurnStepsAmount = 5;
        }

        public void Reinitialize(int width, int height)
        {
            int extendedWidth = width + 2;
            int extendedHeight = height + 2;
            cellsGrid = new ForestFireCell[extendedWidth][];
            for (int x = 0; x < extendedWidth; x++)
            {
                cellsGrid[x] = new ForestFireCell[extendedHeight];
                for (int y = 0; y < extendedHeight; y++)
                    cellsGrid[x][y] = new ForestFireCell(ForestFireCellState.Empty);
            }
            if (PercentageInitialTreeDensity > 0)
                SetRandomCellsState(PercentageInitialTreeDensity, ForestFireCellState.Tree);
            Width = width;
            Height = height;
        }

        public void Reinitialize() =>
            Reinitialize(Width, Height);

        public ForestFireCellState GetCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State;

        public void SetCellState(int x, int y, ForestFireCellState cellState) =>
            cellsGrid[x + borderThickness][y + borderThickness].State = cellState;

        public ColorValues GetColorValues(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].GetColorValues();

        public void SetColorValues(ForestFireCellState cellState, ColorValues colorValues) =>
            ForestFireCell.SetColorValues(cellState, colorValues);

        private void SetRandomCellsState(int cellsPercentage, ForestFireCellState cellState)
        {
            int cellsAmount = Width * Height;
            int cellsOfDesiredStateAmount = (int)(cellsPercentage * 0.01 * cellsAmount);

            int[] randomIndexes = randomizer.GetDistinctRandomNumbers(cellsOfDesiredStateAmount, 0, cellsAmount - 1);

            foreach (int randomIndex in randomIndexes)
            {
                int cellX = randomIndex % Width;
                int cellY = randomIndex / Width;
                cellsGrid[cellX + borderThickness][cellY + borderThickness].State = cellState;
            }
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
            if (randomizer.BernoulliTrialSuccess(TreeBurnProbability))
                return true;
            int burningNeighborsAmount = GetNeighborsOfStateAmount(cellX, cellY, ForestFireCellState.BurningTree);
            return randomizer.AtLeastOneSuccessInBernoulliProcess(BurnFromNeighborProbability, burningNeighborsAmount);
        }

        public void NextStep()
        {
            int extendedWidth = Width + borderThickness * 2;
            int extendedHeight = Height + borderThickness * 2;

            ForestFireCell[][] newCellsGrid = new ForestFireCell[extendedWidth][];

            ForestFireCell emptyCell = cellsGrid[0][0];
            
            for (int x = 0; x < extendedWidth; x++)
            {
                newCellsGrid[x] = new ForestFireCell[extendedHeight];
                for (int y = 0; y < extendedHeight; y++)
                {
                    if (x == 0 || y == 0 || x == extendedWidth - 1 || y == extendedHeight - 1)
                        newCellsGrid[x][y] = emptyCell;
                    else
                    {
                        ForestFireCell cellXY = cellsGrid[x][y];
                        switch (cellXY.State)
                        {
                            case ForestFireCellState.Empty:
                                if (randomizer.BernoulliTrialSuccess(TreeGrowProbability))
                                    cellXY.Grow();
                                break;
                            case ForestFireCellState.Tree:
                                if (StartToBurn(x, y))
                                    cellXY.StartToBurn();
                                break;
                            case ForestFireCellState.BurningTree:
                                cellXY.BurnOneStep();
                                break;
                        }
                        newCellsGrid[x][y] = cellXY;
                    }
                }
            }
            cellsGrid = newCellsGrid;
        }
    }
}
    