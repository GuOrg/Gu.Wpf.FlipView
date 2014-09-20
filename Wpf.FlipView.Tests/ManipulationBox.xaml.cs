using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.FlipView.Tests
{
    using WPF.FlipView;

    /// <summary>
    /// Interaction logic for ManipulationBox.xaml
    /// </summary>
    public partial class ManipulationBox : EventBox
    {
        private readonly ManipulationGestureFinder _manipulationGestureFinder;
        public ManipulationBox()
        {
            InitializeComponent();
            _manipulationGestureFinder = new ManipulationGestureFinder { InputElement = InputElement };
        }

        protected override void OnEnded(object sender, InputEventArgs e)
        {
            Args.Add(new ArgsVm(e));
            Args.Add(string.Format("Find: {0}", _manipulationGestureFinder.Find((ManipulationCompletedEventArgs)e)));
        }
    }
}
