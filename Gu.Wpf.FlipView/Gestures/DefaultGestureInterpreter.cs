namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// The default interpreter of gestures.
    /// </summary>
    public class DefaultGestureInterpreter : Freezable, IGestureInterpreter
    {
#pragma warning disable SA1600 // Elements must be documented
        /// <summary>Identifies the <see cref="MinSwipeVelocity"/> dependency property.</summary>
        public static readonly DependencyProperty MinSwipeVelocityProperty = DependencyProperty.Register(
            nameof(MinSwipeVelocity),
            typeof(double),
            typeof(DefaultGestureInterpreter),
            new PropertyMetadata(1.0));

        /// <summary>Identifies the <see cref="MinSwipeLength"/> dependency property.</summary>
        public static readonly DependencyProperty MinSwipeLengthProperty = DependencyProperty.Register(
            nameof(MinSwipeLength),
            typeof(double),
            typeof(DefaultGestureInterpreter),
            new PropertyMetadata(50.0));

        /// <summary>Identifies the <see cref="MaxDeviationFromHorizontal"/> dependency property.</summary>
        public static readonly DependencyProperty MaxDeviationFromHorizontalProperty = DependencyProperty.Register(
            nameof(MaxDeviationFromHorizontal),
            typeof(double),
            typeof(DefaultGestureInterpreter),
            new PropertyMetadata(30.0));
#pragma warning restore SA1600 // Elements must be documented

        /// <summary>
        /// Gets or sets the minimum velocity for a gesture to be considered a swipe.
        /// </summary>
        public double MinSwipeVelocity
        {
            get => (double)this.GetValue(MinSwipeVelocityProperty);
            set => this.SetValue(MinSwipeVelocityProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum length for a gesture to be considered a swipe.
        /// </summary>
        public double MinSwipeLength
        {
            get => (double)this.GetValue(MinSwipeLengthProperty);
            set => this.SetValue(MinSwipeLengthProperty, value);
        }

        /// <summary>
        /// Gets or sets the maximum angle from horizontal for a swipe to be a swipe left or right.
        /// In degrees.
        /// </summary>
        public double MaxDeviationFromHorizontal
        {
            get => (double)this.GetValue(MaxDeviationFromHorizontalProperty);
            set => this.SetValue(MaxDeviationFromHorizontalProperty, value);
        }

        /// <inheritdoc />
        public bool TryGetGesture(IReadOnlyList<GesturePoint> points, out GestureEventArgs gestureEventArgs)
        {
            gestureEventArgs = null;
            if (points == null || points.Count < 2)
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

        /// <inheritdoc />
        protected override Freezable CreateInstanceCore()
        {
            return new DefaultGestureInterpreter();
        }

        private static bool IsHorizontalEnough(Vector delta, double threshold)
        {
            var a = Math.Atan2(Math.Abs(delta.Y), Math.Abs(delta.X)) * 180 / Math.PI;
            return a < threshold;
        }
    }
}
