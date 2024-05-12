using AssociativeLibrary;
using CollectionLibrary.Associative;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using System.Windows;
using VisualizationAssociativeQueue.Models;
using VisualizationAssociativeQueue.Models.Associativity;
using Brushes = VisualizationAssociativeQueue.Infrastructure.Brushes;

namespace VisualizationAssociativeQueue.ViewModels
{
    /// <summary>
    /// Вьюмодель главного окна.
    /// </summary>
    internal class MainWindowViewModel : ObservableObject
    {
        #region Поля
        private static readonly Predicate<string?> s_validateNumber = (string? strNumber) => 
        {
            var isNumber = int.TryParse(strNumber, out int number);
            return isNumber && number >= 0; 
        };

        private readonly AssociativeQueue<int> _associativeQueue;
        private int? _lastElement;
        #endregion

        #region Свойства

        #region Индикаторы
        public IndicatorViewModel IndicatorViewModelOperation { get; private set; }
        public IndicatorViewModel IndicatorViewModelCount { get; private set; }
        public IndicatorViewModel IndicatorViewModelFirst { get; private set; }
        public IndicatorViewModel IndicatorViewModelLast { get; private set; }
        #endregion

        #region Операции
        public List<IAssociativeOperation<int>> Operations { get; private set; }
        #endregion

        #region Выбранная операция
        private IAssociativeOperation<int> _selectedOperation;
        public IAssociativeOperation<int> SelectedOperation
        {
            get => _selectedOperation;
            set
            {
                var isChangedProperty = SetProperty(ref _selectedOperation, value);

                if (!isChangedProperty)
                    return;

                _associativeQueue.Operation = SelectedOperation;
                ObservableCollectionsManager.Operation = SelectedOperation;

                #region Обновить индикатор операции
                IndicatorViewModelOperation.Name = SelectedOperation.Name;
                IndicatorViewModelOperation.Description = SelectedOperation.Description;

                if (_associativeQueue.Count != 0)
                {
                    IndicatorViewModelOperation.Value = _associativeQueue.GetResultAssociativeOperation();
                    IndicatorViewModelOperation.SolidColorBrush = Brushes.Green;
                }
                #endregion
            }
        }
        #endregion

        #region Менеджер наблюдаемых коллекций
        public ObservableCollectionsManager<int> ObservableCollectionsManager { get; private set; }
        #endregion

        #region Видимость стрелок

        #region Видимость стрелки очереди
        private Visibility _arrowQueueVisibility;
        public Visibility ArrowQueueVisibility
        {
            get => _arrowQueueVisibility;
            set => SetProperty(ref _arrowQueueVisibility, value);
        }
        #endregion

        #region Видимость стрелок пуш-стеков
        private Visibility _arrowPushStacksVisibility;
        public Visibility ArrowPushStacksVisibility
        {
            get => _arrowPushStacksVisibility;
            set => SetProperty(ref _arrowPushStacksVisibility, value);
        }
        #endregion

        #region Видимость стрелок поп-стеков
        private Visibility _arrowPopStacksVisibility;
        public Visibility ArrowPopStacksVisibility
        {
            get => _arrowPopStacksVisibility;
            set => SetProperty(ref _arrowPopStacksVisibility, value);
        }
        #endregion

        #endregion

        #endregion

        #region Команды

        #region Добавить элемент
        public RelayCommand<string?> EnqueueCommand { get; private set; }

        private void ExecuteEnqueueCommand(string? strNumber)
        {
            var number = int.Parse(strNumber!);
            _lastElement = number;

            _associativeQueue.Enqueue(number);
            ObservableCollectionsManager.Enqueue(number);

            DequeueCommand.NotifyCanExecuteChanged();

            UpdateIndicators();
            UpdateArrowsVisibility();
        }

        private bool CanExecuteEnqueueCommand(string? strNumber) => s_validateNumber(strNumber);
        #endregion

        #region Удалить элемент
        public RelayCommand DequeueCommand { get; private set; }

        private void ExecuteDequeueCommand()
        {
            _associativeQueue.Dequeue();
            ObservableCollectionsManager.Dequeue();

            DequeueCommand.NotifyCanExecuteChanged();

            #region Обновление индикаторов
            IndicatorViewModelCount.Value = _associativeQueue.Count;

            if (_associativeQueue.Count == 0)
            {
                IndicatorViewModelOperation.Value = null;
                IndicatorViewModelFirst.Value = null;
                IndicatorViewModelLast.Value = null;
            }
            else
            {
                IndicatorViewModelOperation.Value = _associativeQueue.GetResultAssociativeOperation();
                IndicatorViewModelFirst.Value = _associativeQueue.Peek();
                IndicatorViewModelLast.SolidColorBrush = Brushes.Black;
            }
            #endregion

            UpdateArrowsVisibility();
        }

        private bool CanExecuteDequeueCommand() => _associativeQueue.Count != 0;
        #endregion

        #region Очистить очередь
        public RelayCommand ClearCommand { get; private set; }

        private void ExecuteClearCommand()
        {
            _associativeQueue.Clear();
            ObservableCollectionsManager.Clear();

            DequeueCommand.NotifyCanExecuteChanged();

            UpdateIndicators();
            UpdateArrowsVisibility();
        }
        #endregion

        #region Сгенерировать очередь
        public RelayCommand<string?> GenerateCommand { get; private set; }

        private void ExecuteGenerateCommand(string? strSeed)
        {
            _associativeQueue.Clear();
            ObservableCollectionsManager.Clear();

            var seed = int.Parse(strSeed!);
            int countElements = new Random(seed).Next(3, 9);

            QueueGenerator.DoRandomQueueOperation(countElements, _associativeQueue, out int _, seed);
            QueueGenerator.DoRandomQueueOperation(countElements, ObservableCollectionsManager, out int lastElement, seed);
            _lastElement = lastElement;

            DequeueCommand.NotifyCanExecuteChanged();
            ObservableCollectionsManager.UpdateStatusesStackPeekViewModels();

            UpdateIndicators();
            UpdateArrowsVisibility();
        }

        private bool CanExecuteGenerateCommand(string? strSeed) => s_validateNumber(strSeed);
        #endregion

        #endregion


        public MainWindowViewModel()
        {
            #region Операции и выбранная операция
            string nameOperation = "Max";

            Operations = СollectorOperations.GetAssociativeOperations();
            _selectedOperation = Operations.Where(operation => operation.Name == nameOperation).First();
            #endregion

            #region Ассоциативная очередь
            _associativeQueue = new() { Operation = _selectedOperation };
            #endregion

            #region Менеджер наблюдаемых коллекций
            ObservableCollectionsManager = new(_selectedOperation);
            #endregion

            #region Индикаторы
            IndicatorViewModelOperation = new() 
            { 
                Name = nameOperation, 
                Description = _selectedOperation.Description, 

                // Значение операции null? Цвет - красный.
                // Новое значение операции отличается от старого? Цвет - зелёный, иначе чёрный.
                ChangeSolidColorBrush = (object? oldValue, object? newValue) => 
                    newValue == null ? Brushes.Red : 
                        (int?)oldValue != (int?)newValue ? Brushes.Green : Brushes.Black,
            };

            IndicatorViewModelCount = new() 
            { 
                Name = "Count", 
                Description = "Число элементов в очереди",
                Value = 0,

                // Число элементов 0? Цвет - красный, иначе чёрный. 
                ChangeSolidColorBrush = (object? _, object? newValue) => 
                    (int?)newValue == 0 ? Brushes.Red : Brushes.Black,
            };

            IndicatorViewModelFirst = new() 
            { 
                Name = "First", 
                Description = "Первый элемент в очереди",

                // Первый элемент null? Цвет - красный, иначе зелёный. 
                ChangeSolidColorBrush = (object? _, object? newValue) => 
                    newValue == null ? Brushes.Red : Brushes.Green,
            };

            IndicatorViewModelLast = new() 
            { 
                Name = "Last", 
                Description = "Последний элемент в очереди",

                // Последний элемент null? Цвет - красный, иначе зелёный. 
                ChangeSolidColorBrush = (object? _, object? newValue) => 
                    newValue == null ? Brushes.Red : Brushes.Green,
            };
            #endregion

            #region Видимость стрелок
            _arrowQueueVisibility = Visibility.Collapsed;
            _arrowPushStacksVisibility = Visibility.Collapsed;
            _arrowPopStacksVisibility = Visibility.Collapsed;
            #endregion

            #region Команды
            EnqueueCommand = new(ExecuteEnqueueCommand, CanExecuteEnqueueCommand);
            DequeueCommand = new(ExecuteDequeueCommand, CanExecuteDequeueCommand);
            ClearCommand = new(ExecuteClearCommand);
            GenerateCommand = new(ExecuteGenerateCommand, CanExecuteGenerateCommand);
            #endregion
        }


        #region Методы

        #region Обновление индикаторов
        private void UpdateIndicators([CallerMemberName] string? callerMemberName = null)
        {
            switch (callerMemberName)
            {
                case nameof(ExecuteEnqueueCommand):
                    UpdateIndicatorsAfterEnqueue();
                    break;

                case nameof(ExecuteDequeueCommand):
                    UpdateIndicatorsAfterDequeue();
                    break;

                case nameof(ExecuteClearCommand):
                    UpdateIndicatorsAfterClear();
                    break;

                case nameof(ExecuteGenerateCommand):
                    UpdateIndicatorsAfterGenerate();
                    break;
            }
        }


        private void UpdateIndicatorsAfterEnqueue()
        {
            IndicatorViewModelOperation.Value = _associativeQueue.GetResultAssociativeOperation();
            IndicatorViewModelCount.Value = _associativeQueue.Count;
            IndicatorViewModelFirst.Value = _associativeQueue.Peek();
            IndicatorViewModelLast.Value = _lastElement;

            if (_associativeQueue.Count != 1)
                IndicatorViewModelFirst.SolidColorBrush = Brushes.Black;
        }

        private void UpdateIndicatorsAfterDequeue()
        {
            if (_associativeQueue.Count == 0)
            {
                UpdateIndicatorsAfterClear();
                return;
            }

            IndicatorViewModelCount.Value = _associativeQueue.Count;
            IndicatorViewModelOperation.Value = _associativeQueue.GetResultAssociativeOperation();
            IndicatorViewModelFirst.Value = _associativeQueue.Peek();
            IndicatorViewModelLast.SolidColorBrush = Brushes.Black;
        }

        private void UpdateIndicatorsAfterClear()
        {
            IndicatorViewModelOperation.Value = null;
            IndicatorViewModelFirst.Value = null;
            IndicatorViewModelLast.Value = null;
            IndicatorViewModelCount.Value = 0;
        }

        private void UpdateIndicatorsAfterGenerate()
        {
            IndicatorViewModelOperation.Value = _associativeQueue.GetResultAssociativeOperation();
            IndicatorViewModelFirst.Value = _associativeQueue.Peek();
            IndicatorViewModelLast.Value = _lastElement;
            IndicatorViewModelCount.Value = _associativeQueue.Count;
        }
        #endregion

        #region Обновление видимостей стрелок
        private void UpdateArrowsVisibility()
        {
            ArrowQueueVisibility = ObservableCollectionsManager.Queue.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            ArrowPushStacksVisibility = ObservableCollectionsManager.PushStack.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            ArrowPopStacksVisibility = ObservableCollectionsManager.PopStack.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #endregion
    }
}
