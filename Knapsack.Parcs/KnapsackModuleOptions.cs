using CommandLine;
using Parcs.Module.CommandLine;

namespace Knapsack.Parcs
{
    public class KnapsackModuleOptions : BaseModuleOptions
    {
        [Option("file", Required = true, HelpText = "File name of knapsack values and weights")]
        public string FilePath { get; set; }

        [Option('p', Required = true, HelpText = "Number of points.")]
        public int PointCount { get; set; }
    }
}
