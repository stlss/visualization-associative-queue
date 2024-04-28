using AssociativeLibrary;

namespace VisualizationAssociativeQueue.Models.Associativity.Operations
{
    internal class OperationMax : IAssociativeOperation<int>
    {
        public string Name => "Max";

        public string Description => "Максимум среди всех элементов";

        public Func<int, int, int> Operation => Math.Max;
    }
}
