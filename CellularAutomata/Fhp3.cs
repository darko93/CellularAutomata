namespace CellularAutomata
{
    public class Fhp3 : ICellularAutomaton, IStatedCellularAutomaton<Fhp3CellState>
    {
        private static Fhp3ParticleCellState[] outState1 = null;
        private static Fhp3ParticleCellState[] outState2 = null;
        private static Fhp3ParticleCellState[][] outStatesPair = null;

        private Fhp3Cell[][] cellsGrid = null;
        private Fhp3Cell[][] newCellsGrid = null;

        private const int borderThickness = 1;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Fhp3(int width, int height)
        {
            if (outState1 == null || outState2 == null || outStatesPair == null)
                InitializeOutStates();
            Reinitialize(width, height);
        }

        private void InitializeOutStates()
        {
            outState1 = new Fhp3ParticleCellState[256];
            outState2 = new Fhp3ParticleCellState[256];
            outStatesPair = new Fhp3ParticleCellState[2][]
            {
                outState1,
                outState2
            };

            // Initialize non-boundary part of collisions array.
            for (int i = 0; i < 128; i++)
            {
                Fhp3ParticleCellState iInState = (Fhp3ParticleCellState)i;
                outState1[i] = outState2[i] = iInState;
            }

            // Setting non-boundary collisions.
            Fhp3Collision[] fhp3Collisions = XmlManager.Instance.GetFhp3Collisions();
            foreach (Fhp3Collision fhp3Collision in fhp3Collisions)
            {
                outState1[fhp3Collision.InState] = (Fhp3ParticleCellState)fhp3Collision.OutState1;
                outState2[fhp3Collision.InState] = (Fhp3ParticleCellState)fhp3Collision.OutState2;
            }

            // Initialize boundary part of collisions array and setting boundary collisions.
            for (int i = 128; i < 256; i++)
            {
                Fhp3ParticleCellState iInState = (Fhp3ParticleCellState)i;
                Fhp3ParticleCellState iOutState = GetBoundaryOutState(iInState);
                outState1[i] = outState2[i] = iOutState;
            }
        }

        private Fhp3ParticleCellState GetBoundaryOutState(Fhp3ParticleCellState inState)
        {
            Fhp3ParticleCellState outState = Fhp3ParticleCellState.Wall;

            if (inState.HasFlag(Fhp3ParticleCellState.Northeast))
                outState |= Fhp3ParticleCellState.Southwest;
            if (inState.HasFlag(Fhp3ParticleCellState.East))
                outState |= Fhp3ParticleCellState.West;
            if (inState.HasFlag(Fhp3ParticleCellState.Southeast))
                outState |= Fhp3ParticleCellState.Northwest;
            if (inState.HasFlag(Fhp3ParticleCellState.Southwest))
                outState |= Fhp3ParticleCellState.Northeast;
            if (inState.HasFlag(Fhp3ParticleCellState.West))
                outState |= Fhp3ParticleCellState.East;
            if (inState.HasFlag(Fhp3ParticleCellState.Northwest))
                outState |= Fhp3ParticleCellState.Southeast;
            if (inState.HasFlag(Fhp3ParticleCellState.Rest)) // This shouldn't happen.
                outState |= Fhp3ParticleCellState.Rest;

            return outState;
        }

        public void Reinitialize(int width, int height)
        {
            int extendedWidth = width + borderThickness * 2;
            int extendedHeight = height + borderThickness * 2;

            cellsGrid = new Fhp3Cell[extendedWidth][];
            newCellsGrid = new Fhp3Cell[extendedWidth][];
            for (int x = 0; x < extendedWidth; x++)
            {
                cellsGrid[x] = new Fhp3Cell[extendedHeight];
                newCellsGrid[x] = new Fhp3Cell[extendedHeight];
                for (int y = 0; y < extendedHeight; y++)
                {
                    Fhp3ParticleCellState particleState;
                    if (x == 0 || y == 0 || x == extendedWidth - 1 || y == extendedHeight - 1)
                        particleState = Fhp3ParticleCellState.Wall;
                    else
                        particleState = Fhp3ParticleCellState.None;
                    cellsGrid[x][y] = new Fhp3Cell(particleState);
                    newCellsGrid[x][y] = new Fhp3Cell(particleState);
                }
            }
            Width = width;
            Height = height;
        }

        public void Reinitialize() =>
            Reinitialize(Width, Height);

        public ColorValues GetColorValues(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].GetColorValues();

        public Fhp3CellState GetCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State;

        public void SetCellState(Fhp3CellState cellState, int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].State = cellState;

        public ColorValues GetColorValues(Fhp3CellState cellState) =>
            Fhp3Cell.GetColorValues(cellState);

        public void SetColorValues(ColorValues colorValues, Fhp3CellState cellState) =>
            Fhp3Cell.SetColorValues(colorValues, cellState);

        public ColorValues[] GetCellsColors() =>
            Fhp3Cell.GetCellsColors();

        public Fhp3ParticleCellState GetParticleCellState(int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].ParticleState;

        public void SetParticleCellState(Fhp3ParticleCellState cellState, int x, int y) =>
            cellsGrid[x + borderThickness][y + borderThickness].ParticleState = cellState;

        private void HandleCollisions()
        {
            int extendedWidth = Width + borderThickness * 2;
            int extendedHeight = Height + borderThickness * 2;
            
            for (int x = 0; x < extendedWidth; x++)
            {
                for (int y = 0; y < extendedHeight; y++)
                {
                    Fhp3ParticleCellState particleInStateXY = cellsGrid[x][y].ParticleState;
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

                    Fhp3Cell cellXY = cellsGrid[x][y];
                    Fhp3ParticleCellState particleInStateXY = cellXY.ParticleState;
                    
                    if (particleInStateXY.HasFlag(Fhp3ParticleCellState.Wall))
                        newCellsGrid[x][y].ParticleState |= Fhp3ParticleCellState.Wall;
                    if (particleInStateXY.HasFlag(Fhp3ParticleCellState.Rest))
                        newCellsGrid[x][y].ParticleState |= Fhp3ParticleCellState.Rest;
                    if (particleInStateXY.HasFlag(Fhp3ParticleCellState.Northeast))
                        newCellsGrid[diagonalRightIndex][y - 1].ParticleState |= Fhp3ParticleCellState.Northeast;
                    if (particleInStateXY.HasFlag(Fhp3ParticleCellState.East))
                        newCellsGrid[x + 1][y].ParticleState |= Fhp3ParticleCellState.East;
                    if (particleInStateXY.HasFlag(Fhp3ParticleCellState.Southeast))
                        newCellsGrid[diagonalRightIndex][y + 1].ParticleState |= Fhp3ParticleCellState.Southeast;
                    if (particleInStateXY.HasFlag(Fhp3ParticleCellState.Southwest))
                        newCellsGrid[diagonalLeftIndex][y + 1].ParticleState |= Fhp3ParticleCellState.Southwest;
                    if (particleInStateXY.HasFlag(Fhp3ParticleCellState.West))
                        newCellsGrid[x - 1][y].ParticleState |= Fhp3ParticleCellState.West;
                    if (particleInStateXY.HasFlag(Fhp3ParticleCellState.Northwest))
                        newCellsGrid[diagonalLeftIndex][y - 1].ParticleState |= Fhp3ParticleCellState.Northwest;

                    cellXY.ParticleState = Fhp3ParticleCellState.None;
                }
            }
        }

        private void SwapCellsGrid()
        {
            Fhp3Cell[][] tempCellsGrid = cellsGrid;
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
