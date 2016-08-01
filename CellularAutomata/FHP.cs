using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class FHP : ICellularAutomaton, IStatedCellularAutomaton<FHPCellState>
    {
        private FHPParticleState[][] outState = null;

        private FHPCell[][] cellsGrid = null;
        private FHPCell[][] newCellsGrid = null;

        private const int borderThickness = 1;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public FHP(int width, int height)
        {
            if (outState == null)
                InitializeOutStates();
            Reinitialize(width, height);
        }

        private void InitializeOutStates()
        {
            outState = new FHPParticleState[256][];

            // Initialize non-boundary part of collisions array.
            for (int i = 0; i < 128; i++)
            {
                FHPParticleState iInState = (FHPParticleState)i;
                outState[i] = new[] { iInState, iInState };
            }

            // Setting non-boundary collisions.
            FHPCollision[] fhpCollisions = XMLManager.Instance.GetFHPCollisions();
            foreach (FHPCollision fhpCollision in fhpCollisions)
            {
                FHPParticleState[] fhpOutStatesPair = outState[fhpCollision.InState];
                fhpOutStatesPair[0] = (FHPParticleState)fhpCollision.OutState1;
                fhpOutStatesPair[1] = (FHPParticleState)fhpCollision.OutState2;
            }

            // Initialize boundary part of collisions array and setting boundary collisions.
            for (int i = 128; i < 256; i++)
            {
                FHPParticleState iInState = (FHPParticleState)i;
                FHPParticleState iOutState = GetBoundaryOutState(iInState);
                outState[i] = new[] { iOutState, iOutState };
            }
        }

        private FHPParticleState GetBoundaryOutState(FHPParticleState inState)
        {
            FHPParticleState outState = FHPParticleState.Wall;

            if (inState.HasFlag(FHPParticleState.RightUp))
                outState |= FHPParticleState.LeftDown;
            if (inState.HasFlag(FHPParticleState.Right))
                outState |= FHPParticleState.Left;
            if (inState.HasFlag(FHPParticleState.RightDown))
                outState |= FHPParticleState.LeftUp;
            if (inState.HasFlag(FHPParticleState.LeftDown))
                outState |= FHPParticleState.RightUp;
            if (inState.HasFlag(FHPParticleState.Left))
                outState |= FHPParticleState.Right;
            if (inState.HasFlag(FHPParticleState.LeftUp))
                outState |= FHPParticleState.RightDown;
            if (inState.HasFlag(FHPParticleState.Rest)) // This shouldn't happen.
                outState |= FHPParticleState.Rest;

            return outState;
        }

        public void Reinitialize(int width, int height)
        {
            int extendedWidth = width + borderThickness * 2;
            int extendedHeight = height + borderThickness * 2;

            cellsGrid = new FHPCell[extendedWidth][];
            newCellsGrid = new FHPCell[extendedWidth][];
            for (int x = 0; x < extendedWidth; x++)
            {
                cellsGrid[x] = new FHPCell[extendedHeight];
                newCellsGrid[x] = new FHPCell[extendedHeight];
                for (int y = 0; y < extendedHeight; y++)
                {
                    FHPParticleState particleState;
                    if (x == 0 || y == 0 || x == extendedWidth - 1 || y == extendedHeight - 1)
                        particleState = FHPParticleState.Wall;
                    else
                        particleState = FHPParticleState.None;
                    cellsGrid[x][y] = new FHPCell(particleState);
                    newCellsGrid[x][y] = new FHPCell(particleState);
                }
            }
            Width = width;
            Height = height;
        }

        public void Reinitialize() =>
            Reinitialize(Width, Height);

        public ColorValues GetColorValues(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].GetColorValues();

        public ColorValues[] GetCellsColors() =>
            FHPCell.GetCellsColors();

        public FHPCellState GetCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State;

        public void SetCellState(int x, int y, FHPCellState cellState) =>
            cellsGrid[x + borderThickness][y + borderThickness].State = cellState;

        public ColorValues GetColorValues(FHPCellState cellState) =>
            FHPCell.GetColorValues(cellState);

        public void SetColorValues(FHPCellState cellState, ColorValues colorValues) =>
            FHPCell.SetColorValues(cellState, colorValues);

        private void HandleCollisions()
        {
            int extendedWidth = Width + borderThickness * 2;
            int extendedHeight = Height + borderThickness * 2;
            
            for (int x = 0; x < extendedWidth; x++)
            {
                for (int y = 0; y < extendedHeight; y++)
                {
                    FHPParticleState particleInStateXY = cellsGrid[x][y].ParticleState;
                    int randomOutStateIndex = Randomizer.Instance.Next(2);
                    cellsGrid[x][y].ParticleState = outState[(int)particleInStateXY][randomOutStateIndex];
                }
            }
        }

        private void MoveParticles()
        {
            int extendedWidth = Width + borderThickness * 2;
            int extendedHeight = Height + borderThickness * 2;

            for (int x = 0; x < extendedWidth; x++)
            {
                for (int y = 0; y < extendedHeight; y++)
                {
                    int diagonalRightIndex;
                    int diagonalLeftIndex;
                    if (y % 2 == 0) // If current row index is even.
                    {
                        diagonalRightIndex = x;
                        diagonalLeftIndex = x - 1;
                    }
                    else // If current row index is odd.
                    {
                        diagonalRightIndex = x + 1;
                        diagonalLeftIndex = x;
                    }

                    FHPCell cellXY = cellsGrid[x][y];
                    FHPParticleState particleInStateXY = cellXY.ParticleState;
                    
                    if (particleInStateXY.HasFlag(FHPParticleState.Wall))
                        newCellsGrid[x][y].ParticleState |= FHPParticleState.Wall;
                    if (particleInStateXY.HasFlag(FHPParticleState.Rest))
                        newCellsGrid[x][y].ParticleState |= FHPParticleState.Rest;
                    if (particleInStateXY.HasFlag(FHPParticleState.RightUp))
                        newCellsGrid[diagonalRightIndex][y - 1].ParticleState |= FHPParticleState.RightUp;
                    if (particleInStateXY.HasFlag(FHPParticleState.Right))
                        newCellsGrid[x + 1][y].ParticleState |= FHPParticleState.Right;
                    if (particleInStateXY.HasFlag(FHPParticleState.RightDown))
                        newCellsGrid[diagonalRightIndex][y + 1].ParticleState |= FHPParticleState.RightDown;
                    if (particleInStateXY.HasFlag(FHPParticleState.LeftDown))
                        newCellsGrid[diagonalLeftIndex][y + 1].ParticleState |= FHPParticleState.LeftDown;
                    if (particleInStateXY.HasFlag(FHPParticleState.Left))
                        newCellsGrid[x - 1][y].ParticleState |= FHPParticleState.Left;
                    if (particleInStateXY.HasFlag(FHPParticleState.LeftUp))
                        newCellsGrid[diagonalLeftIndex][y - 1].ParticleState |= FHPParticleState.LeftUp;

                    cellXY.ParticleState = FHPParticleState.None;
                }
            }
        }

        private void SwapCellsGrid()
        {
            FHPCell[][] tempCellsGrid = cellsGrid;
            cellsGrid = newCellsGrid;
            newCellsGrid = tempCellsGrid;
        }

        public void NextStep()
        {
            HandleCollisions();
            MoveParticles();
            SwapCellsGrid();
        }
    }
}
