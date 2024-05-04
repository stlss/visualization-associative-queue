using AssociativeLibrary;
using CollectionLibrary.Observable;
using VisualizationAssociativeQueue.ViewModels;

namespace VisualizationAssociativeQueue.Models
{
    internal class ObservableCollectionsManager
    {
        #region Поля
        private readonly ObservableStack<ElementViewModel<int>>[] _stacks;
        private ElementViewModel<int>? _lastItem;
        private IAssociativeOperation<int> _operation;
        #endregion


        #region Свойства
        public ObservableStack<ElementViewModel<int>> PushStack { get; private set; } = [];
        public ObservableStack<ElementViewModel<int>> PopStack { get; private set; } = [];

        public ObservableStack<ElementViewModel<int>> PushAssociativeStack { get; private set; } = [];
        public ObservableStack<ElementViewModel<int>> PopAssociativeStack { get; private set; } = [];

        public ObservableQueue<ElementViewModel<int>> Queue { get; private set; } = [];

        public IAssociativeOperation<int> Operation
        {
            get => _operation;
            set
            {
                _operation = value;

                UpdateAssociativeStack(PushStack, PushAssociativeStack);
                UpdateAssociativeStack(PopStack, PopAssociativeStack);
            }
        }
        #endregion


        public ObservableCollectionsManager(IAssociativeOperation<int> operation)
        {
            _stacks = [PushStack, PopStack, PushAssociativeStack, PopAssociativeStack];
            _operation = operation;
        }


        #region Методы
        public void Enqueue(int number)
        {
            UpdateCollections();

            var item = new ElementViewModel<int>(number) { Status = ElementStatus.New };

            PushStack.Push(item);

            if (PushAssociativeStack.Count == 0)
                PushAssociativeStack.Push(item);
            else
                PushAssociativeStack.Push(new(Operation.Func(item.Value, PushAssociativeStack.Peek().Value)) { Status = ElementStatus.New });

            Queue.Enqueue(item);
            _lastItem = item;
        }

        public void Dequeue()
        {
            UpdateCollections();

            if (PopStack.Count == 0)
            {
                while (PushStack.Count > 0)
                {
                    var item = PushStack.Pop();
                    PushAssociativeStack.Pop();

                    PopStack.Push(item);

                    if (PopAssociativeStack.Count == 0)
                        PopAssociativeStack.Push(item);
                    else
                        PopAssociativeStack.Push(new(Operation.Func(item.Value, PopAssociativeStack.Peek().Value)));
                }
            }

            PopStack.Peek().Status = ElementStatus.Deleted;
            PopAssociativeStack.Peek().Status = ElementStatus.Deleted;
            Queue.Peek().Status = ElementStatus.Deleted;
        }

        public void Clear() 
        { 
            foreach (var stack  in _stacks)
                stack.Clear();

            Queue.Clear();
        }

        public void Generate()
        {

        }


        private void UpdateCollections()
        {
            foreach (var stack in _stacks)
            {
                if (stack.Count == 0)
                    continue;

                if (stack.Peek().Status == ElementStatus.New)
                    stack.Peek().Status = ElementStatus.Old;

                if (stack.Peek().Status == ElementStatus.Deleted)
                    stack.Pop();
            }

            if (Queue.Count == 0)
                return;

            if (Queue.Peek().Status == ElementStatus.Deleted)
                Queue.Dequeue();

            if (Queue.Count == 0)
                _lastItem = null;
            else if (_lastItem!.Status == ElementStatus.New)
                _lastItem.Status = ElementStatus.Old;
        }

        private void UpdateAssociativeStack(ObservableStack<ElementViewModel<int>> stack, ObservableStack<ElementViewModel<int>> associativeStack)
        {
            var list = stack.ToList();
            var associativeList = associativeStack.ToList();

            for (int i = stack.Count - 2; i >= 0; i--)
                associativeList[i].Value = Operation.Func(associativeList[i + 1].Value, list[i].Value); 
        }
        #endregion
    }
}
