namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;

    /// <summary>
    /// A gesture detected from mouse or touch input.
    /// </summary>
    public class InputGestureEventArgs : GestureEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputGestureEventArgs"/> class.
        /// </summary>
        /// <param name="type">The interpreted gesture.</param>
        /// <param name="velocity">The speed of the gesture.</param>
        /// <param name="delta">The size of the gesture.</param>
        public InputGestureEventArgs(GestureType type, double velocity, Vector delta)
            : base(type)
        {
            this.Velocity = velocity;
            this.Delta = delta;
        }

        /// <summary>
        /// Gets the speed of the gesture.
        /// </summary>
        public double Velocity { get; }

        /// <summary>
        /// Gets the size of the gesture.
        /// </summary>
        public Vector Delta { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Delta: ({this.Delta.X:F0}, {this.Delta.Y:F0}) Velocity: {this.Velocity:F1}";
        }
    }
}
