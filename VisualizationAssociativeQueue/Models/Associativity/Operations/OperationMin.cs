using AssociativeLibrary;

namespace VisualizationAssociativeQueue.Models.Associativity.Operations
{
    internal class OperationMin : IAssociativeOperation<int>
    {
        public string Name => "Min";

        public string Description => "Минимум среди всех элементов";

        public Func<int, int, int> Operation => Math.Min;
    }
}
