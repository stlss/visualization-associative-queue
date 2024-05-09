using CollectionLibrary.Associative;
using ArithmeticOperationLibrary;

namespace TestCollectionLibrary.Associative
{
    public class TestAssociativeQueue
    {
        [Fact]
        public void Test1()
        {
            var queue = new AssociativeQueue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.GetResultAssociativeOperation());

            queue.Operation = new OperationIntSum();
            Assert.Throws<InvalidOperationException>(() => queue.GetResultAssociativeOperation());
        }

        [Fact]
        public void Test2()
        {
            var queue = new AssociativeQueue<int>() { Operation = new OperationIntSum() };
            int sum = 0;

            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
                sum += i;
            }

            Assert.Equal(sum, queue.GetResultAssociativeOperation());
        }

        [Fact]
        public void Test3()
        {
            var queue = new AssociativeQueue<int>() { Operation = new OperationIntProduct() };
            int product = 1;

            for (int i = 1; i < 10; i++)
            {
                queue.Enqueue(i);
                product *= i;
            }

            for (int i = 1; i < 5; i++)
            {
                queue.Dequeue();
                product /= i;
            }

            Assert.Equal(product, queue.GetResultAssociativeOperation());
        }

        [Fact]
        public void Test4()
        {
            var queue = new AssociativeQueue<int>() { Operation = new OperationIntSum() };

            for (int i = 0; i < 10; i++)
                queue.Enqueue(i);

            queue.Clear();

            Assert.Throws<InvalidOperationException>(() => queue.GetResultAssociativeOperation());
        }

        [Fact]
        public void Test5()
        {
            var collection = Enumerable.Range(0, 10);

            var queue1 = new Queue<int>(collection);
            var queue2 = new AssociativeQueue<int>(collection);

            Assert.Equal(queue1, queue2);
        }

        [Fact]
        public void Test6()
        {
            var queue = new AssociativeQueue<int>(Enumerable.Range(0, 10))
            {
                Operation = new OperationIntSum()
            };

            int sum = 0;
            for (int i = 0; i < 10; i++)
                sum += i;

            for (int i = 0; i < 5; i++)
            {
                queue.Dequeue();
                sum -= i;
            }

            Assert.Equal(sum, queue.GetResultAssociativeOperation());
        }
    }
}
