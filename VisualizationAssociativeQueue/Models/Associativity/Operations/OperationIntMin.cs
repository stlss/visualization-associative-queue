using AssociativeLibrary;

namespace VisualizationAssociativeQueue.Models.Associativity.Operations
{
    internal class OperationIntMin : IAssociativeOperation<int>
    {
        public string Name => "Min";

        public string Description => "Минимальный элемент в очереди";

        public Func<int, int, int> Func => Math.Min;
    }
}
