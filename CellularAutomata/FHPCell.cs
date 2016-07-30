using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    class FHPCell
    {
        private static ColorValues emptyCellColor = ColorValues.Air;
        private static ColorValues particleColor = ColorValues.Particle;
        private static ColorValues wallColor = ColorValues.Wall;

        private static Randomizer randomizer = Randomizer.Instance;

        public FHPParticleState ParticleState { get; set; }

        public FHPCell(FHPParticleState particleState)
        {
            ParticleState = particleState;
        }

        public FHPCell(FHPCellState cellState)
        {
            SetState(cellState);
        }

        public FHPCellState GetState()
        {
            if (ParticleState == FHPParticleState.None)
                return FHPCellState.Empty;
            if (ParticleState.HasFlag(FHPParticleState.Wall))
                return FHPCellState.Wall;
            return FHPCellState.Particle;
        }

        public void SetState(FHPCellState state)
        {
            if (state == FHPCellState.Empty)
                ParticleState = FHPParticleState.None;
            else if (state == FHPCellState.Wall)
                ParticleState = FHPParticleState.Wall;
            else // if (state == FHPCellState.Particle)
            {
                int randomDirectionNumber = randomizer.Next(6);
                switch (randomDirectionNumber)
                {
                    case 0:
                        ParticleState = FHPParticleState.NE;
                        break;
                    case 1:
                        ParticleState = FHPParticleState.E;
                        break;
                    case 2:
                        ParticleState = FHPParticleState.SE;
                        break;
                    case 3:
                        ParticleState = FHPParticleState.SW;
                        break;
                    case 4:
                        ParticleState = FHPParticleState.W;
                        break;
                    case 5:
                        ParticleState = FHPParticleState.NW;
                        break;
                }
            }
        }

        public ColorValues GetColorValues()
        {
            if (ParticleState == FHPParticleState.None)
                return emptyCellColor;
            if (ParticleState.HasFlag(FHPParticleState.Wall))
                return wallColor;
            return particleColor;
        }

        public static ColorValues[] GetCellsColors() =>
            new[]
            {
                emptyCellColor,
                particleColor,
                wallColor
            };
        
        public static ColorValues GetColorValues(FHPCellState cellState)
        {
            switch (cellState)
            {
                case FHPCellState.Empty:
                    return emptyCellColor;
                case FHPCellState.Particle:
                    return particleColor;
                case FHPCellState.Wall:
                    return wallColor;
                default:
                    return null;
            }
        }

        public static void SetColorValues(FHPCellState cellState, ColorValues colorValues)
        {
            switch (cellState)
            {
                case FHPCellState.Empty:
                    emptyCellColor = colorValues;
                    break;
                case FHPCellState.Particle:
                    particleColor = colorValues;
                    break;
                case FHPCellState.Wall:
                    particleColor = colorValues;
                    break;
            }
        }
    }
}
