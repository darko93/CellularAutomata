﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public struct Fraction
    {
        public int Nominator { get; set; }
        public int Denominator { get; set; }

        public Fraction(int nominator, int denominator)
        {
            Nominator = nominator;
            Denominator = denominator;
        }

        public void Reinitialize(int nominator, int denominator)
        {
            Nominator = nominator;
            Denominator = denominator;
        }

        internal void Pow(int exponent)
        {
            Nominator = (int) Math.Pow(Nominator, exponent);
            Denominator = (int) Math.Pow(Denominator, exponent);
        }
    }
}
