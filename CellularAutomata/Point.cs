using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Translate(int x, int y)
        {
            X += x;
            Y += y;
        }

        public override string ToString() =>
            $"({X},{Y})";
    }
}
