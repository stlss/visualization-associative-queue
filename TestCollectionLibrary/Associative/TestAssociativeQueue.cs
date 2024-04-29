using CollectionLibrary.Associative;
using ForeignAssociativeOperationLibrary;

namespace TestCollectionLibrary.Associative
{
    public class TestAssociativeQueue
    {
        /// <summary>
        /// Тестирование Enqueue.
        /// </summary>
        [Fact]
        public void TestEnqueue()
        {
            var queue = new AssociativeQueue<int>();
            int count = 5;

            var numbers1 = Enumerable.Range(0, count);
            List<int> numbers2 = [], numbers3 = [];

            for (int i = 0; i < count; i++)
            {
                numbers2.Add(queue.Count);

                queue.Enqueue(i);

                numbers3.Add(queue.Peek());
            }

            Assert.Equal(numbers1, numbers2);
            Assert.Equal(Enumerable.Range(0, count).Select(_ => 0), numbers3);
        }


        /// <summary>
        /// Тестирование Dequeue.
        /// </summary>
        [Fact]
        public void TestDequeue()
        {
            var queue = new AssociativeQueue<int>();
            int count = 5;

            var numbers1 = Enumerable.Range(0, count);
            var numbers2 = Enumerable.Range(0, count).Reverse();

            List<int> numbers3 = [], numbers4 = [];

            for (int i = 0; i < count; i++)
                queue.Enqueue(i);

            for (int i = count; i > 0; i--)
            {
                numbers3.Add(queue.Dequeue());
                numbers4.Add(queue.Count);
            }

            Assert.Equal(numbers1, numbers3);
            Assert.Equal(numbers2, numbers4);
            Assert.Throws(new InvalidOperationException().GetType(), () => queue.Dequeue());
        }

        /// <summary>
        /// Тестирование конструктора с параметром типа IEnumerable<T>.
        /// </summary>
        [Fact]
        public void TestConstructor()
        {
            var queue1 = new AssociativeQueue<int>();

            for (int i = 0; i < 5; i++)
                queue1.Enqueue(i);

            var queue2 = new AssociativeQueue<int>(queue1);

            Assert.Equal(queue1, queue2);
        }

        /// <summary>
        /// Тестирование GetResultAssociativeOperation.
        /// </summary>
        [Fact]
        public void TestGetResultAssociativeOperation()
        {
            var queue = new AssociativeQueue<int>() { Operation = new OperationSum() };
            int sum = 0, product = 1;

            for (int i = 1; i <= 10; i++)
            {
                queue.Enqueue(i);

                sum += i;
                product *= i;
            }

            for (int i = 1; i <= 5; i++)
            {
                queue.Dequeue();

                sum -= i;
                product /= i;
            }

            Assert.Equal(sum, queue.GetResultAssociativeOperation());

            queue.Operation = null;
            Assert.Throws(new InvalidOperationException().GetType(), () => queue.GetResultAssociativeOperation());

            queue.Operation = new OperationProduct();
            Assert.Equal(product, queue.GetResultAssociativeOperation());
        }
    }
}
