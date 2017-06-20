namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// A gesture tracker for mouse.
    /// </summary>
    public class MouseGestureTracker : GestureTrackerBase<MouseEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseGestureTracker"/> class.
        /// </summary>
        public MouseGestureTracker()
        {
            this.Subscribers = new[]
                            {
                                EventSubscriber.Create(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnStart)),
                                EventSubscriber.Create(UIElement.PreviewMouseMoveEvent, new MouseEventHandler(this.OnMove)),
                                EventSubscriber.Create(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnEnd)),
                                EventSubscriber.Create(UIElement.MouseLeaveEvent, new MouseEventHandler(this.OnEnd)),
                            };
        }

        /// <inheritdoc />
        protected override Freezable CreateInstanceCore()
        {
            return new MouseGestureTracker();
        }

        /// <inheritdoc/>
        protected override bool TryGetPoint(MouseEventArgs args, out GesturePoint point)
        {
            var inputElement = this.InputElement;
            if (inputElement == null)
            {
                point = default(GesturePoint);
                return false;
            }

            point = new GesturePoint(args.GetPosition(inputElement), args.Timestamp);
            return true;
        }
    }
}