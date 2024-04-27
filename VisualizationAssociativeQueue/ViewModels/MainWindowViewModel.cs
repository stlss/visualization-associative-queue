using CommunityToolkit.Mvvm.ComponentModel;
using Brushes = VisualizationAssociativeQueue.Models.Brushes;

namespace VisualizationAssociativeQueue.ViewModels
{
    internal class MainWindowViewModel : ObservableObject
    {
        #region Свойства

        #region Индикаторы очереди
        public IndicatorViewModel IndicatorOperation { get; private set; }
        public IndicatorViewModel IndicatorCount { get; private set; }
        public IndicatorViewModel IndicatorFront { get; private set; }
        public IndicatorViewModel IndicatorBack { get; private set; }
        #endregion

        #endregion

        
        public MainWindowViewModel()
        {
            #region Индикаторы очереди
            IndicatorOperation = new() { SolidColorBrush = Brushes.Red };
            IndicatorCount = new() { Name = "Count", Value = 0, SolidColorBrush = Brushes.Red };
            IndicatorFront = new() { Name = "Front", SolidColorBrush = Brushes.Red };
            IndicatorBack = new() { Name = "Back", SolidColorBrush = Brushes.Red };
            #endregion
        }
    }
}
