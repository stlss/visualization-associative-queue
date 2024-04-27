using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace VisualizationAssociativeQueue.ViewModels
{
    internal class IndicatorViewModel : ObservableObject
    {
        #region Название
        private string _name = "NoName";
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        #endregion

        #region Значение
        private object? _value = null; 
        public object? Value
        {
            get => _value;
            set
            {
                var isChangedProperty = SetProperty(ref _value, value);

                if (isChangedProperty)
                    OnPropertyChanged(nameof(DisplayValue));
            }
        }
        #endregion

        #region Отображаемое значение
        public string DisplayValue { get => Value?.ToString() ?? "None"; }
        #endregion

        #region Цвет
        private SolidColorBrush _solidColorBrush = Brushes.Black;
        public SolidColorBrush SolidColorBrush
        {
            get => _solidColorBrush;
            set => SetProperty(ref _solidColorBrush, value);
        }
        #endregion
    }
}
