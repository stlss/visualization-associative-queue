using CollectionLibrary.Associative;
using ArithmeticOperationLibrary;

namespace TestCollectionLibrary.Associative
{
    public class TestAssociativeStack
    {
        [Fact]
        public void Test1()
        {
            var stack = new AssociativeStack<int>();
            Assert.Throws<InvalidOperationException>(() => stack.GetResultAssociativeOperation());

            stack.Operation = new OperationIntSum();
            Assert.Throws<InvalidOperationException>(() => stack.GetResultAssociativeOperation());
        }

        [Fact]
        public void Test2()
        {
            var stack = new AssociativeStack<int>() { Operation = new OperationIntSum() };
            int sum = 0;

            for (int i = 0; i < 10; i++)
            {
                stack.Push(i);
                sum += i;
            }

            Assert.Equal(sum, stack.GetResultAssociativeOperation());
        }

        [Fact]
        public void Test3()
        {
            var stack = new AssociativeStack<int>() { Operation = new OperationIntProduct() };
            int product = 1;

            for (int i = 1; i < 10; i++)
            {
                stack.Push(i);
                product *= i;
            }

            for (int i = 9; i > 5; i--)
            {
                stack.Pop();
                product /= i;
            }

            Assert.Equal(product, stack.GetResultAssociativeOperation());
        }

        [Fact]
        public void Test4()
        {
            var stack = new AssociativeStack<int>() { Operation = new OperationIntSum() };

            for (int i = 0; i < 10; i++)
                stack.Push(i);

            stack.Clear();

            Assert.Throws<InvalidOperationException>(() => stack.GetResultAssociativeOperation());
        }

        [Fact]
        public void Test5()
        {
            var collection = Enumerable.Range(0, 10);

            var stack1 = new Stack<int>(collection);
            var stack2 = new AssociativeStack<int>(collection);

            Assert.Equal(stack1, stack2);
        }

        [Fact]
        public void Test6()
        {
            var stack = new AssociativeStack<int>(Enumerable.Range(0, 10)) 
            { 
                Operation = new OperationIntSum() 
            };

            int sum = 0;
            for (int i = 0; i < 10; i++)
                sum += i;

            for (int i = 0; i < 5; i++)
            {
                stack.Pop();
                sum -= i;
            }

            Assert.Equal(sum, stack.GetResultAssociativeOperation());
        }
    }
}
