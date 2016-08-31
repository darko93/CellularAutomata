namespace CellularAutomata
{
    public class ForestFireCell : ICell<ForestFireCellState>
    {
        private static ColorValues treeColor = ColorValues.Tree;
        private static ColorValues burningTreeColor = ColorValues.BurningTree;
        private static ColorValues emptyCellColor = ColorValues.EmptyCell;

        public static int TotalBurnStepsAmount { get; set; }

        public ForestFireCellState State { get; set; }
        public int BurnStepsAmount { get; set; }

        public ForestFireCell(ForestFireCellState state, int burnTurnsAmount)
        {
            State = state;
            BurnStepsAmount = burnTurnsAmount;
        }

        public ForestFireCell(ForestFireCellState state)
        {
            State = state;
            BurnStepsAmount = 0;
        }

        public ColorValues GetColorValues()
        {
            switch (State)
            {
                case ForestFireCellState.Empty:
                    return emptyCellColor;
                case ForestFireCellState.Tree:
                    return treeColor;
                case ForestFireCellState.BurningTree:
                    return burningTreeColor;
                default:
                    return null;
            }
        }

        public void BurnOneStep()
        {
            BurnStepsAmount--;
            if (BurnStepsAmount < 1)
                State = ForestFireCellState.Empty;
        }

        public void BurnOneStep(int burnStepsAmount)
        {
            State = ForestFireCellState.BurningTree;
            BurnStepsAmount = --burnStepsAmount;
            if (BurnStepsAmount < 1)
                State = ForestFireCellState.Empty;
        }

        public void Grow() =>
            State = ForestFireCellState.Tree;

        public static ColorValues GetColorValues(ForestFireCellState cellState)
        {
            switch (cellState)
            {
                case ForestFireCellState.Empty:
                    return emptyCellColor;
                case ForestFireCellState.Tree:
                    return treeColor;
                case ForestFireCellState.BurningTree:
                    return burningTreeColor;
                default:
                    return null;
            }
        }

        public static void SetColorValues(ColorValues colorValues, ForestFireCellState cellState)
        {
            switch (cellState)
            {
                case ForestFireCellState.Empty:
                    emptyCellColor = colorValues;
                    break;
                case ForestFireCellState.Tree:
                    treeColor = colorValues;
                    break;
                case ForestFireCellState.BurningTree:
                    burningTreeColor = colorValues;
                    break;
            }
        }

        public void StartToBurn()
        {
            State = ForestFireCellState.BurningTree;
            BurnStepsAmount = TotalBurnStepsAmount;
        }

        public static ColorValues[] GetColors() =>
            new[]
            {
                treeColor,
                burningTreeColor,
                emptyCellColor
            };
    }
}
