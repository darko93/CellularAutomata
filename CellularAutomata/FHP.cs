using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class FHP : ICellularAutomaton, IStatedCellularAutomaton<FHPCellState>
    {
        private static FHPParticleCellState[] outState1 = null;
        private static FHPParticleCellState[] outState2 = null;
        private static FHPParticleCellState[][] outStatesPair = null;

        private FHPCell[][] cellsGrid = null;
        private FHPCell[][] newCellsGrid = null;

        private const int borderThickness = 1;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public FHP(int width, int height)
        {
            if (outState1 == null || outState2 == null || outStatesPair == null)
                InitializeOutStates();
            Reinitialize(width, height);
        }

        private void InitializeOutStates()
        {
            outState1 = new FHPParticleCellState[256];
            outState2 = new FHPParticleCellState[256];
            outStatesPair = new FHPParticleCellState[2][]
            {
                outState1,
                outState2
            };

            // Initialize non-boundary part of collisions array.
            for (int i = 0; i < 128; i++)
            {
                FHPParticleCellState iInState = (FHPParticleCellState)i;
                outState1[i] = outState2[i] = iInState;
            }

            // Setting non-boundary collisions.
            FHPCollision[] fhpCollisions = XMLManager.Instance.GetFHPCollisions();
            foreach (FHPCollision fhpCollision in fhpCollisions)
            {
                outState1[fhpCollision.InState] = (FHPParticleCellState)fhpCollision.OutState1;
                outState2[fhpCollision.InState] = (FHPParticleCellState)fhpCollision.OutState2;
            }

            // Initialize boundary part of collisions array and setting boundary collisions.
            for (int i = 128; i < 256; i++)
            {
                FHPParticleCellState iInState = (FHPParticleCellState)i;
                FHPParticleCellState iOutState = GetBoundaryOutState(iInState);
                outState1[i] = outState2[i] = iOutState;
            }
        }

        private FHPParticleCellState GetBoundaryOutState(FHPParticleCellState inState)
        {
            FHPParticleCellState outState = FHPParticleCellState.Wall;

            if (inState.HasFlag(FHPParticleCellState.Northeast))
                outState |= FHPParticleCellState.Southwest;
            if (inState.HasFlag(FHPParticleCellState.East))
                outState |= FHPParticleCellState.West;
            if (inState.HasFlag(FHPParticleCellState.Southeast))
                outState |= FHPParticleCellState.Northwest;
            if (inState.HasFlag(FHPParticleCellState.Southwest))
                outState |= FHPParticleCellState.Northeast;
            if (inState.HasFlag(FHPParticleCellState.West))
                outState |= FHPParticleCellState.East;
            if (inState.HasFlag(FHPParticleCellState.Northwest))
                outState |= FHPParticleCellState.Southeast;
            if (inState.HasFlag(FHPParticleCellState.Rest)) // This shouldn't happen.
                outState |= FHPParticleCellState.Rest;

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
                    FHPParticleCellState particleState;
                    if (x == 0 || y == 0 || x == extendedWidth - 1 || y == extendedHeight - 1)
                        particleState = FHPParticleCellState.Wall;
                    else
                        particleState = FHPParticleCellState.None;
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

        public FHPCellState GetCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State;

        public void SetCellState(FHPCellState cellState, int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State = cellState;

        public ColorValues GetColorValues(FHPCellState cellState) =>
            FHPCell.GetColorValues(cellState);

        public void SetColorValues(ColorValues colorValues, FHPCellState cellState) =>
            FHPCell.SetColorValues(colorValues, cellState);

        public ColorValues[] GetCellsColors() =>
            FHPCell.GetCellsColors();

        public FHPParticleCellState GetParticleCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].ParticleState;

        public void SetParticleCellState(FHPParticleCellState cellState, int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].ParticleState = cellState;

        private void HandleCollisions()
        {
            int extendedWidth = Width + borderThickness * 2;
            int extendedHeight = Height + borderThickness * 2;
            
            for (int x = 0; x < extendedWidth; x++)
            {
                for (int y = 0; y < extendedHeight; y++)
                {
                    FHPParticleCellState particleInStateXY = cellsGrid[x][y].ParticleState;
                    int randomOutStateIndex = Randomizer.Instance.Next(2);
                    cellsGrid[x][y].ParticleState = outStatesPair[randomOutStateIndex][(int)particleInStateXY];
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
                    FHPParticleCellState particleInStateXY = cellXY.ParticleState;
                    
                    if (particleInStateXY.HasFlag(FHPParticleCellState.Wall))
                        newCellsGrid[x][y].ParticleState |= FHPParticleCellState.Wall;
                    if (particleInStateXY.HasFlag(FHPParticleCellState.Rest))
                        newCellsGrid[x][y].ParticleState |= FHPParticleCellState.Rest;
                    if (particleInStateXY.HasFlag(FHPParticleCellState.Northeast))
                        newCellsGrid[diagonalRightIndex][y - 1].ParticleState |= FHPParticleCellState.Northeast;
                    if (particleInStateXY.HasFlag(FHPParticleCellState.East))
                        newCellsGrid[x + 1][y].ParticleState |= FHPParticleCellState.East;
                    if (particleInStateXY.HasFlag(FHPParticleCellState.Southeast))
                        newCellsGrid[diagonalRightIndex][y + 1].ParticleState |= FHPParticleCellState.Southeast;
                    if (particleInStateXY.HasFlag(FHPParticleCellState.Southwest))
                        newCellsGrid[diagonalLeftIndex][y + 1].ParticleState |= FHPParticleCellState.Southwest;
                    if (particleInStateXY.HasFlag(FHPParticleCellState.West))
                        newCellsGrid[x - 1][y].ParticleState |= FHPParticleCellState.West;
                    if (particleInStateXY.HasFlag(FHPParticleCellState.Northwest))
                        newCellsGrid[diagonalLeftIndex][y - 1].ParticleState |= FHPParticleCellState.Northwest;

                    cellXY.ParticleState = FHPParticleCellState.None;
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
