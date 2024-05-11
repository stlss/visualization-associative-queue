using AssociativeLibrary;
using System.Reflection;

namespace CollectionLibrary.Associative
{
    /// <summary>
    /// Обобщённая очередь, поддерживающая нахождение значения ассоцитивной операции за O(1).
    /// </summary>
    public class AssociativeQueue<T> : Queue<T>, IAssociativeCollection<T>
    {
        #region Поля
        /// <summary>
        /// Пуш-стек, добавляемый в очередь элемент добавляется в него.
        /// </summary>
        private readonly AssociativeStack<T> _pushStack;

        /// <summary>
        /// Поп-стек, удаляемый из очереди элемент удаляется из него.
        /// Если в нём нет элементов, то все элементы пуш-стека перегоняются в него.
        /// </summary>
        private readonly AssociativeStack<T> _popStack;

        /// <summary>
        /// Поле для ExceptionWizard.
        /// </summary>
        private readonly PropertyInfo _propertyOperation = typeof(AssociativeQueue<T>).GetProperty("Operation")!;
        #endregion


        #region Конструкторы
        public AssociativeQueue() : base()
        {
            _pushStack = new();
            _popStack = new();
        }

        public AssociativeQueue(IEnumerable<T> collection) : base(collection.Count())
        {
            _pushStack = new(collection.Count());
            _popStack = new();

            foreach (var item in collection)
                Enqueue(item);
        }

        public AssociativeQueue(int capacity) : base(capacity)
        {
            _pushStack = new(capacity / 2);
            _popStack = new(capacity / 2);
        }
        #endregion


        #region Релизация интерфейса IAssociativeCollection<T>
        private IAssociativeOperation<T>? _operation;

        public IAssociativeOperation<T>? Operation
        {
            get => _operation;
            set
            {
                _operation = value;
                _pushStack.Operation = value;
                _popStack.Operation = value;

                var items = ToArray();
                Clear();

                foreach (var item in items)
                    Enqueue(item);
            }
        }

        public T GetResultAssociativeOperation()
        {
            if (Count == 0)
                ExceptionWizard.ThrowEmptyCollection(this);

            if (Operation == null)
                ExceptionWizard.ThrowNullPropertyCollection(this, _propertyOperation);


            bool isNotEmptyPushStack = _pushStack.TryGetResultAssociativeOperation(out T? valueOperationPushStack);
            bool isNotEmptyPopStack = _popStack.TryGetResultAssociativeOperation(out T? valueOperationPopStack);

            if (!isNotEmptyPushStack)
                return valueOperationPopStack!;

            if (!isNotEmptyPopStack)
                return valueOperationPushStack!;

            return Operation!.Func(valueOperationPushStack!, valueOperationPopStack!);
        }

        public bool TryGetResultAssociativeOperation(out T? result)
        {
            if (Count == 0 || Operation == null)
            {
                result = default;
                return false;
            }

            result = GetResultAssociativeOperation();
            return true;
        }
        #endregion


        #region Сокрытие Enqueue, Dequeue, TryDequeue и Clear
        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            _pushStack.Push(item);
        }

        public new T Dequeue()
        {
            if (Count == 0)
                ExceptionWizard.ThrowEmptyCollection(this);

            if (_popStack.Count == 0)
            {
                while (_pushStack.Count > 0)
                    _popStack.Push(_pushStack.Pop());
            }

            var item = base.Dequeue();
            _popStack.Pop();

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

            _pushStack.Clear();
            _popStack.Clear();
        }
        #endregion
    }
}
