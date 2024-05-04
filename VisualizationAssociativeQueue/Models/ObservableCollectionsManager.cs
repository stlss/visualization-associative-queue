using AssociativeLibrary;
using CollectionLibrary.Observable;
using VisualizationAssociativeQueue.Models.Statuses;
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

                UpdateStackPeekViewModels();
            }
        }

        public StackPeekViewModel<int?> PushStackPeekViewModel { get; private set; } = new();
        public StackPeekViewModel<int?> PopStackPeekViewModel { get; private set; } = new();

        public StackPeekViewModel<int?> ResultOperationViewModel { get; private set; } = new();
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

            UpdateStackPeekViewModels();
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

            UpdateStackPeekViewModels();
        }

        public void Clear() 
        { 
            foreach (var stack  in _stacks)
                stack.Clear();

            Queue.Clear();

            UpdateStackPeekViewModels();
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

        private int? GetPeek(ObservableStack<ElementViewModel<int>> stack)
        {
            if (stack.Count == 0 || (stack.Count == 1 && stack.Peek().Status == ElementStatus.Deleted))
                return null;

            if (stack.Peek().Status == ElementStatus.Deleted)
            {
                var item = PopAssociativeStack.Pop();
                int value = PopAssociativeStack.Peek().Value;
                PopAssociativeStack.Push(item);

                return value;
            }

            return stack.Peek().Value;
        }

        private void UpdateResultOperationViewModel()
        {
            if (PushStackPeekViewModel.Value == null && PopStackPeekViewModel.Value == null)
            {
                ResultOperationViewModel.Value = null;
                return;
            }

            if (PushStackPeekViewModel.Value == null)
            {
                ResultOperationViewModel.Value = PopStackPeekViewModel.Value;
                return;
            }

            if (PopStackPeekViewModel.Value == null)
            {
                ResultOperationViewModel.Value = PushStackPeekViewModel.Value;
                return;
            }

            ResultOperationViewModel.Value = Operation.Func((int)PushStackPeekViewModel.Value, (int)PopStackPeekViewModel.Value);
        }

        private void UpdateStackPeekViewModels()
        {
            PushStackPeekViewModel.Value = GetPeek(PushAssociativeStack);
            PopStackPeekViewModel.Value = GetPeek(PopAssociativeStack);

            UpdateResultOperationViewModel();
        }
        #endregion
    }
}
