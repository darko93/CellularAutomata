namespace CellularAutomata
{
    interface ICell<TCellState>
    {
        TCellState State { get; set; }
        ColorValues GetColorValues();
    }
}
