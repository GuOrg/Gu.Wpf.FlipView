using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf.FlipView.Tests
{
    /// <summary>
    /// Interaction logic for TouchControl.xaml
    /// </summary>
    public partial class TouchControl : UserControl
    {
        public TouchControl()
        {
            InitializeComponent();
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ArgsBox.Items.Clear();
        }

        private void UIElement_OnManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            ArgsBox.Items.Add(string.Format("ManipulationBoundaryFeedbackEventArgs {0}", e));
        }
        private void UIElement_OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            ArgsBox.Items.Add(string.Format("{0}", e));
        }
        private void UIElement_OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            ArgsBox.Items.Add(string.Format("{0}", e));
        }
        private void UIElement_OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            ArgsBox.Items.Add(string.Format("{0}", e));
        }
        private void UIElement_OnManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            ArgsBox.Items.Add(string.Format("{0}", e));
        }
        private void UIElement_OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            ArgsBox.Items.Add(string.Format("{0}", e));
        }
    }
}
