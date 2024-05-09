using AssociativeLibrary;

namespace VisualizationAssociativeQueue.Models.Associativity.Operations
{
    internal class OperationIntMin : IAssociativeOperation<int>
    {
        public string Name => "Min";

        public string Description => "Минимум";

        public Func<int, int, int> Func => Math.Min;
    }
}
