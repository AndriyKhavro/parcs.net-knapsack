using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knapsack.Parcs
{
    public class KnapsackDeserializer
    {
        public KnapsackProblem Deserialize(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            int n = int.Parse(lines[0]);

            var items = lines.Skip(1).Take(n)
                .Select(ReadArray)
                .Select(arr => new Item { Value = arr[1], Weight = arr[2] })
                .ToArray();

            double capacity = double.Parse(lines[n + 1]);

            return new KnapsackProblem { Items = items, Capacity = capacity };
        }

        private static double[] ReadArray(string line)
        {
            return line.Split(' ', '\t')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(double.Parse)
                .ToArray();
        }
    }
}
