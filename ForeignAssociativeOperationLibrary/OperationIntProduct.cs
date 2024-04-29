using AssociativeLibrary;

namespace ForeignAssociativeOperationLibrary
{
    public class OperationIntProduct : IAssociativeOperation<int>
    {
        public string Name => "Product";

        public string Description => "Вычисляет произведение";

        public Func<int, int, int> Func => (int x1, int x2) => x1 * x2;
    }
}
