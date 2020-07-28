using System;

namespace Utils.DataAnalysis
{
    public class ValuesGenerator
    {
        private Random r;
        private double minValue;
        private double delta;

        public ValuesGenerator(double minValue, double maxValue)
        {
            r = new Random();
            this.minValue = minValue;
            delta = maxValue - minValue;
        }

        public ValuesGenerator(double minValue, double maxValue, int seed)
        {
            r = new Random(seed);
            this.minValue = minValue;
            delta = maxValue - minValue;
        }

        public double Next()
        {
            return minValue + r.NextDouble() * delta;
        }
    }
}
