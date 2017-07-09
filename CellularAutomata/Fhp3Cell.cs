namespace CellularAutomata
{
    class Fhp3Cell : ICell<Fhp3CellState>
    {
        private static ColorValues airColor = ColorValues.Air;
        private static ColorValues particleColor = ColorValues.Particle;
        private static ColorValues wallColor = ColorValues.Wall;

        public Fhp3ParticleCellState ParticleState { get; set; }

        public Fhp3CellState State
        {
            get
            {
                if (ParticleState == Fhp3ParticleCellState.None)
                    return Fhp3CellState.Empty;
                if (ParticleState.HasFlag(Fhp3ParticleCellState.Wall))
                    return Fhp3CellState.Wall;
                return Fhp3CellState.Particle;
            }
            set
            {
                SetState(value);
            }
        }

        public Fhp3Cell(Fhp3ParticleCellState particleState)
        {
            ParticleState = particleState;
        }

        public Fhp3Cell(Fhp3CellState cellState)
        {
            SetState(cellState);
        }

        public void SetState(Fhp3CellState state)
        {
            if (state == Fhp3CellState.Empty)
                ParticleState = Fhp3ParticleCellState.None;
            else if (state == Fhp3CellState.Wall)
                ParticleState = Fhp3ParticleCellState.Wall;
            else // if (state == Fhp3CellState.Particle)
            {
                int randomDirectionNumber = Randomizer.Instance.Next(7);
                switch (randomDirectionNumber)
                {
                    case 0:
                        ParticleState = Fhp3ParticleCellState.Northeast;
                        break;
                    case 1:
                        ParticleState = Fhp3ParticleCellState.East;
                        break;
                    case 2:
                        ParticleState = Fhp3ParticleCellState.Southeast;
                        break;
                    case 3:
                        ParticleState = Fhp3ParticleCellState.Southwest;
                        break;
                    case 4:
                        ParticleState = Fhp3ParticleCellState.West;
                        break;
                    case 5:
                        ParticleState = Fhp3ParticleCellState.Northwest;
                        break;
                    case 6:
                        ParticleState = Fhp3ParticleCellState.Rest;
                        break;
                }
            }
        }

        public ColorValues GetColorValues()
        {
            if (ParticleState == Fhp3ParticleCellState.None)
                return airColor;
            if (ParticleState.HasFlag(Fhp3ParticleCellState.Wall))
                return wallColor;
            return particleColor;
        }
        
        public static ColorValues GetColorValues(Fhp3CellState cellState)
        {
            switch (cellState)
            {
                case Fhp3CellState.Empty:
                    return airColor;
                case Fhp3CellState.Particle:
                    return particleColor;
                case Fhp3CellState.Wall:
                    return wallColor;
                default:
                    return null;
            }
        }

        public static void SetColorValues(ColorValues colorValues, Fhp3CellState cellState)
        {
            switch (cellState)
            {
                case Fhp3CellState.Empty:
                    airColor = colorValues;
                    break;
                case Fhp3CellState.Particle:
                    particleColor = colorValues;
                    break;
                case Fhp3CellState.Wall:
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
