using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class ColorValues
    {
        public byte Red { get; private set; }
        public byte Green { get; private set; }
        public byte Blue { get; private set; }
        public ColorValues(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public int ToInt()
        {
            int colorValue = 0;
            colorValue |= 255 << 24; // Alpha
            colorValue |= Red << 16;
            colorValue |= Green << 8;
            colorValue |= Blue;
            return colorValue;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash *= 23 + Red.GetHashCode();
                hash *= 23 + Green.GetHashCode();
                hash *= 23 + Blue.GetHashCode();
                return hash;
            }
        }

        private bool ValuesEqual(ColorValues other) =>
            Red == other.Red && Green == other.Green && Blue == other.Blue;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ColorValues other = (ColorValues)obj;
            return ValuesEqual(other);
        }

        public bool Equals(ColorValues other) =>
            ValuesEqual(other);

        public override string ToString() =>
            $"R={Red}, G={Green}, B={Blue}";

        public static ColorValues EmptyCell => new ColorValues(128, 128, 128);
        public static ColorValues Tree => new ColorValues(0, 255, 0);
        public static ColorValues BurningTree => new ColorValues(255, 0, 0);
        public static ColorValues Air => new ColorValues(255, 255, 255);
        public static ColorValues Sand => new ColorValues(244, 164, 96); // sandy brown
        //public static ColorValues Sand => new ColorValues(225, 169, 95); // earth yellow
        //public static ColorValues Sand => new ColorValues(194, 178, 128); // sand
        public static ColorValues Wall => new ColorValues(0, 0, 0);
        public static ColorValues Particle => new ColorValues(31, 117, 254); // blue (crayola)
    }
}
