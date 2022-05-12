namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;

    /// <summary>
    /// A gesture triggered by user input.
    /// </summary>
    public class GesturedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GesturedEventArgs"/> class.
        /// </summary>
        /// <param name="gesture">The type of gesture detected.</param>
        /// <param name="gestureEventArgs">The gesture event args with more info.</param>
        public GesturedEventArgs(GestureType gesture, GestureEventArgs gestureEventArgs)
            : base(GesturePanel.GesturedEvent)
        {
            this.Gesture = gesture;
            this.GestureEventArgs = gestureEventArgs;
        }

        /// <summary>
        /// Gets the type of gesture that was detected.
        /// </summary>
        public GestureType Gesture { get; }

        /// <summary>
        /// Gets the gesture event args with more info.
        /// </summary>
        public GestureEventArgs GestureEventArgs { get; }
    }
}
