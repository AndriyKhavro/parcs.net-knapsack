using Parcs;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Knapsack.Parcs
{
    public class KnapsackMainModule : MainModule
    {
        private readonly KnapsackModuleOptions _moduleOptions;

        public KnapsackMainModule(KnapsackModuleOptions moduleOptions)
        {
            _moduleOptions = moduleOptions;
        }

        public override void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            var problem = new KnapsackDeserializer().Deserialize(_moduleOptions.FilePath);
            var pointContexts = new KnapsackParallelizer()
                .ParallelizeKnapsack(problem.Items, problem.Capacity, problem.Items.Length - 1, _moduleOptions.PointCount);

            var channelsWithContexts = pointContexts.Select(pointContext =>
            {
                IPoint point = info.CreatePoint();
                IChannel channel = point.CreateChannel();
                point.ExecuteClass(typeof(KnapsackWorkerModule).ToString());
                return new { Channel = channel, Context = pointContext };
            }).ToArray();

            foreach (var pair in channelsWithContexts)
            {
                pair.Channel.WriteObject(pair.Context.Parameters);
            }

            Console.WriteLine($"Starting Knapsack Module on {_moduleOptions.PointCount} points");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            double result = channelsWithContexts.Select(pair => pair.Context.Transform(pair.Channel.ReadDouble())).Max();

            Console.WriteLine($"Result: {result}. Elapsed: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
