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
        bool TryGetGesture(IReadOnlyList<GesturePoint> points, out GestureEventArgs gestureEventArgs);

        /// <summary>
        /// Check if <paramref name="eventArgs"/> can be interpreted as a gesture.
        /// Some hardware turns swipes into navigation commands.
        /// </summary>
        bool TryGetGesture(ExecutedRoutedEventArgs eventArgs, out GestureEventArgs gestureEventArgs);
    }
}