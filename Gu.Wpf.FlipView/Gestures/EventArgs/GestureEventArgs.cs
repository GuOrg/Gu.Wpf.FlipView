namespace Gu.Wpf.FlipView.Gestures
{
    using System;

    /// <summary>
    /// A gesture triggered by user input.
    /// </summary>
    public abstract class GestureEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GestureEventArgs"/> class.
        /// </summary>
        /// <param name="type">The interpreted gesture</param>
        protected GestureEventArgs(GestureType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Gets the type of interpreted gesture.
        /// </summary>
        public GestureType Type { get; }
    }
}
