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
            this.Patterns = new[]
                            {
                                EventPattern.Create(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnStart)),
                                EventPattern.Create(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnEnd)),
                                EventPattern.Create(UIElement.PreviewMouseMoveEvent, new MouseEventHandler(this.OnMove)),
                                EventPattern.Create(UIElement.MouseLeaveEvent, new MouseEventHandler(this.OnMove)),
                            };
        }

        /// <inheritdoc />
        protected override Freezable CreateInstanceCore()
        {
            return new MouseGestureTracker();
        }

        /// <inheritdoc/>
        protected override bool TryAddPoint(MouseEventArgs args)
        {
            var inputElement = this.InputElement;

            if (inputElement == null)
            {
                return false;
            }

            this.Points.Add(new GesturePoint(args.GetPosition(inputElement), args.Timestamp));
            return true;
        }
    }
}