namespace Gu.Wpf.FlipView.Gestures
{
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// An interpreter for the user input detected by <see cref="IGestureTracker"/>
    /// </summary>
    public interface IGestureInterpreter
    {
        /// <summary>
        /// Check if <paramref name="points"/> is a gesture.
        /// </summary>
        /// <param name="points">The points registered by the tracker.</param>
        bool TryGetGesture(IReadOnlyList<GesturePoint> points, out GestureEventArgs gestureEventArgs);

        /// <summary>
        /// Check if <paramref name="executed"/> can be interpreted as a gesture.
        /// Some hardware turns swipes into navigation commands.
        /// </summary>
        /// <param name="executed">The event args for the command that executed.</param>
        bool TryGetGesture(ExecutedRoutedEventArgs executed, out GestureEventArgs gestureEventArgs);
    }
}