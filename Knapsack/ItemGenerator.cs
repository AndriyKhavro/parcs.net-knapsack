using System;
using System.Linq;

namespace Knapsack
{
    public class ItemGenerator
    {
        public Item[] GenerateRandom(int n, double capacity)
        {
            Random random = new Random();
            var values = Enumerable.Range(0, n).Select(_ => new Item
            {
                Weight = random.Next(1, 100),
                Value = random.NextDouble() * capacity / n * 2
            }).ToArray();

            return values;
        }
    }
}
