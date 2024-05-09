using AssociativeLibrary;

namespace BitwiseOperationLibrary
{
    public class OperationIntXor : IAssociativeOperation<int>
    {
        public string Name => "Xor";

        public string Description => "Побитовое логическое исключающее ИЛИ элементов в очереди";

        public Func<int, int, int> Func => (int x1, int x2) => x1 ^ x2;
    }
}
