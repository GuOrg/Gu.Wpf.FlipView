namespace Gu.Wpf.FlipView.Demo.ControlDemos
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