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
                null,
                OnGestureTrackerChanged));

        static GesturePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GesturePanel), new FrameworkPropertyMetadata(typeof(GesturePanel)));
        }

        public GesturePanel()
        {
            this.GestureTracker = new TouchGestureTracker();
        }

        public event GesturedEventhandler Gestured
        {
            add => this.AddHandler(GesturedEvent, value);
            remove => this.RemoveHandler(GesturedEvent, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="IGestureTracker"/> that interprets touch input.
        /// </summary>
        public IGestureTracker GestureTracker
        {
            get => (IGestureTracker)this.GetValue(GestureTrackerProperty);
            set => this.SetValue(GestureTrackerProperty, value);
        }

        protected virtual void OnGestureTrackerChanged(IGestureTracker oldTracker, IGestureTracker newTracker)
        {
            if (oldTracker != null)
            {
                oldTracker.InputElement = null;
                oldTracker.Gestured -= this.OnGesture;
            }

            if (newTracker != null)
            {
                newTracker.InputElement = this;
                newTracker.Gestured += this.OnGesture;
            }
        }

        protected virtual void OnGesture(object sender, GestureEventArgs e)
        {
            if (this.GestureTracker?.Interpreter == null)
            {
                return;
            }

            var gesture = GestureType.Unknown;
            if (this.GestureTracker.Interpreter.IsSwipeRight(e))
            {
                gesture = GestureType.SwipeRight;
            }

            if (this.GestureTracker.Interpreter.IsSwipeLeft(e))
            {
                gesture = GestureType.SwipeLeft;
            }

            this.RaiseEvent(new GesturedEventArgs(gesture, e));
        }

        private static void OnGestureTrackerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((GesturePanel)o).OnGestureTrackerChanged((IGestureTracker)e.OldValue, (IGestureTracker)e.NewValue);
        }
    }
}
