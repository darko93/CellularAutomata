namespace CellularAutomata
{
    public class ForestFire : ICellularAutomaton, IStatedCellularAutomaton<ForestFireCellState>
    {
        private ForestFireCell[][] cellsGrid = null;
        private ForestFireCell[][] newCellsGrid = null;

        private static Randomizer randomizer = Randomizer.Instance;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Neighborhood Neighborhood { get; set; } = Neighborhood.vonNeumann;
        public Fraction GrowProbability { get; set; } = new Fraction(1, 200);
        public Fraction SpontanBurnProbability { get; set; } = new Fraction(1, 200000);
        public Fraction BurnFromNeighborProbability { get; set; } = new Fraction(1, 3);
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
            int extendedWidth = width + borderThickness * 2;
            int extendedHeight = height + borderThickness * 2;
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
            Width = width;
            Height = height;
        }

        public void Reinitialize() =>
            Reinitialize(Width, Height);

        public ColorValues GetColorValues(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].GetColorValues();

        public ForestFireCellState GetCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State;

        public void SetCellState(ForestFireCellState cellState, int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State = cellState;

        public ColorValues GetColorValues(ForestFireCellState cellState) =>
            ForestFireCell.GetColorValues(cellState);

        public void SetColorValues(ColorValues colorValues, ForestFireCellState cellState) =>
            ForestFireCell.SetColorValues(colorValues, cellState);
        
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
            int burningNeighborsAmount = GetNeighborsOfStateAmount(cellX, cellY, ForestFireCellState.BurningTree);
            if (burningNeighborsAmount == 0)
                return randomizer.BernoulliTrialSuccess(SpontanBurnProbability);
            else // if (burningNeighborsAmount > 0)
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
