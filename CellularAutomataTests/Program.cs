using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CellularAutomata;
using System.Diagnostics;

namespace CellularAutomataTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(IsDistinctRandomNumbersRealyDistinct());
            Console.ReadKey();
        }

        private static void ForestFirePerformance()
        {
            int steps = 10000;
            ForestFire forestFire = new ForestFire(150, 100);
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < steps; i++)
                forestFire.NextStep();
            sw.Stop();
            Console.WriteLine($"Steps: {steps}\nTime: {sw.ElapsedMilliseconds}");
        }

        private static bool IsDistinctRandomNumbersRealyDistinct()
        {
            int[] distinctRandomNumbers = Randomizer.Instance.GetDistinctRandomNumbers(30000, 1, 30000);
            int beforeCount = distinctRandomNumbers.Length;
            distinctRandomNumbers = distinctRandomNumbers.Distinct().ToArray();
            int afterCount = distinctRandomNumbers.Length;
            return beforeCount == afterCount;
        }
    }
}
