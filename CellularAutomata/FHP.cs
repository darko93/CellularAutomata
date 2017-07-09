namespace CellularAutomata
{
    public class Fhp : ICellularAutomaton, IStatedCellularAutomaton<FhpCellState>
    {
        private static FhpParticleCellState[] outState1 = null;
        private static FhpParticleCellState[] outState2 = null;
        private static FhpParticleCellState[][] outStatesPair = null;

        private FhpCell[][] cellsGrid = null;
        private FhpCell[][] newCellsGrid = null;

        private const int borderThickness = 1;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Fhp(int width, int height)
        {
            if (outState1 == null || outState2 == null || outStatesPair == null)
                InitializeOutStates();
            Reinitialize(width, height);
        }

        private void InitializeOutStates()
        {
            outState1 = new FhpParticleCellState[256];
            outState2 = new FhpParticleCellState[256];
            outStatesPair = new FhpParticleCellState[2][]
            {
                outState1,
                outState2
            };

            // Initialize non-boundary part of collisions array.
            for (int i = 0; i < 128; i++)
            {
                FhpParticleCellState iInState = (FhpParticleCellState)i;
                outState1[i] = outState2[i] = iInState;
            }

            // Setting non-boundary collisions.
            FhpCollision[] fhpCollisions = XmlManager.Instance.GetFhpCollisions();
            foreach (FhpCollision fhpCollision in fhpCollisions)
            {
                outState1[fhpCollision.InState] = (FhpParticleCellState)fhpCollision.OutState1;
                outState2[fhpCollision.InState] = (FhpParticleCellState)fhpCollision.OutState2;
            }

            // Initialize boundary part of collisions array and setting boundary collisions.
            for (int i = 128; i < 256; i++)
            {
                FhpParticleCellState iInState = (FhpParticleCellState)i;
                FhpParticleCellState iOutState = GetBoundaryOutState(iInState);
                outState1[i] = outState2[i] = iOutState;
            }
        }

        private FhpParticleCellState GetBoundaryOutState(FhpParticleCellState inState)
        {
            FhpParticleCellState outState = FhpParticleCellState.Wall;

            if (inState.HasFlag(FhpParticleCellState.Northeast))
                outState |= FhpParticleCellState.Southwest;
            if (inState.HasFlag(FhpParticleCellState.East))
                outState |= FhpParticleCellState.West;
            if (inState.HasFlag(FhpParticleCellState.Southeast))
                outState |= FhpParticleCellState.Northwest;
            if (inState.HasFlag(FhpParticleCellState.Southwest))
                outState |= FhpParticleCellState.Northeast;
            if (inState.HasFlag(FhpParticleCellState.West))
                outState |= FhpParticleCellState.East;
            if (inState.HasFlag(FhpParticleCellState.Northwest))
                outState |= FhpParticleCellState.Southeast;
            if (inState.HasFlag(FhpParticleCellState.Rest)) // This shouldn't happen.
                outState |= FhpParticleCellState.Rest;

            return outState;
        }

        public void Reinitialize(int width, int height)
        {
            int extendedWidth = width + borderThickness * 2;
            int extendedHeight = height + borderThickness * 2;

            cellsGrid = new FhpCell[extendedWidth][];
            newCellsGrid = new FhpCell[extendedWidth][];
            for (int x = 0; x < extendedWidth; x++)
            {
                cellsGrid[x] = new FhpCell[extendedHeight];
                newCellsGrid[x] = new FhpCell[extendedHeight];
                for (int y = 0; y < extendedHeight; y++)
                {
                    FhpParticleCellState particleState;
                    if (x == 0 || y == 0 || x == extendedWidth - 1 || y == extendedHeight - 1)
                        particleState = FhpParticleCellState.Wall;
                    else
                        particleState = FhpParticleCellState.None;
                    cellsGrid[x][y] = new FhpCell(particleState);
                    newCellsGrid[x][y] = new FhpCell(particleState);
                }
            }
            Width = width;
            Height = height;
        }

        public void Reinitialize() =>
            Reinitialize(Width, Height);

        public ColorValues GetColorValues(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].GetColorValues();

        public FhpCellState GetCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State;

        public void SetCellState(FhpCellState cellState, int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State = cellState;

        public ColorValues GetColorValues(FhpCellState cellState) =>
            FhpCell.GetColorValues(cellState);

        public void SetColorValues(ColorValues colorValues, FhpCellState cellState) =>
            FhpCell.SetColorValues(colorValues, cellState);

        public ColorValues[] GetCellsColors() =>
            FhpCell.GetCellsColors();

        public FhpParticleCellState GetParticleCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].ParticleState;

        public void SetParticleCellState(FhpParticleCellState cellState, int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].ParticleState = cellState;

        private void HandleCollisions()
        {
            int extendedWidth = Width + borderThickness * 2;
            int extendedHeight = Height + borderThickness * 2;
            
            for (int x = 0; x < extendedWidth; x++)
            {
                for (int y = 0; y < extendedHeight; y++)
                {
                    FhpParticleCellState particleInStateXY = cellsGrid[x][y].ParticleState;
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

                    FhpCell cellXY = cellsGrid[x][y];
                    FhpParticleCellState particleInStateXY = cellXY.ParticleState;
                    
                    if (particleInStateXY.HasFlag(FhpParticleCellState.Wall))
                        newCellsGrid[x][y].ParticleState |= FhpParticleCellState.Wall;
                    if (particleInStateXY.HasFlag(FhpParticleCellState.Rest))
                        newCellsGrid[x][y].ParticleState |= FhpParticleCellState.Rest;
                    if (particleInStateXY.HasFlag(FhpParticleCellState.Northeast))
                        newCellsGrid[diagonalRightIndex][y - 1].ParticleState |= FhpParticleCellState.Northeast;
                    if (particleInStateXY.HasFlag(FhpParticleCellState.East))
                        newCellsGrid[x + 1][y].ParticleState |= FhpParticleCellState.East;
                    if (particleInStateXY.HasFlag(FhpParticleCellState.Southeast))
                        newCellsGrid[diagonalRightIndex][y + 1].ParticleState |= FhpParticleCellState.Southeast;
                    if (particleInStateXY.HasFlag(FhpParticleCellState.Southwest))
                        newCellsGrid[diagonalLeftIndex][y + 1].ParticleState |= FhpParticleCellState.Southwest;
                    if (particleInStateXY.HasFlag(FhpParticleCellState.West))
                        newCellsGrid[x - 1][y].ParticleState |= FhpParticleCellState.West;
                    if (particleInStateXY.HasFlag(FhpParticleCellState.Northwest))
                        newCellsGrid[diagonalLeftIndex][y - 1].ParticleState |= FhpParticleCellState.Northwest;

                    cellXY.ParticleState = FhpParticleCellState.None;
                }
            }
        }

        private void SwapCellsGrid()
        {
            FhpCell[][] tempCellsGrid = cellsGrid;
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
