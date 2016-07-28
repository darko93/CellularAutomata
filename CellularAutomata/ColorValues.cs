using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class ColorValues
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public ColorValues(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
        public ColorValues() { }

        public int ToInt()
        {
            int colorValue = 0;
            colorValue |= 255 << 24;
            colorValue |= R << 16;
            colorValue |= G << 8;
            colorValue |= B;
            return colorValue;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash *= 23 + R.GetHashCode();
                hash *= 23 + G.GetHashCode();
                hash *= 23 + B.GetHashCode();
                return hash;
            }
        }

        private bool ValuesEqual(ColorValues other) =>
            R == other.R && G == other.G && B == other.B;

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
            $"R={R}, G={G}, B={B}";

        public static ColorValues EmptyCell => new ColorValues(128, 128, 128);
        public static ColorValues Tree => new ColorValues(0, 255, 0);
        public static ColorValues BurningTree => new ColorValues(255, 0, 0);
        public static ColorValues Air => new ColorValues(255, 255, 255);
        public static ColorValues Sand => new ColorValues(244, 164, 96);
        public static ColorValues Wall => new ColorValues(0, 0, 0);
        public static ColorValues Particle => new ColorValues(0, 0, 255);
    }
}
