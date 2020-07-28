using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.DataAnalysis
{
    public class MomentsCalculator
    {
        private double mean;
        private double sd;
        private double median;
        private double mode;

        private double WelfordsM;
        private double WelfordsS;

        private int length;

        private List<double> values;

        public MomentsCalculator()
        {
            mean = 0.0;
            sd = 0.0;
            median = 0.0;
            mode = 0.0;

            WelfordsM = 0.0;
            WelfordsS = 0.0;

            values = new List<double>();
            length = 0;
        }

        public double[] GetValues()
        {
            median = values.Median();
            mode = Mode(values.ToArray());
            return new[] { mean, sd, median, mode };
        }

        public void Add(double value)
        {
            values.Add(value);

            mean = UpdateMean(value);
            sd = UpdateSD(value);

            length++;
        }

        private double UpdateMean(double value)
        {
            return (mean * length + value) / (length + 1);
        }

        private double UpdateSD(double value)
        {
            double tmpM = WelfordsM;
            WelfordsM += (value - tmpM) / (length + 1);
            WelfordsS += (value - tmpM) * (value - WelfordsM);
            return Math.Sqrt(WelfordsS / (length - 1));
        }

        public static double Mode(double[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException("Array cannot be empty");

            Dictionary<double, int> valuesMap = new Dictionary<double, int>();
            for (int i = 0; i < values.Length; i++)
            {
                if (!valuesMap.ContainsKey(values[i]))
                    valuesMap.Add(values[i], 0);
                valuesMap[values[i]]++;
            }
            double[] distinctValues = valuesMap.Keys.ToArray();
            int[] counts = valuesMap.Values.ToArray();

            int maxCount = 0;
            double mode = double.NaN;
            for (int i = 0; i < counts.Length; i++)
                if (counts[i] > maxCount)
                {
                    maxCount = counts[i];
                    mode = distinctValues[i];
                }

            return mode;
        }
    }
}
