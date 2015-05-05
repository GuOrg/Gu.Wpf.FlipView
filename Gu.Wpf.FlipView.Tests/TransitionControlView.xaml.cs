using System.Windows;
using System.Windows.Controls;

namespace Gu.Wpf.FlipView.Tests
{
    /// <summary>
    /// Interaction logic for TransitionControlView.xaml
    /// </summary>
    public partial class TransitionControlView : UserControl
    {
        private Button _button;

        public TransitionControlView()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (_button != null)
            {
                _button.Content = TransitionControl.Content;
            }
            var button = (Button)e.Source;
            TransitionControl.Content = button.Content;
            _button = button;
        }
    }
}
