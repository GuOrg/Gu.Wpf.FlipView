namespace Gu.Wpf.FlipView.Demo.UiTestWindows
{
    using System.Windows;
    using Gu.Wpf.FlipView.Gestures;

    public partial class MouseAndTouchGesturePanelWindow : Window
    {
        public MouseAndTouchGesturePanelWindow()
        {
            this.InitializeComponent();
        }

        private void OnGesture(object sender, GesturedEventArgs e)
        {
            this.Gestures.Items.Add(e);
        }
    }
}
