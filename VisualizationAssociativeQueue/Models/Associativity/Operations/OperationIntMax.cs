using AssociativeLibrary;

namespace VisualizationAssociativeQueue.Models.Associativity.Operations
{
    internal class OperationIntMax : IAssociativeOperation<int>
    {
        public string Name => "Max";

        public string Description => "Максимум";

        public Func<int, int, int> Func => Math.Max;
    }
}
