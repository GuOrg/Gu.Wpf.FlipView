namespace Gu.Wpf.FlipView
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Gu.Wpf.FlipView.Gestures;

    /// <summary>
    /// A panel that interprets user input like mouse and touch and detects gestures.
    /// </summary>
    [DefaultEvent("Gestured")]
    public class GesturePanel : Decorator
    {
        /// <summary>Identifies the <see cref="Gestured"/> routed event.</summary>
        public static readonly RoutedEvent GesturedEvent = EventManager.RegisterRoutedEvent(
            nameof(Gestured),
            RoutingStrategy.Bubble,
            typeof(GesturedEventHandler),
            typeof(GesturePanel));

        /// <summary>Identifies the <see cref="GestureTracker"/> dependency property.</summary>
        public static readonly DependencyProperty GestureTrackerProperty = DependencyProperty.Register(
            nameof(GestureTracker),
            typeof(IGestureTracker),
            typeof(GesturePanel),
            new PropertyMetadata(
                null,
                OnGestureTrackerChanged));

        /// <summary>Identifies the <see cref="Background"/> dependency property.</summary>
        public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(
            typeof(GesturePanel),
            new FrameworkPropertyMetadata(
                Brushes.Transparent,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        static GesturePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GesturePanel), new FrameworkPropertyMetadata(typeof(GesturePanel)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GesturePanel"/> class.
        /// </summary>
        public GesturePanel()
        {
            this.GestureTracker = new TouchGestureTracker();
        }

        /// <summary>
        /// Notifies when a gesture was detected.
        /// </summary>
        public event GesturedEventHandler Gestured
        {
            add => this.AddHandler(GesturedEvent, value);
            remove => this.RemoveHandler(GesturedEvent, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="IGestureTracker"/> that interprets touch input.
        /// </summary>
        public IGestureTracker? GestureTracker
        {
            get => (IGestureTracker?)this.GetValue(GestureTrackerProperty);
            set => this.SetValue(GestureTrackerProperty, value);
        }

        /// <summary>
        /// Gets or sets the the brush used to fill the area within the panel.
        /// </summary>
        public Brush Background
        {
            get => (Brush)this.GetValue(BackgroundProperty);
            set => this.SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// In addition to the child, Border renders a background + border.  The background is drawn inside the border.
        /// </summary>
        protected override void OnRender(DrawingContext dc)
        {
            if (dc is null)
            {
                throw new System.ArgumentNullException(nameof(dc));
            }

            if (this.Background is { } background)
            {
                dc.DrawRectangle(background, null, new Rect(this.RenderSize));
            }
        }

        /// <summary>This method is invoked when the <see cref="GestureTrackerProperty"/> changes.</summary>
        /// <param name="oldTracker">The old value of <see cref="GestureTrackerProperty"/>.</param>
        /// <param name="newTracker">The new value of <see cref="GestureTrackerProperty"/>.</param>
        protected virtual void OnGestureTrackerChanged(IGestureTracker? oldTracker, IGestureTracker? newTracker)
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

        /// <summary>
        /// Called by the <see cref="GestureTracker"/> when it detects a gesture.
        /// </summary>
        /// <param name="sender">The sender <see cref="object"/>.</param>
        /// <param name="e">The <see cref="GestureEventArgs"/>.</param>
        protected virtual void OnGesture(object? sender, GestureEventArgs e)
        {
            if (e is null)
            {
                throw new System.ArgumentNullException(nameof(e));
            }

            this.RaiseEvent(new GesturedEventArgs(e.Type, e));
        }

        private static void OnGestureTrackerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (ReferenceEquals(e.OldValue, e.NewValue))
            {
                return;
            }

            ((GesturePanel)o).OnGestureTrackerChanged((IGestureTracker)e.OldValue, (IGestureTracker)e.NewValue);
        }
    }
}
