namespace Gu.Wpf.FlipView.Demo.Misc
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class EventBox : UserControl
    {
        public static readonly RoutedUICommand ClearCommand = new RoutedUICommand("Clear", "Clear", typeof(EventBox));

        public static readonly DependencyProperty SwipeAreaProperty = DependencyProperty.Register(
            nameof(SwipeArea),
            typeof(FrameworkElement),
            typeof(EventBox),
            new PropertyMetadata(default(FrameworkElement)));

        static EventBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventBox), new FrameworkPropertyMetadata(typeof(EventBox)));
        }

        protected EventBox()
        {
            this.CommandBindings.Add(new CommandBinding(ClearCommand, this.ClearExecuted));
        }

        public FrameworkElement SwipeArea
        {
            get => (FrameworkElement)this.GetValue(SwipeAreaProperty);
            set => this.SetValue(SwipeAreaProperty, value);
        }

        public ObservableCollection<object> Args { get; } = new ObservableCollection<object>();

        protected virtual void OnStarted(object sender, InputEventArgs e)
        {
            this.Args.Clear();
            this.Args.Add(new ArgsVm(e));
        }

        protected virtual void OnInput(object sender, InputEventArgs e)
        {
            this.Args.Add(new ArgsVm(e));
        }

        protected virtual void OnEnded(object sender, InputEventArgs e)
        {
            this.Args.Add(new ArgsVm(e));
        }

        protected virtual void ClearExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            this.Args.Clear();
        }
    }
}
