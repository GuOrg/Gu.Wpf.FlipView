namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// A gesture tracker for mouse.
    /// </summary>
    public class TouchGestureTracker : GestureTrackerBase<TouchEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TouchGestureTracker"/> class.
        /// </summary>
        public TouchGestureTracker()
        {
            this.Subscribers = new[]
                                {
                                    EventSubscriber.Create(UIElement.PreviewTouchDownEvent, new EventHandler<TouchEventArgs>(this.OnStart)),
                                    EventSubscriber.Create(UIElement.PreviewTouchMoveEvent, new EventHandler<TouchEventArgs>(this.OnMove)),
                                    EventSubscriber.Create(UIElement.PreviewTouchUpEvent, new EventHandler<TouchEventArgs>(this.OnEnd)),
                                    EventSubscriber.Create(UIElement.TouchLeaveEvent, new EventHandler<TouchEventArgs>(this.OnEnd)),
                                    new EventSubscriber(
                                        x => x.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, this.OnBrowseForward)),
                                        x => x.CommandBindings.Remove(new CommandBinding(NavigationCommands.BrowseForward, this.OnBrowseForward))),
                                    new EventSubscriber(
                                        x => x.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, this.OnBrowseBack)),
                                        x => x.CommandBindings.Remove(new CommandBinding(NavigationCommands.BrowseBack, this.OnBrowseBack)))
                                };
        }

        /// <inheritdoc />
        protected override Freezable CreateInstanceCore()
        {
            return new TouchGestureTracker();
        }

        /// <inheritdoc />
        protected override bool TryAddPoint(TouchEventArgs args, out GesturePoint point)
        {
            var inputElement = this.InputElement;

            if (inputElement == null)
            {
                point = default(GesturePoint);
                return false;
            }

            point = new GesturePoint(args.GetTouchPoint(inputElement).Position, args.Timestamp);
            return true;
        }

        private void OnBrowseForward(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsGesturing = false;
            this.OnGestured(new Gesture(e));
        }

        private void OnBrowseBack(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsGesturing = false;
            this.OnGestured(new Gesture(e));
        }
    }
}