using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.FlipView.Tests
{
    using WPF.FlipView;

    /// <summary>
    /// Interaction logic for ManipulationBox.xaml
    /// </summary>
    public partial class ManipulationBox : UserControl
    {
        private ManipulationGestureFinder _manipulationGestureFinder;
        public ManipulationBox()
        {
            InitializeComponent();
            _manipulationGestureFinder = new ManipulationGestureFinder { InputElement = InputElement };
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ArgsBox.Items.Clear();
        }

        private void OnStarted(object sender, InputEventArgs e)
        {
            ArgsBox.Items.Clear();
            ArgsBox.Items.Add(new ArgsVm(e));
        }

        private void OnInput(object sender, InputEventArgs e)
        {
            ArgsBox.Items.Add(new ArgsVm(e));
        }
      
        private void OnEnded(object sender, InputEventArgs e)
        {
            ArgsBox.Items.Add(new ArgsVm(e));
            ArgsBox.Items.Add(string.Format("Find: {0}", _manipulationGestureFinder.Find((ManipulationCompletedEventArgs) e)));
        }
    }
}
