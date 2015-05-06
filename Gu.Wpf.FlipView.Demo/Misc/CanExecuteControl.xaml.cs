namespace Gu.Wpf.FlipView.Demo.Misc
{
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for CanExecuteControl.xaml
    /// </summary>
    public partial class CanExecuteControl : UserControl
    {
        private bool _canBoost = true;
        public CanExecuteControl()
        {
            InitializeComponent();
            this.CommandBindings.Add(new CommandBinding(MediaCommands.BoostBass, this.Executed, this.CanExecute));
        }
        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this._canBoost;
        }
        private async void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this._canBoost = false;
            await Task.Delay(500);
            this.Boosts.Items.Add("boosted");
            this._canBoost = true;

            //CommandManager.InvalidateRequerySuggested();
            //var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            //{
            //    RoutedEvent = MouseUpEvent
            //};
            //Button.RaiseEvent(args);
        }
    }
}
