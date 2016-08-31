using System;

namespace CellularAutomata
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

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

        public int SquaredDistanceFrom(Point other) =>
           (other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y);

        public double DistanceFrom(Point other) =>
            Math.Sqrt(SquaredDistanceFrom(other));

        public override string ToString() =>
            $"({X},{Y})";
    }
}
