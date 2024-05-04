using CollectionLibrary.Observable;
using System.Collections.Specialized;

namespace TestCollectionLibrary.Observable
{
    public class TestObservableQueue
    {
        [Fact]
        public void Test1()
        {
            var queue = new ObservableQueue<int>();

            var list1 = new List<int>();
            var list2 = new List<int>();

            queue.CollectionChanged += (object? sender, NotifyCollectionChangedEventArgs e) =>
            {
                Assert.Equal(NotifyCollectionChangedAction.Add, e.Action);

                foreach (var item in e.NewItems!)
                    list2.Add((int)item);
            };

            for (int i = 0; i < 10; i++)
            {
                list1.Add(i);
                queue.Enqueue(i);
            }

            Assert.Equal(list1, list2);
        }

        [Fact]
        public void Test2()
        {
            var queue = new ObservableQueue<int>(Enumerable.Range(0, 10));

            var list1 = new List<int>();
            var list2 = new List<int>();

            queue.CollectionChanged += (object? sender, NotifyCollectionChangedEventArgs e) =>
            {
                Assert.Equal(NotifyCollectionChangedAction.Remove, e.Action);

                foreach (var item in e.OldItems!)
                    list2.Add((int)item);
            };

            for (int i = 0; i < 10; i++)
            {
                list1.Add(i);
                queue.Dequeue();
            }

            Assert.Equal(list1, list2);
        }

        [Fact]
        public void Test3()
        {
            var queue = new ObservableQueue<int>(Enumerable.Range(0, 10));

            queue.CollectionChanged += (object? sender, NotifyCollectionChangedEventArgs e) =>
            {
                Assert.Equal(NotifyCollectionChangedAction.Reset, e.Action);
                Assert.Null(e.OldItems);
                Assert.Null(e.NewItems);
            };

            queue.Clear();
        }
    }
}
