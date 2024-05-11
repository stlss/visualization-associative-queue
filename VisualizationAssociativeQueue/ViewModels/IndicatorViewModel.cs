using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace VisualizationAssociativeQueue.ViewModels
{
    /// <summary>
    /// Вьюмодель индикатора очереди, который на экране (в нижней части) выглядит как название свойства очереди и его значение.
    /// </summary>
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

        #region Описание
        private string _description = "NoDescription";
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        #endregion


        #region Значение
        private object? _value = default; 
        public object? Value
        {
            get => _value;
            set
            {
                SolidColorBrush = ChangeSolidColorBrush?.Invoke(_value, value) ?? Brushes.Black;

                var isChangedProperty = SetProperty(ref _value, value);

                if (!isChangedProperty)
                    return;

                OnPropertyChanged(nameof(DisplayValue));
            }
        }
        #endregion

        #region Отображаемое значение
        public string DisplayValue { get => Value?.ToString() ?? "None"; }
        #endregion


        #region Цвет
        private SolidColorBrush _solidColorBrush = Brushes.Red;
        public SolidColorBrush SolidColorBrush
        {
            get => _solidColorBrush;
            set => SetProperty(ref _solidColorBrush, value);
        }
        #endregion

        #region Логика изменения цвета
        public Func<object?, object?, SolidColorBrush>? ChangeSolidColorBrush { get; set; }
        #endregion
    }
}
