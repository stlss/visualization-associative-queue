using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;
using VisualizationAssociativeQueue.Models;
using Brushes = VisualizationAssociativeQueue.Infrastructure.Brushes;

namespace VisualizationAssociativeQueue.ViewModels
{
    internal class ElementViewModel<T>(T value) : ObservableObject
    {
        #region Значение
        private T _value = value;
        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        #endregion

        #region Статус
        private ElementStatus _status = ElementStatus.Old;
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
