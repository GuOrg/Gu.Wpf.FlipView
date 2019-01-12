namespace Gu.Wpf.FlipView.Demo.UiTestWindows
{
    using System.Windows;
    using Gu.Wpf.FlipView.Gestures;

    public partial class MouseGesturePanelWindow : Window
    {
        public MouseGesturePanelWindow()
        {
            this.InitializeComponent();
        }

        private void OnGesture(object sender, GesturedEventArgs e)
        {
            this.Gestures.Items.Add(e);
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            this.Gestures.Items.Clear();
        }
    }
}
