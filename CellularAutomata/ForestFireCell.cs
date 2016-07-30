﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class ForestFireCell
    {
        public ForestFireCellState State { get; set; }
        public int BurnStepsAmount { get; set; }

        public static int TotalBurnStepsAmount { get; set; }

        private static ColorValues treeColor = ColorValues.Tree;
        private static ColorValues burningTreeColor = ColorValues.BurningTree;
        private static ColorValues emptyCellColor = ColorValues.EmptyCell;

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

        public static void SetColorValues(ForestFireCellState cellState, ColorValues colorValues)
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

        public void StartToBurn()
        {
            State = ForestFireCellState.BurningTree;
            BurnStepsAmount = TotalBurnStepsAmount;
        }

        //POTEM TO USUNĄC
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

        public static ColorValues[] GetColors() =>
            new[]
            {
                treeColor,
                burningTreeColor,
                emptyCellColor
            };
    }
}
