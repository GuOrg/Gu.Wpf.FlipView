namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;

    /// <summary>
    /// A point in a gesture, can be a timestamped position in a mouse move.
    /// </summary>
    public struct GesturePoint
    {
        /// <summary>
        /// The position.
        /// </summary>
        public readonly Point Point;

        /// <summary>
        /// The time when the input occurred.
        /// </summary>
        public readonly int Time;

        /// <summary>
        /// Initializes a new instance of the <see cref="GesturePoint"/> struct.
        /// </summary>
        /// <param name="point">The position.</param>
        /// <param name="time">The time when the input occurred.</param>
        public GesturePoint(Point point, int time)
        {
            this.Point = point;
            this.Time = time;
        }
    }
}