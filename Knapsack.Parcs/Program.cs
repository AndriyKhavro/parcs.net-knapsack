using CommandLine;
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
                var parserResult = Parser.Default.ParseArguments<KnapsackModuleOptions>(args);

                parserResult
                    .WithParsed(result => options = result)
                    .WithNotParsed(errors =>
                        throw new ArgumentException($@"Cannot parse the arguments. Possible usages:{options.GetUsage()}"));
            }

            (new KnapsackMainModule(options)).RunModule(options);
        }
    }
}
