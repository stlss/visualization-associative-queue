using AssociativeLibrary;

namespace ArithmeticOperationLibrary
{
    public class OperationIntLcm : IAssociativeOperation<int>
    {
        public string Name => "Lcm";

        public string Description => "Наименьшее общее кратное элементов в очереди";

        public Func<int, int, int> Func => GetLcm;


        private int GetLcm(int x1, int x2) => x1 * x2 / new OperationIntGcd().Func(x1, x2);
    }
}
