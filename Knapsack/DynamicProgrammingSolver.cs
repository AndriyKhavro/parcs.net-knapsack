using System;
using System.Collections.Generic;

namespace Knapsack
{
    public class DynamicProgrammingSolver
    {
        public double Solve(Item[] items, double capacity)
        {
            if (items.Length == 0)
            {
                return 0;
            }

            var cache = CreateCache(items.Length);

            return KnapsackRecursiveDP(items, capacity, items.Length - 1, cache);
        }

        private static double KnapsackRecursiveDP(Item[] items, double capacity, int index, IDictionary<int, Dictionary<double, double>> cache)
        {
            if (index == 0)
            {
                if (items[0].Weight <= capacity && items[0].Value > 0)
                {
                    return items[0].Value;
                }

                return 0;
            }

            if (cache[index].TryGetValue(capacity, out double cachedResult))
            {
                return cachedResult;
            }

            double dontTake = KnapsackRecursiveDP(items, capacity, index - 1, cache);

            double result = items[index].Weight <= capacity && items[index].Value > 0
                ? Math.Max(items[index].Value + KnapsackRecursiveDP(items, capacity - items[index].Weight, index - 1, cache),
                  dontTake)
                : dontTake;

            return cache[index][capacity] = result;
        }

        private static IDictionary<int, Dictionary<double, double>> CreateCache(int length)
        {
            var cache = new Dictionary<int, Dictionary<double, double>>();
            for (int i = 0; i < length; i++)
            {
                cache[i] = new Dictionary<double, double>();
            }

            return cache;
        }
    }
}
