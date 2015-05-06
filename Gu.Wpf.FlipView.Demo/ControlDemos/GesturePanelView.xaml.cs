namespace Gu.Wpf.FlipView.Demo.ControlDemos
{
    using System.Windows.Controls;

    using Gu.Wpf.FlipView.Gestures;

    /// <summary>
    /// Interaction logic for GesturePanel.xaml
    /// </summary>
    public partial class GesturePanelView : UserControl
    {
        public GesturePanelView()
        {
            InitializeComponent();
        }

        private void GesturePanel_OnGestured(object sender, GesturedEventArgs e)
        {
            Gestures.Items.Add(e);
        }
    }
}
