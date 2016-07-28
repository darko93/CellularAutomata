using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    class SandPileCell
    {
        private const int colorRGBDeviation = 30;
        private ColorValues deviatedSandColor = null;
        private static Randomizer randomizer = Randomizer.Instance;

        private static ColorValues sandColor = ColorValues.Sand;
        private static ColorValues airColor = ColorValues.Air;
        private static ColorValues wallColor = ColorValues.Wall;

        public SandPileCellState State { get; set; }

        public ColorValues GetColorValues()
        {
            switch (State)
            {
                case SandPileCellState.Sand:
                    return deviatedSandColor;
                case SandPileCellState.Air:
                    return airColor;
                case SandPileCellState.Wall:
                    return wallColor;
                default: return null;
            }
        }

        public static void SetColorValues(SandPileCellState cellState, ColorValues colorValues)
        {
            switch (cellState)
            {
                case SandPileCellState.Sand:
                    sandColor = colorValues;
                    break;
                case SandPileCellState.Air:
                    airColor = colorValues;
                    break;
                case SandPileCellState.Wall:
                    wallColor = colorValues;
                    break;
            }
        }

        public SandPileCell(SandPileCellState cellState, ColorMode sandColorMode)
        {
            State = cellState;
            SetSandColorMode(sandColorMode);
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
            byte deviatedRed = GetByteBoundColorValue(sandColor.R + redDeviation);
            byte deviatedGreen = GetByteBoundColorValue(sandColor.G + greenDeviation);
            byte deviatedBlue = GetByteBoundColorValue(sandColor.B + blueDeviation);
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

            State = SandPileCellState.Air;
            deviatedSandColor = lowerCell.deviatedSandColor;

            lowerCell.State = SandPileCellState.Sand;
            lowerCell.deviatedSandColor = tempThisDeviatedSandColor;
        }

        public static ColorValues[] GetColors() => // Gets only basic sand color
            new[]
            {
                sandColor,
                airColor,
                wallColor
            };
    }
}
