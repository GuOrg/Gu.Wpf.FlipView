using System.Windows.Controls;

namespace Wpf.FlipView.Tests
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for CanExecuteControl.xaml
    /// </summary>
    public partial class CanExecuteControl : UserControl
    {
        private bool _canBoost = true;
        public CanExecuteControl()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(MediaCommands.BoostBass, Executed, CanExecute));
        }
        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _canBoost;
        }
        private async void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _canBoost = false;
            await Task.Delay(500);
            Boosts.Items.Add("boosted");
            _canBoost = true;

            //CommandManager.InvalidateRequerySuggested();
            //var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            //{
            //    RoutedEvent = MouseUpEvent
            //};
            //Button.RaiseEvent(args);
        }
    }
}
