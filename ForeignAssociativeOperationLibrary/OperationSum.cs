using AssociativeLibrary;

namespace ForeignAssociativeOperationLibrary
{
    public class OperationSum : IAssociativeOperation<int>
    {
        public string Name => "Sum";

        public string Description => "Вычисляет сумму";

        public Func<int, int, int> Operation => (int x1, int x2) => x1 + x2;
    }
}
