namespace Gu.Wpf.FlipView.Gestures
{
    /// <summary>
    /// The type of gesture that was detected
    /// </summary>
    public enum GestureType
    {
        /// <summary>
        /// The stream of events did not match a gesture.
        /// </summary>
        Unknown,

        /// <summary>
        /// The stream of events was a swipe to the left.
        /// </summary>
        SwipeLeft,

        /// <summary>
        /// The stream of events was a swipe to the right.
        /// </summary>
        SwipeRight,
    }
}
