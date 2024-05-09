using AssociativeLibrary;

namespace BitwiseOperationLibrary
{
    public class OperationIntOr : IAssociativeOperation<int>
    {
        public string Name => "Or";

        public string Description => "Побитовое логическое ИЛИ";

        public Func<int, int, int> Func => (int x1, int x2) => x1 | x2;
    }
}
