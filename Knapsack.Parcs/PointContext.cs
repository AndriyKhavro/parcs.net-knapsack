using System;

namespace Knapsack.Parcs
{
    public class PointContext
    {
        public PointParameters Parameters { get; set; }

        public Func<double, double> Transform { get; set; } = new Func<double, double>(x => x);
    }
}
