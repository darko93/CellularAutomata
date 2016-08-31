namespace CellularAutomata
{
    class FHPCell : ICell<FHPCellState>
    {
        private static ColorValues airColor = ColorValues.Air;
        private static ColorValues particleColor = ColorValues.Particle;
        private static ColorValues wallColor = ColorValues.Wall;

        public FHPParticleCellState ParticleState { get; set; }

        public FHPCellState State
        {
            get
            {
                if (ParticleState == FHPParticleCellState.None)
                    return FHPCellState.Empty;
                if (ParticleState.HasFlag(FHPParticleCellState.Wall))
                    return FHPCellState.Wall;
                return FHPCellState.Particle;
            }
            set
            {
                SetState(value);
            }
        }

        public FHPCell(FHPParticleCellState particleState)
        {
            ParticleState = particleState;
        }

        public FHPCell(FHPCellState cellState)
        {
            SetState(cellState);
        }

        public void SetState(FHPCellState state)
        {
            if (state == FHPCellState.Empty)
                ParticleState = FHPParticleCellState.None;
            else if (state == FHPCellState.Wall)
                ParticleState = FHPParticleCellState.Wall;
            else // if (state == FHPCellState.Particle)
            {
                int randomDirectionNumber = Randomizer.Instance.Next(7);
                switch (randomDirectionNumber)
                {
                    case 0:
                        ParticleState = FHPParticleCellState.Northeast;
                        break;
                    case 1:
                        ParticleState = FHPParticleCellState.East;
                        break;
                    case 2:
                        ParticleState = FHPParticleCellState.Southeast;
                        break;
                    case 3:
                        ParticleState = FHPParticleCellState.Southwest;
                        break;
                    case 4:
                        ParticleState = FHPParticleCellState.West;
                        break;
                    case 5:
                        ParticleState = FHPParticleCellState.Northwest;
                        break;
                    case 6:
                        ParticleState = FHPParticleCellState.Rest;
                        break;
                }
            }
        }

        public ColorValues GetColorValues()
        {
            if (ParticleState == FHPParticleCellState.None)
                return airColor;
            if (ParticleState.HasFlag(FHPParticleCellState.Wall))
                return wallColor;
            return particleColor;
        }
        
        public static ColorValues GetColorValues(FHPCellState cellState)
        {
            switch (cellState)
            {
                case FHPCellState.Empty:
                    return airColor;
                case FHPCellState.Particle:
                    return particleColor;
                case FHPCellState.Wall:
                    return wallColor;
                default:
                    return null;
            }
        }

        public static void SetColorValues(ColorValues colorValues, FHPCellState cellState)
        {
            switch (cellState)
            {
                case FHPCellState.Empty:
                    airColor = colorValues;
                    break;
                case FHPCellState.Particle:
                    particleColor = colorValues;
                    break;
                case FHPCellState.Wall:
                    particleColor = colorValues;
                    break;
            }
        }

        public static ColorValues[] GetCellsColors() =>
            new[]
            {
                airColor,
                particleColor,
                wallColor
            };
    }
}
