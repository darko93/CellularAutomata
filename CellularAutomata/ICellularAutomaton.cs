using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CellularAutomataTests")]

namespace CellularAutomata
{
    public interface ICellularAutomaton
    {
        int Width { get; }
        int Height { get; }
        ColorValues GetColorValues(int x, int y);
        void Reinitialize(int width, int height);
        void Reinitialize();
        void NextStep();
        ColorValues[] GetCellsColors();
    }
}
