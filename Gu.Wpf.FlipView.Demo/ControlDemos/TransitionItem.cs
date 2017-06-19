namespace Gu.Wpf.FlipView.Demo.ControlDemos
{
    using System.Windows.Media;

    public class TransitionItem
    {
        public TransitionItem(SolidColorBrush brush, string text)
        {
            this.Brush = brush;
            this.Text = text;
        }

        public SolidColorBrush Brush { get; }

        public string Text { get; }
    }
}