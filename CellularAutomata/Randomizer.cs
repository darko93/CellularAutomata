using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    class Randomizer
    {
        private Randomizer() { }
        public static Randomizer Instance { get; } = new Randomizer();
        
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public int Next(int minValue, int maxValue)
        {
            lock(syncLock)
            {
                return random.Next(minValue, maxValue);
            }
        }

        public int Next(int maxValue)
        {
            lock(syncLock)
            {
                return random.Next(maxValue);
            }
        }

        public int[] GetDistinctRandomNumbers(int numbersAmount, int minValue, int maxValue)
        {
            List<int> possibleNumbers = new List<int>(numbersAmount);
            int[] resultNumbers = new int[numbersAmount];

            for (int i = minValue; i <= maxValue; i++)
                possibleNumbers.Add(i);

            for (int i = 0; i < numbersAmount; i++)
            {
                int randomIndex = random.Next(possibleNumbers.Count);
                resultNumbers[i] = possibleNumbers[randomIndex];
                possibleNumbers.RemoveAt(randomIndex);
            }

            return resultNumbers;
        }

        public Point[] GetDistinctRandomPoints(int pointsPercentage, Point leftBottomBound, Point rightTopBound)
        {
            int width = rightTopBound.X - leftBottomBound.X + 1;
            int height = rightTopBound.Y - leftBottomBound.Y + 1;
            int allPossiblePointsAmount = width * height;
            int desiredPointsAmount = (int)(pointsPercentage * 0.01 * allPossiblePointsAmount);

            int[] distinctRandomNumbers = GetDistinctRandomNumbers(desiredPointsAmount, 0, allPossiblePointsAmount - 1);

            Point[] distinctRandomPoints = new Point[desiredPointsAmount];

            for (int i = 0; i < desiredPointsAmount; i++)
            {
                int iRandomNumber = distinctRandomNumbers[i];
                int randomX = iRandomNumber % width;
                int randomY = iRandomNumber / width;
                Point randomPoint = new Point(randomX, randomY);
                randomPoint.Translate(leftBottomBound.X, leftBottomBound.Y);
                distinctRandomPoints[i] = randomPoint;
            }

            return distinctRandomPoints;
        }

        private Fraction GetComplementaryEventProbability(Fraction probability)
        {
            int nominator = probability.Denominator - probability.Nominator;
            return new Fraction(nominator, probability.Denominator);
        }
        
        public bool BernoulliTrialSuccess(Fraction successProbability) =>
            random.Next(successProbability.Denominator) < successProbability.Nominator;
        
        public bool AtLeastOneSuccessInBernoulliProcess(Fraction singleTrialSuccessProbability, int trialsAmount) 
        {
            Fraction failureProbability = GetComplementaryEventProbability(singleTrialSuccessProbability);
            failureProbability.Pow(trialsAmount);
            return !BernoulliTrialSuccess(failureProbability);
        }
    }
}
