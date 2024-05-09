using AssociativeLibrary;

namespace ArithmeticOperationLibrary
{
    public class OperationIntGcd : IAssociativeOperation<int>
    {
        public string Name => "Gcd";

        public string Description => "Наибольший общий делитель элементов в очереди";

        public Func<int, int, int> Func => GetGcd;


        private int GetGcd(int x1, int x2) => x2 == 0 ? x1 : GetGcd(x2, x1 % x2);
    }
}
