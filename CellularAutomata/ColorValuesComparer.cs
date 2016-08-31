using System.Collections.Generic;

namespace CellularAutomata
{
    class ColorValuesComparer : Comparer<ColorValues>
    {
        public override int Compare(ColorValues color1, ColorValues color2)
        {
            if (color1.Red != color2.Red)
                return color1.Red.CompareTo(color2.Red);
            if (color1.Green != color2.Green)
                return color1.Green.CompareTo(color2.Green);
            return color1.Blue.CompareTo(color2.Blue);
        }
    }
}
