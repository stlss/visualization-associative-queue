using CollectionLibrary.Observable;
using System.Collections.Specialized;

namespace TestCollectionLibrary.Observable
{
    public class TestObservableStack
    {
        [Fact]
        public void Test1()
        {
            var stack = new ObservableStack<int>();

            var list1 = new List<int>();
            var list2 = new List<int>();

            stack.CollectionChanged += (object? sender, NotifyCollectionChangedEventArgs e) =>
            {
                Assert.Equal(NotifyCollectionChangedAction.Add, e.Action);

                foreach (var item in e.NewItems!)
                    list2.Add((int)item);
            };

            for (int i = 0; i < 10; i++)
            {
                list1.Add(i);
                stack.Push(i);
            }

            Assert.Equal(list1, list2);
        }

        [Fact]
        public void Test2()
        {
            var stack = new ObservableStack<int>(Enumerable.Range(0, 10));

            var list1 = new List<int>();
            var list2 = new List<int>();

            stack.CollectionChanged += (object? sender, NotifyCollectionChangedEventArgs e) =>
            {
                Assert.Equal(NotifyCollectionChangedAction.Remove, e.Action);

                foreach (var item in e.OldItems!)
                    list2.Add((int)item);
            };

            for (int i = 9; i >= 0; i--)
            {
                list1.Add(i);
                stack.Pop();
            }

            Assert.Equal(list1, list2);
        }

        [Fact]
        public void Test3()
        {
            var stack = new ObservableStack<int>(Enumerable.Range(0, 10));

            stack.CollectionChanged += (object? sender, NotifyCollectionChangedEventArgs e) =>
            {
                Assert.Equal(NotifyCollectionChangedAction.Reset, e.Action);
                Assert.Null(e.OldItems);
                Assert.Null(e.NewItems);
            };

            stack.Clear();
        }
    }
}
