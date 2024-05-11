using AssociativeLibrary;
using CollectionLibrary.Observable;
using VisualizationAssociativeQueue.Models.Statuses;
using VisualizationAssociativeQueue.ViewModels;

namespace VisualizationAssociativeQueue.Models
{
    /// <summary>
    /// Менеджер наблюдаемых коллекций, содержимое которых отображается пользователю.
    /// </summary>
    internal class ObservableCollectionsManager
    {
        #region Поля
        private readonly ObservableStack<ElementViewModel>[] _stacks;
        private readonly StackPeekViewModel[] _stackPeekviewModels;
        private ElementViewModel? _lastElementViewModel;
        private IAssociativeOperation<int> _operation;
        #endregion


        #region Свойства
        public ObservableStack<ElementViewModel> PushStack { get; private set; } = [];
        public ObservableStack<ElementViewModel> PopStack { get; private set; } = [];

        public ObservableStack<ElementViewModel> PushAssociativeStack { get; private set; } = [];
        public ObservableStack<ElementViewModel> PopAssociativeStack { get; private set; } = [];

        public ObservableQueue<ElementViewModel> Queue { get; private set; } = [];

        public IAssociativeOperation<int> Operation
        {
            get => _operation;
            set
            {
                _operation = value;

                UpdateAssociativeStack(PushStack, PushAssociativeStack);
                UpdateAssociativeStack(PopStack, PopAssociativeStack);

                UpdateStackPeekViewModels();

                foreach (var stackPeekViewModel in _stackPeekviewModels)
                    if (stackPeekViewModel.Value != null)
                        stackPeekViewModel.Status = StackPeekStatus.New;
            }
        }

        public StackPeekViewModel PushAssociativeStackPeekViewModel { get; private set; } = new();
        public StackPeekViewModel PopAssociativeStackPeekViewModel { get; private set; } = new();

        public StackPeekViewModel ResultAssociativeOperationViewModel { get; private set; } = new();
        #endregion


        public ObservableCollectionsManager(IAssociativeOperation<int> operation)
        {
            _stacks = [PushStack, PopStack, PushAssociativeStack, PopAssociativeStack];
            _stackPeekviewModels = [PushAssociativeStackPeekViewModel, PopAssociativeStackPeekViewModel, ResultAssociativeOperationViewModel];

            _operation = operation;
        }


        #region Методы

        #region Публичные методы
        public void Enqueue(int number)
        {
            UpdateCollections();

            var item = new ElementViewModel 
            { 
                Value = number,
                Status = ElementStatus.New 
            };

            PushStack.Push(item);

            if (PushAssociativeStack.Count == 0)
                PushAssociativeStack.Push(item);
            else
            {
                var associativeItem = new ElementViewModel() 
                { 
                    Value = Operation.Func((int)item.Value, (int)PushAssociativeStack.Peek().Value!)
                };

                PushAssociativeStack.Push(associativeItem);
            }

            Queue.Enqueue(item);
            _lastElementViewModel = item;

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
                    {
                        var associativeItem = new ElementViewModel() 
                        { 
                            Value = Operation.Func((int)item.Value!, (int)PopAssociativeStack.Peek().Value!),
                            Status = ElementStatus.Old 
                        };

                        PopAssociativeStack.Push(associativeItem);
                    }
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
        #endregion


        #region Приватные методы
        /// <summary>
        /// Во всех коллекциях меняет элементы со статусом New на Old и удаляет все элементы со статусом Deleted.
        /// </summary>
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
                _lastElementViewModel = null;
            else if (_lastElementViewModel!.Status == ElementStatus.New)
                _lastElementViewModel.Status = ElementStatus.Old;
        }

        /// <summary>
        /// Пересчитывает элементы associativeStack в соответствии stack и Operation.
        /// </summary>
        private void UpdateAssociativeStack(ObservableStack<ElementViewModel> stack, ObservableStack<ElementViewModel> associativeStack)
        {
            var list = stack.ToList();
            var associativeList = associativeStack.ToList();

            for (int i = stack.Count - 2; i >= 0; i--)
                associativeList[i].Value = Operation.Func((int)associativeList[i + 1].Value!, (int)list[i].Value!); 
        }

        /// <summary>
        /// Обновляет значения у PushAssociativeStackPeekViewModel, PopAssociativeStackPeekViewModel и ResultAssociativeOperationViewModel.
        /// </summary>
        private void UpdateStackPeekViewModels()
        {
            PushAssociativeStackPeekViewModel.Value = GetActulStackPeek(PushAssociativeStack);
            PopAssociativeStackPeekViewModel.Value = GetActulStackPeek(PopAssociativeStack);

            UpdateResultAssociativeOperationViewModel();
        }

        /// <summary>
        /// Возвращает значение актуального верхнего элемента стека. Актуальный элемент не имеет статус Deleted. 
        /// </summary>
        private object? GetActulStackPeek(ObservableStack<ElementViewModel> stack)
        {
            if (stack.Count == 0 || (stack.Count == 1 && stack.Peek().Status == ElementStatus.Deleted))
                return null;

            if (stack.Peek().Status == ElementStatus.Deleted)
            {
                var item = PopAssociativeStack.Pop();
                int value = (int)PopAssociativeStack.Peek().Value!;
                PopAssociativeStack.Push(item);

                return value;
            }

            return stack.Peek().Value;
        }

        /// <summary>
        /// Обновляет значение ResultAssociativeOperationViewModel в соответствии значений PushAssociativeStackPeekViewModel, PopAssociativeStackPeekViewModel и Operation.
        /// </summary>
        private void UpdateResultAssociativeOperationViewModel()
        {
            if (PushAssociativeStackPeekViewModel.Value == null && PopAssociativeStackPeekViewModel.Value == null)
            {
                ResultAssociativeOperationViewModel.Value = null;
                return;
            }

            if (PushAssociativeStackPeekViewModel.Value == null)
            {
                ResultAssociativeOperationViewModel.Value = PopAssociativeStackPeekViewModel.Value;
                return;
            }

            if (PopAssociativeStackPeekViewModel.Value == null)
            {
                ResultAssociativeOperationViewModel.Value = PushAssociativeStackPeekViewModel.Value;
                return;
            }

            ResultAssociativeOperationViewModel.Value = Operation.Func((int)PushAssociativeStackPeekViewModel.Value, (int)PopAssociativeStackPeekViewModel.Value);
        }
        #endregion

        #endregion
    }
}
