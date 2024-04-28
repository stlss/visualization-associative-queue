using AssociativeLibrary;

namespace ForeignAssociativeOperationLibrary
{
    public class OperationGcd : IAssociativeOperation<int>
    {
        public string Name => "Gcd";

        public string Description => "Вычисляет наибольший общий делитель";

        public Func<int, int, int> Operation => GetGcd;


        private int GetGcd(int x1, int x2) => x2 == 0 ? x1 : GetGcd(x2, x1 % x2);
    }
}
