using AssociativeLibrary;

namespace ForeignAssociativeOperationLibrary
{
    public class OperationProduct : IAssociativeOperation<int>
    {
        public string Name => "Product";

        public string Description => "Вычисляет произведение";

        public Func<int, int, int> Operation => (int x1, int x2) => x1 * x2;
    }
}
