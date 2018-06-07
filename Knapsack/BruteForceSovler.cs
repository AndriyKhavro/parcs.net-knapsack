using System;
using System.Collections.Generic;

namespace Knapsack
{
    public class BruteForceSovler
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

        public IEnumerable<Func<double>> ParallelizeKnapsack(Item[] items, double capacity, int index, int threadCount)
        {
            if (threadCount < 1)
            {
                throw new ArgumentException("TheadCount must be greater than zero", nameof(threadCount));
            }

            if (capacity <= 0)
            {
                yield break;
            }

            if (threadCount == 1)
            {
                yield return new Func<double>(() => new BruteForceSovler().KnapsackRecursiveBrute(items, capacity, index));
            }
            else
            {
                int threadCountTake = threadCount / 2;
                int threadCountDontTake = threadCount - threadCountTake;

                foreach (var func in ParallelizeKnapsack(items, capacity, index - 1, threadCountDontTake))
                    yield return func;

                foreach (var func in ParallelizeKnapsack(items, capacity - items[index].Weight, index - 1, threadCountTake))
                {
                    double currentValue = items[index].Value;
                    yield return new Func<double>(() => currentValue + func());
                }
            }
        }
    }
}
