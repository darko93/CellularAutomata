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

        private static Randomizer randomizer = Randomizer.Instance;

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
            FHPParticleState outState = FHPParticleState.None;

            if (inState.HasFlag(FHPParticleState.NE))
                outState |= FHPParticleState.SW;
            if (inState.HasFlag(FHPParticleState.E))
                outState |= FHPParticleState.W;
            if (inState.HasFlag(FHPParticleState.SE))
                outState |= FHPParticleState.NW;
            if (inState.HasFlag(FHPParticleState.SW))
                outState |= FHPParticleState.NE;
            if (inState.HasFlag(FHPParticleState.E))
                outState |= FHPParticleState.W;
            if (inState.HasFlag(FHPParticleState.NW))
                outState |= FHPParticleState.SE;

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
            cellsGrid[x + borderThickness][y + borderThickness].GetState();

        public void SetCellState(int x, int y, FHPCellState cellState) =>
            cellsGrid[x + borderThickness][y + borderThickness].SetState(cellState);

        public ColorValues GetColorValues(FHPCellState cellState) =>
            FHPCell.GetColorValues(cellState);

        public void SetColorValues(FHPCellState cellState, ColorValues colorValues) =>
            FHPCell.SetColorValues(cellState, colorValues);

        public void SetRandomCellsState(int cellsPercentage, FHPCellState cellState, Point leftBottomBound, Point rightTopBound)
        {
            Point[] randomPoints = randomizer.GetDistinctRandomPoints(cellsPercentage, leftBottomBound, rightTopBound);

            foreach (Point point in randomPoints)
                cellsGrid[point.X + borderThickness][point.Y + borderThickness].SetState(cellState);
        }

        public void SetRandomCellsState(int cellsPercentage, FHPCellState cellState)
        {
            Point leftBottomBound = new Point(0, 0);
            Point rightTopBound = new Point(Width - 1, Height - 1);
            SetRandomCellsState(cellsPercentage, cellState, leftBottomBound, rightTopBound);
        }

        public void SetRandomCellsState(int cellsPercentage, FHPCellState cellState, Point location, int width, int height)
        {
            double halfWidth = width * 0.5;
            int leftBoundX = location.X - (int)Math.Floor(halfWidth);
            int rightBoundX = location.X + (int)Math.Ceiling(halfWidth);

            double halfHeight = height * 0.5;
            int bottomBoundY = location.Y - (int)Math.Floor(halfHeight);
            int topBoundY = location.Y + (int)Math.Floor(halfHeight);

            Point leftBottomBound = new Point(leftBoundX, bottomBoundY);
            Point rightTopBound = new Point(rightBoundX, topBoundY);

            SetRandomCellsState(cellsPercentage, cellState, leftBottomBound, rightTopBound);
        }

        private void HandleCollisions()
        {
            int extendedWidth = Width + borderThickness * 2;
            int extendedHeight = Height + borderThickness * 2;
            
            for (int x = 0; x < extendedWidth; x++)
            {
                for (int y = 0; y < extendedHeight; y++)
                {
                    FHPParticleState xyParticleInState = cellsGrid[x][y].ParticleState;
                    int randomOutStateIndex = randomizer.Next(2);
                    cellsGrid[x][y].ParticleState = outState[(int)xyParticleInState][randomOutStateIndex];
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

                    FHPParticleState xyParticleInState = cellsGrid[x][y].ParticleState;

                    if (xyParticleInState.HasFlag(FHPParticleState.Wall))
                        newCellsGrid[x][y].ParticleState |= FHPParticleState.Wall;
                    if (xyParticleInState.HasFlag(FHPParticleState.Rest))
                        newCellsGrid[x][y].ParticleState |= FHPParticleState.Rest;
                    if (xyParticleInState.HasFlag(FHPParticleState.NE))
                        newCellsGrid[diagonalRightIndex][y - 1].ParticleState |= FHPParticleState.NE;
                    if (xyParticleInState.HasFlag(FHPParticleState.E))
                        newCellsGrid[x + 1][y].ParticleState |= FHPParticleState.E;
                    if (xyParticleInState.HasFlag(FHPParticleState.SE))
                        newCellsGrid[diagonalRightIndex][y + 1].ParticleState |= FHPParticleState.SE;
                    if (xyParticleInState.HasFlag(FHPParticleState.SW))
                        newCellsGrid[diagonalLeftIndex][y + 1].ParticleState |= FHPParticleState.SW;
                    if (xyParticleInState.HasFlag(FHPParticleState.W))
                        newCellsGrid[x - 1][y].ParticleState |= FHPParticleState.W;
                    if (xyParticleInState.HasFlag(FHPParticleState.NW))
                        newCellsGrid[diagonalLeftIndex][y - 1].ParticleState |= FHPParticleState.NW;
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
