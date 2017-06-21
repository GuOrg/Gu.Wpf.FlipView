namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// A gesture tracker for mouse.
    /// </summary>
    public class MouseGestureTracker : GestureTrackerBase<MouseEventArgs>
    {
        private readonly SubscribeInfos subscribers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseGestureTracker"/> class.
        /// </summary>
        public MouseGestureTracker()
        {
            this.subscribers = new SubscribeInfos(
                SubscribeInfo.Create(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnStart)),
                SubscribeInfo.Create(UIElement.PreviewMouseMoveEvent, new MouseEventHandler(this.OnMove)),
                SubscribeInfo.Create(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnEnd)),
                SubscribeInfo.Create(UIElement.MouseLeaveEvent, new MouseEventHandler(this.OnEnd)));
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

        /// <inheritdoc />
        protected override void OnInputElementChanged(UIElement oldElement, UIElement newElement)
        {
            this.subscribers.RemoveHandlers(oldElement);
            this.subscribers.AddHandlers(newElement);
        }
    }
}