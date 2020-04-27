namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// The default interpreter of gestures.
    /// </summary>
    public class DefaultGestureInterpreter : IGestureInterpreter
    {
        /// <summary>
        /// Gets a new instance with default settings for mouse swipes.
        /// </summary>
        public static IGestureInterpreter Mouse => new DefaultGestureInterpreter
        {
            MaxDeviationFromHorizontal = 30,
            MinSwipeLength = 40,
            MinSwipeVelocity = 1,
        };

        /// <summary>
        /// Gets a new instance with default settings for touch swipes.
        /// </summary>
        public static IGestureInterpreter Touch => new DefaultGestureInterpreter
        {
            MaxDeviationFromHorizontal = 30,
            MinSwipeLength = 50,
            MinSwipeVelocity = 1,
        };

        /// <summary>
        /// Gets or sets the minimum velocity for a gesture to be considered a swipe.
        /// </summary>
        public double MinSwipeVelocity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the minimum length for a gesture to be considered a swipe.
        /// </summary>
        public double MinSwipeLength { get; set; } = 50;

        /// <summary>
        /// Gets or sets the maximum angle from horizontal for a swipe to be a swipe left or right.
        /// In degrees.
        /// </summary>
        public double MaxDeviationFromHorizontal { get; set; } = 30;

        /// <inheritdoc />
        public bool TryGetGesture(IReadOnlyList<GesturePoint> points, out GestureEventArgs gestureEventArgs)
        {
            gestureEventArgs = null;
            if (points is null || points.Count < 2)
            {
                return false;
            }

            var first = points[0];
            var last = points[points.Count - 1];
            var duration = last.Time - first.Time;
            var delta = last.Point - first.Point;
            var velocity = delta.Length / duration;
            if (velocity >= this.MinSwipeVelocity &&
                Math.Abs(delta.X) >= this.MinSwipeLength &&
                IsHorizontalEnough(delta, this.MaxDeviationFromHorizontal))
            {
                gestureEventArgs = delta.X < 0
                    ? new InputGestureEventArgs(GestureType.SwipeLeft, velocity, delta)
                    : new InputGestureEventArgs(GestureType.SwipeRight, velocity, delta);
            }

            return gestureEventArgs != null;
        }

        /// <inheritdoc />
        public bool TryGetGesture(ExecutedRoutedEventArgs executed, out GestureEventArgs gestureEventArgs)
        {
            if (executed is null)
            {
                throw new ArgumentNullException(nameof(executed));
            }

            gestureEventArgs = null;
            if (executed.Command == NavigationCommands.BrowseForward)
            {
                gestureEventArgs = new CommandGestureEventArgs(GestureType.SwipeLeft, executed);
            }

            if (executed.Command == NavigationCommands.BrowseBack)
            {
                gestureEventArgs = new CommandGestureEventArgs(GestureType.SwipeRight, executed);
            }

            return gestureEventArgs != null;
        }

        private static bool IsHorizontalEnough(Vector delta, double threshold)
        {
            var a = Math.Atan2(Math.Abs(delta.Y), Math.Abs(delta.X)) * 180 / Math.PI;
            return a < threshold;
        }
    }
}
