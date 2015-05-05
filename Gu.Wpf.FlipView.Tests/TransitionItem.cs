namespace Gu.Wpf.FlipView.Tests
{
    using System.Windows.Media;

    public class TransitionItem
    {
        public TransitionItem(SolidColorBrush brush, string text)
        {
            Brush = brush;
            Text = text;
        }
        public SolidColorBrush Brush { get; private set; }
        public string Text { get; private set; } 
    }
}