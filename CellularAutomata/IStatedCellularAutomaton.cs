namespace CellularAutomata
{
    public interface IStatedCellularAutomaton<TCellState> : ICellularAutomaton
    {
        TCellState GetCellState(int x, int y);
        void SetCellState(TCellState cellState, int x, int y);
        ColorValues GetColorValues(TCellState cellState);
        void SetColorValues(ColorValues colorValues, TCellState cellState);
    }
}
