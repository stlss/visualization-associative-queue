using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Brushes = VisualizationAssociativeQueue.Models.Brushes;

namespace VisualizationAssociativeQueue.ViewModels
{
    internal class MainWindowViewModel : ObservableObject
    {
        #region Свойства

        #region Индикаторы
        public IndicatorViewModel IndicatorOperation { get; private set; }
        public IndicatorViewModel IndicatorCount { get; private set; }
        public IndicatorViewModel IndicatorFront { get; private set; }
        public IndicatorViewModel IndicatorBack { get; private set; }
        #endregion

        #endregion

        #region Команды

        #region Добавить элемент
        public RelayCommand<string?> PushCommand { get; private set; }

        private void ExecutePushCommand(string? strNumber)
        {

        }

        private bool CanExecutePushCommand(string? strNumber) => int.TryParse(strNumber, out _);
        #endregion

        #region Удалить элемент
        public RelayCommand PopCommand { get; private set; }

        private void ExecutePopCommand()
        {

        }

        private bool CanExecutePopCommand() => false;
        #endregion

        #region
        #endregion

        #region Очистить очередь
        public RelayCommand ClearCommand { get; private set; }

        private void ExecuteClearCommand()
        {

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
            #region Индикаторы
            IndicatorOperation = new() { SolidColorBrush = Brushes.Red };
            IndicatorCount = new() { Name = "Count", Value = 0, SolidColorBrush = Brushes.Red };
            IndicatorFront = new() { Name = "Front", SolidColorBrush = Brushes.Red };
            IndicatorBack = new() { Name = "Back", SolidColorBrush = Brushes.Red };
            #endregion

            #region Команды
            PushCommand = new(ExecutePushCommand, CanExecutePushCommand);
            PopCommand = new(ExecutePopCommand, CanExecutePopCommand);
            ClearCommand = new(ExecuteClearCommand);
            GenerateCommand = new(ExecuteGenerateCommand, CanExecuteGenerateCommand);
            #endregion
        }
    }
}
