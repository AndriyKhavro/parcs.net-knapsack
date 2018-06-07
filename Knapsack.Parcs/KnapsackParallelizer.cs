using System;
using System.Collections.Generic;

namespace Knapsack.Parcs
{
    public class KnapsackParallelizer
    {
        public IEnumerable<PointContext> ParallelizeKnapsack(Item[] items, double capacity, int index, int threadCount)
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
                yield return new PointContext
                {
                    Parameters = new PointParameters { Items = items, Capacity = capacity, Index = index }
                };
            }
            else
            {
                int threadCountTake = threadCount / 2;
                int threadCountDontTake = threadCount - threadCountTake;

                foreach (var pointContext in ParallelizeKnapsack(items, capacity, index - 1, threadCountDontTake))
                    yield return pointContext;

                foreach (var pointContext in ParallelizeKnapsack(items, capacity - items[index].Weight, index - 1, threadCountTake))
                {
                    double currentValue = items[index].Value;
                    yield return new PointContext
                    {
                        Parameters = pointContext.Parameters,
                        Transform = new Func<double, double>(x => currentValue + pointContext.Transform(x))
                    };
                }
            }
        }
    }
}
