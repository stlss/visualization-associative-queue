using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CollectionLibrary.Observable
{
    public class ObservableStack<T> : Stack<T>, INotifyCollectionChanged
    {
        #region Реализация интерфейса INotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e) 
        { 
            CollectionChanged?.Invoke(this, e);
        }
        #endregion


        #region Конструкторы
        public ObservableStack() : base() { }

        public ObservableStack(IEnumerable<T> collection) : base(collection) { }

        public ObservableStack(int capacity) : base(capacity) { }
        #endregion


        #region Сокрытие Push, Pop, TryPop и Clear
        public new void Push(T item)
        {
            base.Push(item);
            OnCollectionChanged(new(NotifyCollectionChangedAction.Add, item));
        }

        public new T Pop() 
        {
            if (Count == 0)
                ExceptionWizard.ThrowEmptyCollection(this);

            var item = base.Pop();
            OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, item));

            return item;
        }

        public new bool TryPop(out T? result)
        {
            if (Count == 0)
            {
                result = default;
                return false;
            }

            result = Pop();
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
