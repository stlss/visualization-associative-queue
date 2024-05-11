using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;
using VisualizationAssociativeQueue.Models.Statuses;
using Brushes = VisualizationAssociativeQueue.Infrastructure.Brushes;

namespace VisualizationAssociativeQueue.ViewModels
{
    /// <summary>
    /// Вьюмоделей элемента коллекции, который на экране выглядит как квадрат со значением элемента и его статусом.
    /// </summary>
    internal class ElementViewModel : ObservableObject
    {
        #region Значение
        private object? _value = null;
        public object? Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        #endregion


        #region Статус
        private ElementStatus _status = ElementStatus.New;
        public ElementStatus Status 
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
                    case ElementStatus.New:
                        return "New";

                    case ElementStatus.Deleted:
                        return "Deleted";
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
                    case ElementStatus.New:
                        return Brushes.Green;

                    case ElementStatus.Deleted:
                        return Brushes.Red;
                }

                return Brushes.Gray;
            }
        }
        #endregion
    }
}
