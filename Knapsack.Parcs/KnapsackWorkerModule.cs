using Parcs;
using System.Threading;

namespace Knapsack.Parcs
{
    public class KnapsackWorkerModule : IModule
    {
        public void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            PointParameters parameters = info.Parent.ReadObject<PointParameters>();

            Item[] items = parameters.Items;
            double capacity = parameters.Capacity;
            int index = parameters.Index;

            double result = new BruteForceSolver().KnapsackRecursiveBrute(items, capacity, index);

            info.Parent.WriteData(result);
        }
    }
}
