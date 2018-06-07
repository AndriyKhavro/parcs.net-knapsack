using System;

namespace Knapsack.Parcs
{
    public class BruteForceSolver
    {
        public double KnapsackRecursiveBrute(Item[] items, double capacity, int index)
        {
            if (index == 0)
            {
                if (items[0].Weight <= capacity && items[0].Value > 0)
                {
                    return items[0].Value;
                }

                return 0;
            }

            double dontTake = KnapsackRecursiveBrute(items, capacity, index - 1);

            double result = items[index].Weight <= capacity && items[index].Value > 0
                ? Math.Max(items[index].Value + KnapsackRecursiveBrute(items, capacity - items[index].Weight, index - 1),
                  dontTake)
                : dontTake;

            return result;
        }
    }
}
