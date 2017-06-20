namespace Gu.Wpf.FlipView.Gestures
{
    public interface IGestureInterpreter
    {
        /// <summary>
        /// Check if <paramref name="args"/> is a swipe to the right.
        /// </summary>
        bool IsSwipeRight(GestureEventArgs args);

        /// <summary>
        /// Check if <paramref name="gesture"/> is a swipe to the right.
        /// </summary>
        bool IsSwipeRight(Gesture gesture);

        /// <summary>
        /// Check if <paramref name="args"/> is a swipe to the left.
        /// </summary>
        bool IsSwipeLeft(GestureEventArgs args);

        /// <summary>
        /// Check if <paramref name="gesture"/> is a swipe to the left.
        /// </summary>
        bool IsSwipeLeft(Gesture gesture);
    }
}