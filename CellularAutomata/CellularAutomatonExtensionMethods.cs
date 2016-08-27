using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public static class CellularAutomatonExtensionMethods
    {
        public static void SetCellsState<TCellState>(this IStatedCellularAutomaton<TCellState> cellularAutomaton, TCellState cellState, Point leftTopBound, Point rightBottomBound)
        {
            for (int x = leftTopBound.X; x <= rightBottomBound.X; x++)
                for (int y = leftTopBound.Y; y <= rightBottomBound.Y; y++)
                    cellularAutomaton.SetCellState(cellState, x, y);
        }

        public static void SetCellsState<TCellState>(this IStatedCellularAutomaton<TCellState> cellularAuomaton, TCellState cellState, Point center, int width, int height)
        {
            double halfWidth = width * 0.5;
            int leftBoundX = center.X - (int)Math.Floor(halfWidth);
            int rightBoundX = center.X + (int)Math.Ceiling(halfWidth);

            double halfHeight = height * 0.5;
            int topBoundY = center.Y - (int)Math.Floor(halfHeight);
            int bottomBoundY = center.Y + (int)Math.Ceiling(halfHeight);

            Point leftTopBound = new Point(leftBoundX, topBoundY);
            Point rightBottomBound = new Point(rightBoundX, bottomBoundY);

            cellularAuomaton.SetCellsState(cellState, leftTopBound, rightBottomBound);
        }

        public static void SetRandomCellsState<TCellState>(this IStatedCellularAutomaton<TCellState> cellularAutomaton, TCellState cellState, int cellsPercentage, Point leftTopBound, Point rightBottomBound)
        {
            Point[] randomPoints = Randomizer.Instance.GetDistinctRandomPoints(cellsPercentage, leftTopBound, rightBottomBound);

            foreach (Point point in randomPoints)
                cellularAutomaton.SetCellState(cellState, point.X, point.Y);
        }

        public static void SetRandomCellsState<TCellState>(this IStatedCellularAutomaton<TCellState> cellularAutomaton, TCellState cellState, int cellsPercentage)
        {
            Point leftTopBound = new Point(0, 0);
            Point rightBottomBound = new Point(cellularAutomaton.Width - 1, cellularAutomaton.Height - 1);
            cellularAutomaton.SetRandomCellsState(cellState, cellsPercentage, leftTopBound, rightBottomBound);
        }

        public static void SetRandomCellsState<TCellState>(this IStatedCellularAutomaton<TCellState> cellularAutomaton, TCellState cellState, int cellsPercentage, Point center, int width, int height)
        {
            double halfWidth = width * 0.5;
            int leftBoundX = center.X - (int)Math.Floor(halfWidth);
            int rightBoundX = center.X + (int)Math.Ceiling(halfWidth);

            double halfHeight = height * 0.5;
            int topBoundY = center.Y - (int)Math.Floor(halfHeight);
            int bottomBoundY = center.Y + (int)Math.Ceiling(halfHeight);

            Point leftTopBound = new Point(leftBoundX, topBoundY);
            Point rightBottomBound = new Point(rightBoundX, bottomBoundY);

            cellularAutomaton.SetRandomCellsState(cellState, cellsPercentage, leftTopBound, rightBottomBound);
        }
    }
}
