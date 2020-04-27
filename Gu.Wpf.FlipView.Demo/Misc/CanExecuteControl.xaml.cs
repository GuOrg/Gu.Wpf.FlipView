namespace Gu.Wpf.FlipView.Demo.Misc
{
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class CanExecuteControl : UserControl
    {
        private bool canBoost = true;

        public CanExecuteControl()
        {
            this.InitializeComponent();
            this.CommandBindings.Add(new CommandBinding(MediaCommands.BoostBass, this.Executed, this.CanExecute));
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.canBoost;
        }

#pragma warning disable AvoidAsyncVoid // Avoid async void
        private async void Executed(object sender, ExecutedRoutedEventArgs e)
#pragma warning restore AvoidAsyncVoid // Avoid async void
        {
            this.canBoost = false;
            await Task.Delay(500).ConfigureAwait(true);
            this.Boosts.Items.Add("boosted");
            this.canBoost = true;
        }
    }
}
