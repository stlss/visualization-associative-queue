using System.Windows;
using System.Windows.Controls;

namespace VisualizationAssociativeQueue
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Синхронизация скроллов
        private void SynchronizeLeftScrollViewers(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewerPushStack.ScrollToHorizontalOffset(e.HorizontalOffset);
            ScrollViewerPushAssociativeStack.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void SynchronizeRightScrollViewers(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewerPopStack.ScrollToHorizontalOffset(e.HorizontalOffset);
            ScrollViewerPopAssociativeStack.ScrollToHorizontalOffset(e.HorizontalOffset);
        }
        #endregion
    }
}