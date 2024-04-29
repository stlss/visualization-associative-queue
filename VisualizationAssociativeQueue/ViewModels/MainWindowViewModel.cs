using AssociativeLibrary;
using CollectionLibrary.Associative;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VisualizationAssociativeQueue.Models.Associativity;
using Brushes = VisualizationAssociativeQueue.Infrastructure.Brushes;

namespace VisualizationAssociativeQueue.ViewModels
{
    internal class MainWindowViewModel : ObservableObject
    {
        #region Поля
        private readonly AssociativeQueue<int> _associativeQueue;
        #endregion

        #region Свойства

        #region Индикаторы
        public IndicatorViewModel<int?> IndicatorOperation { get; private set; }
        public IndicatorViewModel<int> IndicatorCount { get; private set; }
        public IndicatorViewModel<int?> IndicatorFirst { get; private set; }
        public IndicatorViewModel<int?> IndicatorLast { get; private set; }
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

                #region Обновить индикатор операции
                IndicatorOperation.Name = SelectedOperation.Name;
                IndicatorOperation.Description = SelectedOperation.Description;

                if (_associativeQueue.Count != 0)
                {
                    IndicatorOperation.Value = _associativeQueue.GetResultAssociativeOperation();
                    IndicatorOperation.SolidColorBrush = Brushes.Green;
                }
                #endregion
            }
        }
        #endregion

        #endregion

        #region Команды

        #region Добавить элемент
        public RelayCommand<string?> EnqueueCommand { get; private set; }

        private void ExecuteEnqueueCommand(string? strNumber)
        {
            var number = int.Parse(strNumber!);

            _associativeQueue.Enqueue(number);

            DequeueCommand.NotifyCanExecuteChanged();

            #region Обновление индикаторов
            IndicatorOperation.Value = _associativeQueue.GetResultAssociativeOperation();
            IndicatorCount.Value = _associativeQueue.Count;
            IndicatorFirst.Value = _associativeQueue.Peek();
            IndicatorLast.Value = number;

            if (_associativeQueue.Count != 1)
                IndicatorFirst.SolidColorBrush = Brushes.Black;
            #endregion
        }

        private bool CanExecuteEnqueueCommand(string? strNumber) => int.TryParse(strNumber, out _);
        #endregion

        #region Удалить элемент
        public RelayCommand DequeueCommand { get; private set; }

        private void ExecuteDequeueCommand()
        {
            _associativeQueue.Dequeue();

            DequeueCommand.NotifyCanExecuteChanged();

            #region Обновление индикаторов
            IndicatorCount.Value = _associativeQueue.Count;

            if (_associativeQueue.Count == 0)
            {
                IndicatorOperation.Value = null;
                IndicatorFirst.Value = null;
                IndicatorLast.Value = null;
            }
            else
            {
                IndicatorOperation.Value = _associativeQueue.GetResultAssociativeOperation();
                IndicatorFirst.Value = _associativeQueue.Peek();
                IndicatorLast.SolidColorBrush = Brushes.Black;
            }
            #endregion
        }

        private bool CanExecuteDequeueCommand() => _associativeQueue.Count != 0;
        #endregion

        #region
        #endregion

        #region Очистить очередь
        public RelayCommand ClearCommand { get; private set; }

        private void ExecuteClearCommand()
        {
            _associativeQueue.Clear();

            #region Обновить индикаторы
            IndicatorOperation.Value = null;
            IndicatorFirst.Value = null;
            IndicatorLast.Value = null;

            IndicatorCount.Value = 0;
            #endregion
        }
        #endregion

        #region Сгенерировать очередь
        public RelayCommand<string?> GenerateCommand { get; private set; }

        private void ExecuteGenerateCommand(string? strNumber)
        {

        }

        private bool CanExecuteGenerateCommand(string? strNumber) => int.TryParse(strNumber, out _);
        #endregion

        #endregion


        public MainWindowViewModel()
        {
            #region Ассоциативная очередь
            _associativeQueue = new() { Operation = _selectedOperation };
            #endregion

            #region Операции и выбранная операция
            string nameOperation = "Max";

            Operations = СollectorOperations.GetAssociativeOperations();
            _selectedOperation = Operations.Where(operation => operation.Name == nameOperation).First();
            #endregion

            #region Индикаторы
            IndicatorOperation = new() { 
                Name = nameOperation, Description = _selectedOperation.Description, 
                Value = null, SolidColorBrush = Brushes.Red,
                // Значение операции null? Цвет - красный.
                // Новое значение операции отличается от старого? Цвет - зелёный, иначе чёрный.
                ChangeSolidColorBrush = (int? oldValue, int? newValue) => 
                    newValue == null ? Brushes.Red : 
                        oldValue != newValue ? Brushes.Green : Brushes.Black,
            };

            IndicatorCount = new() { 
                Name = "Count", Description = "Число элементов в очереди",
                Value = 0, SolidColorBrush = Brushes.Red,
                // Число элементов 0? Цвет - красный, иначе чёрный. 
                ChangeSolidColorBrush = (int _, int newValue) => 
                    newValue == 0 ? Brushes.Red : Brushes.Black,
            };

            IndicatorFirst = new() { 
                Name = "First", Description = "Первый элемент в очереди",
                Value = null, SolidColorBrush = Brushes.Red,
                // Первый элемент null? Цвет - красный, иначе зелёный. 
                ChangeSolidColorBrush = (int? _, int? newValue) => 
                    newValue == null ? Brushes.Red : Brushes.Green,
            };

            IndicatorLast = new() { 
                Name = "Last", Description = "Последний элемент в очереди",
                Value = null, SolidColorBrush = Brushes.Red,
                // Последний элемент null? Цвет - красный, иначе зелёный. 
                ChangeSolidColorBrush = (int? _, int? newValue) => 
                    newValue == null ? Brushes.Red : Brushes.Green,
            };
            #endregion

            #region Команды
            EnqueueCommand = new(ExecuteEnqueueCommand, CanExecuteEnqueueCommand);
            DequeueCommand = new(ExecuteDequeueCommand, CanExecuteDequeueCommand);
            ClearCommand = new(ExecuteClearCommand);
            GenerateCommand = new(ExecuteGenerateCommand, CanExecuteGenerateCommand);
            #endregion
        }
    }
}
