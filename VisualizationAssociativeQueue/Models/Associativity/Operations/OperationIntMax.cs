using AssociativeLibrary;

namespace VisualizationAssociativeQueue.Models.Associativity.Operations
{
    internal class OperationIntMax : IAssociativeOperation<int>
    {
        public string Name => "Max";

        public string Description => "Максимальный элемент в очереди";

        public Func<int, int, int> Func => Math.Max;
    }
}
