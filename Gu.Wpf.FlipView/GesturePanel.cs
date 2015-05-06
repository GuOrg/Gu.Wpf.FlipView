namespace Gu.Wpf.FlipView
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    using Gu.Wpf.FlipView.Gestures;

    [DefaultEvent("Gestured")]
    public class GesturePanel : ContentControl
    {
        public static readonly RoutedEvent GesturedEvent = EventManager.RegisterRoutedEvent("Gestured", RoutingStrategy.Bubble, typeof(GesturedEventhandler), typeof(GesturePanel));
        public static readonly DependencyProperty GestureTrackerProperty = DependencyProperty.Register(
            "GestureTracker",
            typeof(IGestureTracker),
            typeof(GesturePanel),
            new PropertyMetadata(
                new TouchGestureTracker(),
                OnGestureTrackerChanged));
        static GesturePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GesturePanel), new FrameworkPropertyMetadata(typeof(GesturePanel)));
        }

        public event GesturedEventhandler Gestured
        {
            add { AddHandler(GesturedEvent, value); }
            remove { RemoveHandler(GesturedEvent, value); }
        }

        public IGestureTracker GestureTracker
        {
            get { return (IGestureTracker)GetValue(GestureTrackerProperty); }
            set { SetValue(GestureTrackerProperty, value); }
        }

        protected virtual void OnGestureTrackerChanged(IGestureTracker oldTracker, IGestureTracker newTracker)
        {
            if (oldTracker != null)
            {
                oldTracker.InputElement = null;
                oldTracker.Gestured -= OnGesture;
            }
            if (newTracker != null)
            {
                newTracker.InputElement = this;
                newTracker.Gestured += OnGesture;
            }
        }

        private static void OnGestureTrackerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((GesturePanel)o).OnGestureTrackerChanged((IGestureTracker)e.OldValue, (IGestureTracker)e.NewValue);
        }

        protected virtual void OnGesture(object sender, GestureEventArgs e)
        {
            if (GestureTracker == null || GestureTracker.Interpreter == null)
            {
                return;
            }
            var gesture = GestureType.Unknown;
            if (GestureTracker.Interpreter.IsSwipeRight(e))
            {
                gesture = GestureType.SwipeRight;
            }
            if (GestureTracker.Interpreter.IsSwipeLeft(e))
            {
                gesture = GestureType.SwipeLeft;
            }

            RaiseEvent(new GesturedEventArgs(gesture, e));
        }
    }
}
