using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;
using VisualizationAssociativeQueue.Models.Statuses;
using Brushes = VisualizationAssociativeQueue.Infrastructure.Brushes;

namespace VisualizationAssociativeQueue.ViewModels
{
    /// <summary>
    /// Вьюмодель верхушки стека, которая на экране выглядит как элемент коллекции, находящиеся в правом нижнем углу области визуализации.
    /// </summary>
    internal class StackPeekViewModel : ObservableObject
    {
        #region Значение
        private object? _value = null;
        public object? Value
        {
            get => _value;
            set
            {
                var isChangedProperty = SetProperty(ref _value, value);

                if (!isChangedProperty)
                {
                    if (value != null)
                        Status = StackPeekStatus.Old;

                    return;
                }

                OnPropertyChanged(nameof(DisplayValue));
                Status = value == null ? StackPeekStatus.Missing : StackPeekStatus.New;
            }
        }
        #endregion

        #region Отображаемое значение
        public string DisplayValue { get => Value?.ToString() ?? "None"; }
        #endregion


        #region Статус
        private StackPeekStatus _status = StackPeekStatus.Missing;
        public StackPeekStatus Status
        {
            get => _status;
            set
            {
                if (!SetProperty(ref _status, value))
                    return;

                OnPropertyChanged(nameof(DisplayStatus));
                OnPropertyChanged(nameof(SolidColorBrush));
            }
        }
        #endregion

        #region Отображаемый статус
        public string DisplayStatus
        {
            get
            {
                switch (Status)
                {
                    case StackPeekStatus.New:
                        return "New";

                    case StackPeekStatus.Missing:
                        return "Missing";
                }

                return string.Empty;
            }
        }
        #endregion


        #region Цвет
        public SolidColorBrush SolidColorBrush
        {
            get
            {
                switch (Status)
                {
                    case StackPeekStatus.New:
                        return Brushes.Green;

                    case StackPeekStatus.Missing:
                        return Brushes.Red;
                }

                return Brushes.Gray;
            }
        }
        #endregion
    }
}
