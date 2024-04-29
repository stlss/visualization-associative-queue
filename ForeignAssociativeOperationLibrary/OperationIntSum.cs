using AssociativeLibrary;

namespace ForeignAssociativeOperationLibrary
{
    public class OperationIntSum : IAssociativeOperation<int>
    {
        public string Name => "Sum";

        public string Description => "Вычисляет сумму";

        public Func<int, int, int> Func => (int x1, int x2) => x1 + x2;
    }
}
