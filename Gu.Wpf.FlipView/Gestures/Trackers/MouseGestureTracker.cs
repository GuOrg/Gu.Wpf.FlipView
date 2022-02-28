namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// A gesture tracker for mouse.
    /// </summary>
    public class MouseGestureTracker : AbstractGestureTracker<MouseEventArgs>
    {
        /// <summary>
        /// The key for the default resource.
        /// </summary>
        public static readonly ComponentResourceKey ResourceKey = new(typeof(MouseGestureTracker), typeof(MouseGestureTracker));

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

        /// <summary>
        /// Gets returns a new instance of <see cref="MouseGestureTracker"/> with default settings.
        /// </summary>
        public static MouseGestureTracker Default => new()
        {
            Interpreter = DefaultGestureInterpreter.Mouse,
        };

        /// <inheritdoc/>
        protected override bool TryGetPoint(MouseEventArgs args, out GesturePoint point)
        {
            if (args is null)
            {
                throw new System.ArgumentNullException(nameof(args));
            }

            var inputElement = this.InputElement;
            if (inputElement is null)
            {
                point = default;
                return false;
            }

            point = new GesturePoint(args.GetPosition(inputElement), args.Timestamp);
            return true;
        }

        /// <inheritdoc />
        protected override void OnInputElementChanged(UIElement? oldElement, UIElement? newElement)
        {
            this.subscribers.RemoveHandlers(oldElement);
            this.subscribers.AddHandlers(newElement);
        }
    }
}
