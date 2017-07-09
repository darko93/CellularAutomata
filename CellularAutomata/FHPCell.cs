namespace CellularAutomata
{
    class FhpCell : ICell<FhpCellState>
    {
        private static ColorValues airColor = ColorValues.Air;
        private static ColorValues particleColor = ColorValues.Particle;
        private static ColorValues wallColor = ColorValues.Wall;

        public FhpParticleCellState ParticleState { get; set; }

        public FhpCellState State
        {
            get
            {
                if (ParticleState == FhpParticleCellState.None)
                    return FhpCellState.Empty;
                if (ParticleState.HasFlag(FhpParticleCellState.Wall))
                    return FhpCellState.Wall;
                return FhpCellState.Particle;
            }
            set
            {
                SetState(value);
            }
        }

        public FhpCell(FhpParticleCellState particleState)
        {
            ParticleState = particleState;
        }

        public FhpCell(FhpCellState cellState)
        {
            SetState(cellState);
        }

        public void SetState(FhpCellState state)
        {
            if (state == FhpCellState.Empty)
                ParticleState = FhpParticleCellState.None;
            else if (state == FhpCellState.Wall)
                ParticleState = FhpParticleCellState.Wall;
            else // if (state == FhpCellState.Particle)
            {
                int randomDirectionNumber = Randomizer.Instance.Next(7);
                switch (randomDirectionNumber)
                {
                    case 0:
                        ParticleState = FhpParticleCellState.Northeast;
                        break;
                    case 1:
                        ParticleState = FhpParticleCellState.East;
                        break;
                    case 2:
                        ParticleState = FhpParticleCellState.Southeast;
                        break;
                    case 3:
                        ParticleState = FhpParticleCellState.Southwest;
                        break;
                    case 4:
                        ParticleState = FhpParticleCellState.West;
                        break;
                    case 5:
                        ParticleState = FhpParticleCellState.Northwest;
                        break;
                    case 6:
                        ParticleState = FhpParticleCellState.Rest;
                        break;
                }
            }
        }

        public ColorValues GetColorValues()
        {
            if (ParticleState == FhpParticleCellState.None)
                return airColor;
            if (ParticleState.HasFlag(FhpParticleCellState.Wall))
                return wallColor;
            return particleColor;
        }
        
        public static ColorValues GetColorValues(FhpCellState cellState)
        {
            switch (cellState)
            {
                case FhpCellState.Empty:
                    return airColor;
                case FhpCellState.Particle:
                    return particleColor;
                case FhpCellState.Wall:
                    return wallColor;
                default:
                    return null;
            }
        }

        public static void SetColorValues(ColorValues colorValues, FhpCellState cellState)
        {
            switch (cellState)
            {
                case FhpCellState.Empty:
                    airColor = colorValues;
                    break;
                case FhpCellState.Particle:
                    particleColor = colorValues;
                    break;
                case FhpCellState.Wall:
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
