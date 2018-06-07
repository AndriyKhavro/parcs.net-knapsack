using System;

namespace Knapsack.Parcs
{
    [Serializable]
    public class PointParameters
    {
        public Item[] Items { get; set; }

        public double Capacity { get; set; }

        public int Index { get; set; }
    }
}
