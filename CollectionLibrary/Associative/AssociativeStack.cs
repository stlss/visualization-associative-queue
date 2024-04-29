using AssociativeLibrary;

namespace CollectionLibrary.Associative
{
    public class AssociativeStack<T> : Stack<T>, IAssociativeCollection<T>
    {
        /// <summary>
        /// Стек, i-ый элемент которого является результатом ассоциативной операции (0-го, 1-го, ..., i-ого) элементов основного стека. 
        /// </summary>
        private readonly Stack<T> _onlyAssociativeStack;


        #region Конструкторы
        public AssociativeStack() : base()
        { 
            _onlyAssociativeStack = new();
        }

        public AssociativeStack(IEnumerable<T> collection) : base(collection)
        {
            _onlyAssociativeStack = new(collection.Count());

            foreach (var item in collection)
                Push(item);
        }

        public AssociativeStack(int capacity) : base(capacity)
        {
            _onlyAssociativeStack = new(capacity);
        }
        #endregion


        #region Реализация интерфейса IAssociativeCollection<T>
        private IAssociativeOperation<T>? _operation;

        public IAssociativeOperation<T>? Operation
        {
            get => _operation;
            set
            {
                _operation = value;

                var items = ToArray();
                Clear();

                foreach (var item in items)
                    Push(item);
            }
        }

        public T GetResultAssociativeOperation()
        {
            if (Count == 0)
                ExceptionWizard.ThrowEmptyAssociativeCollection(this);

            if (Operation == null)
                ExceptionWizard.ThrowNullOperationAssociativeCollection(this);

            return _onlyAssociativeStack.Peek();
        }

        public bool TryGetResultAssociativeOperation(out T? result)
        {
            if (Count == 0 || Operation == null)
            {
                result = default;
                return false;
            }

            result = _onlyAssociativeStack.Peek();
            return true;
        }
        #endregion


        #region Сокрытие Push, Pop, TryPop и Clear
        public new void Push(T item)
        {
            base.Push(item);

            if (Operation == null)
                return;

            if (_onlyAssociativeStack.Count == 0)
                _onlyAssociativeStack.Push(item);
            else
                _onlyAssociativeStack.Push(Operation.Operation(item, _onlyAssociativeStack.Peek()));
        }

        public new T Pop()
        {
            if (Count == 0)
                ExceptionWizard.ThrowEmptyAssociativeCollection(this);

            var item = base.Pop();
            _onlyAssociativeStack.Pop();

            return item;
        }

        public new bool TryPop(out T? item)
        {
            if (Count == 0)
            {
                item = default;
                return false;
            }

            item = Pop();
            return true;
        }

        public new void Clear()
        {
            base.Clear();
            _onlyAssociativeStack.Clear();
        }
        #endregion
    }
}
