using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class FHP
    {
        private FHPParticleState[][] outState = null;

        private FHPCell[][] cellsGrid = null;

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
            for (int i = 0; i < 256; i++)
            {
                FHPParticleState iState = (FHPParticleState)i;
                outState[i] = new[] { iState, iState };
            }

            FHPCollision[] fhpCollisions = XMLManager.Instance.GetFHPCollisions();

            foreach (FHPCollision fhpCollision in fhpCollisions)
            {
                FHPParticleState[] fhpOutStatesPair = outState[fhpCollision.InState];
                fhpOutStatesPair[0] = (FHPParticleState)fhpCollision.OutState1;
                fhpOutStatesPair[1] = (FHPParticleState)fhpCollision.OutState2;
            }
        }

        public void Reinitialize(int width, int height)
        {
            int extendedWidth = width + borderThickness * 2;
            int extendedHeight = height + borderThickness * 2;

            cellsGrid = new FHPCell[extendedWidth][];
            for (int x = 0; x < extendedWidth; x++)
            {
                cellsGrid[x] = new FHPCell[extendedHeight];
                for (int y = 0; y < extendedHeight; y++)
                {
                    FHPCellState cellState;
                    if (x == 0 || y == 0 || x == extendedWidth - 1 || y == extendedHeight - 1)
                        cellState = FHPCellState.Wall;
                    else
                        cellState = FHPCellState.Particle;

                    cellsGrid[x][y] = new FHPCell(cellState);
                }
            }
            Width = width;
            Height = height;
        }

        public void NextStep()
        {

        }
    }
}
