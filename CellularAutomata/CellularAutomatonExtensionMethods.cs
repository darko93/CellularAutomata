using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public static class CellularAutomatonExtensionMethods
    {
        public static void SetRandomCellsState<TCellState>(this IStatedCellularAutomaton<TCellState> cellularAutomaton, TCellState cellState, int cellsPercentage, Point leftBottomBound, Point rightTopBound)
        {
            Point[] randomPoints = Randomizer.Instance.GetDistinctRandomPoints(cellsPercentage, leftBottomBound, rightTopBound);

            foreach (Point point in randomPoints)
                cellularAutomaton.SetCellState(point.X, point.Y, cellState);
        }

        public static void SetRandomCellsState<TCellState>(this IStatedCellularAutomaton<TCellState> cellularAutomaton, TCellState cellState, int cellsPercentage)
        {
            Point leftBottomBound = new Point(0, 0);
            Point rightTopBound = new Point(cellularAutomaton.Width - 1, cellularAutomaton.Height - 1);
            cellularAutomaton.SetRandomCellsState(cellState, cellsPercentage, leftBottomBound, rightTopBound);
        }

        public static void SetRandomCellsState<TCellState>(this IStatedCellularAutomaton<TCellState> cellularAutomaton, TCellState cellState, int cellsPercentage, Point location, int width, int height)
        {
            double halfWidth = width * 0.5;
            int leftBoundX = location.X - (int)Math.Floor(halfWidth);
            int rightBoundX = location.X + (int)Math.Ceiling(halfWidth);

            double halfHeight = height * 0.5;
            int bottomBoundY = location.Y - (int)Math.Floor(halfHeight);
            int topBoundY = location.Y + (int)Math.Floor(halfHeight);

            Point leftBottomBound = new Point(leftBoundX, bottomBoundY);
            Point rightTopBound = new Point(rightBoundX, topBoundY);

            cellularAutomaton.SetRandomCellsState(cellState, cellsPercentage, leftBottomBound, rightTopBound);
        }
    }
}
