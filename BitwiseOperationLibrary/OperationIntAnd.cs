using AssociativeLibrary;

namespace BitwiseOperationLibrary
{
    public class OperationIntAnd : IAssociativeOperation<int>
    {
        public string Name => "And";

        public string Description => "Побитовое логическое И";

        public Func<int, int, int> Func => (int x1, int x2) => x1 & x2;
    }
}
