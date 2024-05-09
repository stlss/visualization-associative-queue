using AssociativeLibrary;

namespace ArithmeticOperationLibrary
{
    public class OperationIntProduct : IAssociativeOperation<int>
    {
        public string Name => "Product";

        public string Description => "Произведение элементов в очереди";

        public Func<int, int, int> Func => (int x1, int x2) => x1 * x2;
    }
}
