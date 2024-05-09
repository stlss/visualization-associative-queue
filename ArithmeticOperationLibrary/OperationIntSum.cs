using AssociativeLibrary;

namespace ArithmeticOperationLibrary
{
    public class OperationIntSum : IAssociativeOperation<int>
    {
        public string Name => "Sum";

        public string Description => "Сумма элементов в очереди";

        public Func<int, int, int> Func => (int x1, int x2) => x1 + x2;
    }
}
