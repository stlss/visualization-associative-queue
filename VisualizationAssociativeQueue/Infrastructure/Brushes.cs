using System.Windows.Media;

namespace VisualizationAssociativeQueue.Infrastructure
{
    /// <summary>
    /// Статический класс, предоставляющий собственные цвета.
    /// </summary>
    internal static class Brushes
    {
        private static SolidColorBrush GetSolidColorBrushFromRGB(byte r, byte g, byte b) => new(Color.FromArgb(255, r, g, b));

        public static SolidColorBrush Black => GetSolidColorBrushFromRGB(0, 0, 0);
        public static SolidColorBrush Green => GetSolidColorBrushFromRGB(14, 209, 69);
        public static SolidColorBrush Gray => GetSolidColorBrushFromRGB(195, 195, 195);
        public static SolidColorBrush Red => GetSolidColorBrushFromRGB(236, 28, 36);
    }
}
