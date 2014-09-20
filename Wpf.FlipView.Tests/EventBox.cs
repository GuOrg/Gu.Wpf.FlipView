using System.Windows;
using System.Windows.Controls;

namespace Wpf.FlipView.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class EventBox : UserControl
    {
        public static RoutedUICommand ClearCommand = new RoutedUICommand("Clear", "Clear", typeof(EventBox));


        public static readonly DependencyProperty SwipeAreaProperty = DependencyProperty.Register(
            "SwipeArea",
            typeof(FrameworkElement),
            typeof(Tests.EventBox),
            new PropertyMetadata(default(FrameworkElement)));

        private readonly ObservableCollection<object> _args = new ObservableCollection<object>();

        static EventBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventBox), new FrameworkPropertyMetadata(typeof(EventBox)));
        }
        public EventBox()
        {
            CommandBindings.Add(new CommandBinding(ClearCommand, ClearExecuted));
        }

        public FrameworkElement SwipeArea
        {
            get { return (FrameworkElement)GetValue(SwipeAreaProperty); }
            set { SetValue(SwipeAreaProperty, value); }
        }

        public ObservableCollection<object> Args
        {
            get { return _args; }
        }

        protected virtual void OnStarted(object sender, InputEventArgs e)
        {
            _args.Clear();
            _args.Add(new ArgsVm(e));
        }

        protected virtual void OnInput(object sender, InputEventArgs e)
        {
            _args.Add(new ArgsVm(e));
        }

        protected virtual void OnEnded(object sender, InputEventArgs e)
        {
            _args.Add(new ArgsVm(e));
        }

        protected virtual void ClearExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            _args.Clear();
        }
    }
}
