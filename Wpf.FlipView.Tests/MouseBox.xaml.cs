using System;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.FlipView.Tests
{
    /// <summary>
    /// Interaction logic for MouseBox.xaml
    /// </summary>
    public partial class MouseBox : UserControl
    {
        public MouseBox()
        {
            InitializeComponent();
        }
        private void OnClear(object sender, RoutedEventArgs e)
        {
            ArgsBox.Items.Clear();
        }

        private void OnStarted(object sender, System.Windows.Input.InputEventArgs e)
        {
            ArgsBox.Items.Clear();
            ArgsBox.Items.Add(e);
        }

        private void OnInput(object sender, System.Windows.Input.InputEventArgs e)
        {
            ArgsBox.Items.Add(e);
        }

        private void OnEnded(object sender, System.Windows.Input.InputEventArgs e)
        {
            ArgsBox.Items.Add(e);
        }
    }
}
