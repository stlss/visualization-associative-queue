using AssociativeLibrary;

namespace ForeignAssociativeOperationLibrary
{
    public class OperationLcm : IAssociativeOperation<int>
    {
        public string Name => "Lcm";

        public string Description => "Вычисляет наименьшее общее кратное";

        public Func<int, int, int> Operation => GetLcm;


        private int GetLcm(int x1, int x2) => x1 * x2 / new OperationGcd().Operation(x1, x2);
    }
}
