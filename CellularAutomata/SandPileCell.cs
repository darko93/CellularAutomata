using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    class SandPileCell : ICell<SandPileCellState>
    {
        private static ColorValues sandColor = ColorValues.Sand;
        private static ColorValues airColor = ColorValues.Air;
        private static ColorValues wallColor = ColorValues.Wall;

        private static Randomizer randomizer = Randomizer.Instance;
        private const int colorRGBDeviation = 50;

        private ColorValues deviatedSandColor = null;

        public SandPileCellState State { get; set; }

        public SandPileCell(SandPileCellState cellState, ColorMode sandColorMode)
        {
            State = cellState;
            SetSandColorMode(sandColorMode);
        }

        public ColorValues GetColorValues()
        {
            switch (State)
            {
                case SandPileCellState.SandGrain:
                    return deviatedSandColor;
                case SandPileCellState.Empty:
                    return airColor;
                case SandPileCellState.Wall:
                    return wallColor;
                default:
                    return null;
            }
        }

        public void SetSandColorMode(ColorMode sandColorMode)
        {
            if (sandColorMode == ColorMode.SlightlyDifferent)
                ReinitializeDeviatedSandColor();
            else // if (sandColorMode == ColorMode.Uniform)
                deviatedSandColor = sandColor;
        }

        public void ReinitializeDeviatedSandColor()
        {
            int redDeviation = randomizer.Next(-colorRGBDeviation, colorRGBDeviation);
            int greenDeviation = randomizer.Next(-colorRGBDeviation, colorRGBDeviation);
            int blueDeviation = randomizer.Next(-colorRGBDeviation, colorRGBDeviation);
            byte deviatedRed = GetByteBoundColorValue(sandColor.Red + redDeviation);
            byte deviatedGreen = GetByteBoundColorValue(sandColor.Green + greenDeviation);
            byte deviatedBlue = GetByteBoundColorValue(sandColor.Blue + blueDeviation);
            deviatedSandColor = new ColorValues(deviatedRed, deviatedGreen, deviatedBlue);
        }

        private byte GetByteBoundColorValue(int colorValue)
        {
            if (colorValue < 0)
                return 0;
            if (colorValue > 255)
                return 255;
            return (byte)colorValue;
        }

        public void FallTo(SandPileCell lowerCell)
        {
            ColorValues tempThisDeviatedSandColor = deviatedSandColor;

            State = SandPileCellState.Empty;
            deviatedSandColor = lowerCell.deviatedSandColor;

            lowerCell.State = SandPileCellState.SandGrain;
            lowerCell.deviatedSandColor = tempThisDeviatedSandColor;
        }

        public static ColorValues GetColorValues(SandPileCellState cellState)
        {
            switch (cellState)
            {
                case SandPileCellState.Empty:
                    return airColor;
                case SandPileCellState.SandGrain:
                    return sandColor;
                case SandPileCellState.Wall:
                    return wallColor;
                default:
                    return null;
            }
        }

        public static void SetColorValues(ColorValues colorValues, SandPileCellState cellState)
        {
            switch (cellState)
            {
                case SandPileCellState.SandGrain:
                    sandColor = colorValues;
                    break;
                case SandPileCellState.Empty:
                    airColor = colorValues;
                    break;
                case SandPileCellState.Wall:
                    wallColor = colorValues;
                    break;
            }
        }

        public static ColorValues[] GetColors() => // Gets only basic sand color.
            new[]
            {
                sandColor,
                airColor,
                wallColor
            };
    }
}
