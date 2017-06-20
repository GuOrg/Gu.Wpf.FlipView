namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// The default interpreter of gestures.
    /// </summary>
    public class GestureInterpreter : Freezable, IGestureInterpreter
    {
#pragma warning disable SA1600 // Elements must be documented
        public static readonly DependencyProperty MinSwipeVelocityProperty = DependencyProperty.Register(
            "MinSwipeVelocity",
            typeof(double),
            typeof(GestureInterpreter),
            new PropertyMetadata(1.0));

        public static readonly DependencyProperty MinSwipeLengthProperty = DependencyProperty.Register(
            "MinSwipeLength",
            typeof(double),
            typeof(GestureInterpreter),
            new PropertyMetadata(50.0));

        public static readonly DependencyProperty MaxDeviationFromHorizontalProperty = DependencyProperty.Register(
            "MaxDeviationFromHorizontal",
            typeof(double),
            typeof(GestureInterpreter),
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
        public bool TryGetGesture(ExecutedRoutedEventArgs eventArgs, out GestureEventArgs gestureEventArgs)
        {
            gestureEventArgs = null;
            if (eventArgs.Command == NavigationCommands.BrowseForward)
            {
                gestureEventArgs = new CommandGestureEventArgs(GestureType.SwipeLeft, eventArgs);
            }

            if (eventArgs.Command == NavigationCommands.BrowseBack)
            {
                gestureEventArgs = new CommandGestureEventArgs(GestureType.SwipeRight, eventArgs);
            }

            return gestureEventArgs != null;
        }

        /// <inheritdoc />
        protected override Freezable CreateInstanceCore()
        {
            return new GestureInterpreter();
        }

        private static bool IsHorizontalEnough(Vector delta, double threshold)
        {
            var a = Math.Atan2(Math.Abs(delta.Y), Math.Abs(delta.X)) * 180 / Math.PI;
            return a < threshold;
        }
    }
}
