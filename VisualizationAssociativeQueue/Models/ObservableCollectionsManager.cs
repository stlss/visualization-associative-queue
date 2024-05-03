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
        #endregion


        #region Свойства
        public ObservableStack<ElementViewModel<int>> PushStack { get; private set; } = [];
        public ObservableStack<ElementViewModel<int>> PopStack { get; private set; } = [];

        public ObservableStack<ElementViewModel<int>> PushAssociativeStack { get; private set; } = [];
        public ObservableStack<ElementViewModel<int>> PopAssociativeStack { get; private set; } = [];

        public ObservableQueue<ElementViewModel<int>> Queue { get; private set; } = [];

        public IAssociativeOperation<int> Operation { get; private set; }
        #endregion


        public ObservableCollectionsManager(IAssociativeOperation<int> operation)
        {
            _stacks = [PushStack, PopStack, PushAssociativeStack, PopAssociativeStack];
            Operation = operation;
        }


        #region Методы
        public void Enqueue(int number)
        {
            UpdateCollections();

            var item = new ElementViewModel<int>(number);
            Queue.Enqueue(item);
            _lastItem = item;
        }

        public void Dequeue()
        {
            UpdateCollections();

            Queue.Peek().Status = ElementStatus.Deleted;
        }

        public void ChangeOperation()
        {

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
        #endregion
    }
}
