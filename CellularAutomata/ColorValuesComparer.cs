using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    class ColorValuesComparer : Comparer<ColorValues>
    {
        public override int Compare(ColorValues color1, ColorValues color2)
        {
            if (color1.R != color2.R)
                return color1.R.CompareTo(color2.R);
            if (color1.G != color2.G)
                return color1.G.CompareTo(color2.G);
            return color1.B.CompareTo(color2.B);
        }
    }
}
