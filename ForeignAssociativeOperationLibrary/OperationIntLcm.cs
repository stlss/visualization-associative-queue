using AssociativeLibrary;

namespace ForeignAssociativeOperationLibrary
{
    public class OperationIntLcm : IAssociativeOperation<int>
    {
        public string Name => "Lcm";

        public string Description => "Вычисляет наименьшее общее кратное";

        public Func<int, int, int> Func => GetLcm;


        private int GetLcm(int x1, int x2) => x1 * x2 / new OperationIntGcd().Func(x1, x2);
    }
}
