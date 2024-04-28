using AssociativeLibrary;

namespace VisualizationAssociativeQueue.Models.Associativity.Operations
{
    internal class OperationMin : IAssociativeOperation<int>
    {
        public string Name => "Min";

        public string Description => "Вычисляет минимум";

        public Func<int, int, int> Operation => Math.Min;
    }
}
