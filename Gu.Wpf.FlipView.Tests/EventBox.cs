namespace Gu.Wpf.FlipView.Tests
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Gu.Wpf.FlipView.Tests.MocksAndHelpers;

    public class EventBox : UserControl
    {
        public static RoutedUICommand ClearCommand = new RoutedUICommand("Clear", "Clear", typeof(EventBox));

        public static readonly DependencyProperty SwipeAreaProperty = DependencyProperty.Register(
            "SwipeArea",
            typeof(FrameworkElement),
            typeof(global::Gu.Wpf.FlipView.Tests.EventBox),
            new PropertyMetadata(default(FrameworkElement)));

        private readonly ObservableCollection<object> _args = new ObservableCollection<object>();

        static EventBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventBox), new FrameworkPropertyMetadata(typeof(EventBox)));
        }
       
        public EventBox()
        {
            this.CommandBindings.Add(new CommandBinding(ClearCommand, this.ClearExecuted));
        }

        public FrameworkElement SwipeArea
        {
            get { return (FrameworkElement)this.GetValue(SwipeAreaProperty); }
            set { this.SetValue(SwipeAreaProperty, value); }
        }

        public ObservableCollection<object> Args
        {
            get { return this._args; }
        }

        protected virtual void OnStarted(object sender, InputEventArgs e)
        {
            this._args.Clear();
            this._args.Add(new ArgsVm(e));
        }

        protected virtual void OnInput(object sender, InputEventArgs e)
        {
            this._args.Add(new ArgsVm(e));
        }

        protected virtual void OnEnded(object sender, InputEventArgs e)
        {
            this._args.Add(new ArgsVm(e));
        }

        protected virtual void ClearExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            this._args.Clear();
        }
    }
}
