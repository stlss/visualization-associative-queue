using CollectionLibrary.Associative;
using ForeignAssociativeOperationLibrary;

namespace TestCollectionLibrary.Associative
{
    public class TestAssociativeStack
    {
        /// <summary>
        /// Тестирование Push.
        /// </summary>
        [Fact]
        public void TestPush()
        {
            var stack = new AssociativeStack<int>();
            int count = 5;

            var numbers1 = Enumerable.Range(0, count);
            List<int> numbers2 = [], numbers3 = [];

            for (int i = 0; i < count; i++)
            {
                numbers2.Add(stack.Count);

                stack.Push(i);

                numbers3.Add(stack.Peek());
            }

            Assert.Equal(numbers1, numbers2);
            Assert.Equal(numbers1, numbers3);
        }


        /// <summary>
        /// Тестирование Pop.
        /// </summary>
        [Fact]
        public void TestPop()
        {
            var stack = new AssociativeStack<int>();
            int count = 5;

            var numbers1 = Enumerable.Range(0, count).Reverse();
            List<int> numbers2 = [], numbers3 = [];

            for (int i = 0; i < count; i++)
                stack.Push(i);

            for (int i = count; i > 0; i--)
            {
                numbers2.Add(stack.Pop());
                numbers3.Add(stack.Count);
            }

            Assert.Equal(numbers1, numbers2);
            Assert.Equal(numbers1, numbers3);
            Assert.Throws(new InvalidOperationException().GetType(), () => stack.Pop());
        }

        /// <summary>
        /// Тестирование конструктора с параметром типа IEnumerable<T>.
        /// </summary>
        [Fact]
        public void TestConstructor()
        {
            var stack1 = new AssociativeStack<int>();

            for (int i = 0; i < 5; i++)
                stack1.Push(i);

            var stack2 = new AssociativeStack<int>(stack1);

            Assert.Equal(stack1, stack2);
        }

        /// <summary>
        /// Тестирование GetResultAssociativeOperation.
        /// </summary>
        [Fact]
        public void TestGetResultAssociativeOperation() 
        {
            var stack = new AssociativeStack<int>() { Operation = new OperationSum() };
            int sum = 0, product = 1;

            for (int i = 0; i < 10; i++)
            {
                stack.Push(i);

                sum += i;
                product *= i;
            }

            for (int i = 9; i >= 5; i--)
            {
                stack.Pop();

                sum -= i;
                product /= i;
            }

            Assert.Equal(sum, stack.GetResultAssociativeOperation());

            stack.Operation = null;
            Assert.Throws(new InvalidOperationException().GetType(), () => stack.GetResultAssociativeOperation());

            stack.Operation = new OperationProduct();
            Assert.Equal(product, stack.GetResultAssociativeOperation());
        }
    }
}
