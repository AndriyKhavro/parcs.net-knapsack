using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Knapsack
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            var items = Enumerable.Range(0, n)
                .Select(_ => ReadArray())
                .Select(arr => new Item { Value = arr[1], Weight = arr[2] })
                .ToArray();

            double capacity = double.Parse(Console.ReadLine());

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            double seqResult = new DynamicProgrammingSolver().Solve(items, capacity);
            Console.WriteLine($"Result DP: {seqResult}. Elapsed: {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();

            double bruteSeqResult = new BruteForceSovler().KnapsackRecursiveBrute(items, capacity, items.Length - 1);
            Console.WriteLine($"Result Sequential Brute Force: {bruteSeqResult}. Elapsed: {stopwatch.ElapsedMilliseconds}");
            foreach (int threadCount in new[] { 1, 2, 4, 8 })
            {
                stopwatch.Restart();

                double bruteForceResult = GetBruteForceResult(items, capacity, threadCount);

                Console.WriteLine($"Result Brute Force: {bruteForceResult}. Thread Count: {threadCount}. Elapsed: {stopwatch.ElapsedMilliseconds}");
            }
        }

        private static double GetBruteForceResult(Item[] items, double capacity, int threadCount)
        {
            var tasks = new BruteForceSovler().ParallelizeKnapsack(items, capacity, items.Length - 1, threadCount).Select((func, i) => Task.Run(func));

            var allTask = Task.WhenAll(tasks.Select((t, i) => t.ContinueWith((result) => {
                Console.WriteLine($"Task {i + 1} finished");
                return result.Result;
                })));

            return allTask.Result.Max();
        }

        private static double[] ReadArray()
        {
            return Console.ReadLine().Split(' ', '\t')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(double.Parse)
                .ToArray();
        }

        private static double KnapsackDP(Item[] items, double capacity)
        {
            if (items.Length == 0)
            {
                return 0;
            }

            var cache = CreateCache(items.Length);

            return KnapsackRecursiveDP(items, capacity, items.Length - 1, cache);
        }

        private static double KnapsackBrute(Item[] items, double capacity)
        {
            if (items.Length == 0)
            {
                return 0;
            }

            return KnapsackRecursiveBrute(items, capacity, items.Length - 1);
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

        private static double KnapsackRecursiveBrute(Item[] items, double capacity, int index)
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
    }
}
