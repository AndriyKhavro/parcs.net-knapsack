using System;

namespace Knapsack.Parcs
{
    public class Program
    {
        static void Main(string[] args)
        {
            var options = new KnapsackModuleOptions();

            if (args != null)
            {
                if (!CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    throw new ArgumentException($@"Cannot parse the arguments. Possible usages:
{options.GetUsage()}");
                }
            }

            (new KnapsackMainModule(options)).RunModule(options);
        }
    }
}
