namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// A gesture tracker for mouse.
    /// </summary>
    public class TouchGestureTracker : AbstractGestureTracker<TouchEventArgs>
    {
        /// <summary>
        /// The key for the default resource.
        /// </summary>
        public static readonly ComponentResourceKey ResourceKey = new ComponentResourceKey(typeof(TouchGestureTracker), typeof(TouchGestureTracker));

        private readonly SubscribeInfos subscribers;

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchGestureTracker"/> class.
        /// </summary>
        public TouchGestureTracker()
        {
            this.subscribers = new SubscribeInfos(
                                    SubscribeInfo.Create(UIElement.PreviewTouchDownEvent, new EventHandler<TouchEventArgs>(this.OnStart)),
                                    SubscribeInfo.Create(UIElement.PreviewTouchMoveEvent, new EventHandler<TouchEventArgs>(this.OnMove)),
                                    SubscribeInfo.Create(UIElement.PreviewTouchUpEvent, new EventHandler<TouchEventArgs>(this.OnEnd)),
                                    SubscribeInfo.Create(UIElement.TouchLeaveEvent, new EventHandler<TouchEventArgs>(this.OnEnd)),
                                    SubscribeInfo.Create(NavigationCommands.BrowseForward, this.OnBrowseForward),
                                    SubscribeInfo.Create(NavigationCommands.BrowseBack, this.OnBrowseBack));
        }

        /// <summary>
        /// Gets returns a new instance of <see cref="MouseGestureTracker"/> with default settings.
        /// </summary>
        public static MouseGestureTracker Default => new MouseGestureTracker
        {
            Interpreter = DefaultGestureInterpreter.Touch,
        };

        /// <inheritdoc />
        protected override bool TryGetPoint(TouchEventArgs args, out GesturePoint point)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var inputElement = this.InputElement;

            if (inputElement is null)
            {
                point = default;
                return false;
            }

            point = new GesturePoint(args.GetTouchPoint(inputElement).Position, args.Timestamp);
            return true;
        }

        /// <inheritdoc />
        protected override void OnInputElementChanged(UIElement oldElement, UIElement newElement)
        {
            this.subscribers.RemoveHandlers(oldElement);
            this.subscribers.AddHandlers(newElement);
        }

        private void OnBrowseForward(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsGesturing = false;
            this.OnExecuted(e);
        }

        private void OnBrowseBack(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsGesturing = false;
            this.OnExecuted(e);
        }
    }
}
