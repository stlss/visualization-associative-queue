using System.Collections.Specialized;

namespace CollectionLibrary.Observable
{
    public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged
    {
        #region Реализация интерфейса INotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
        #endregion


        #region Конструкторы
        public ObservableQueue() : base() { }

        public ObservableQueue(IEnumerable<T> collection) : base(collection) { }

        public ObservableQueue(int capacity) : base(capacity) { }
        #endregion


        #region Сокрытие Enqueue, Dequeue, TryDequeue и Clear
        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Add, item));
        }

        public new T Dequeue()
        {
            if (Count == 0)
                ExceptionWizard.ThrowEmptyCollection(this);

            var item = base.Dequeue();
            OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, item));

            return item;
        }

        public new bool TryDequeue(out T? result)
        {
            if (Count == 0)
            {
                result = default;
                return false;
            }

            result = Dequeue();
            return true;
        }

        public new void Clear()
        {
            base.Clear();
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
        }
        #endregion
    }
}
