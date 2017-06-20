namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Windows;

    /// <summary>
    /// Listens to events in <see cref="InputElement"/> and detects gestures.
    /// </summary>
    public interface IGestureTracker
    {
        /// <summary>
        /// Notifies when a gesture is detected.
        /// </summary>
        event EventHandler<GestureEventArgs> Gestured;

        /// <summary>
        /// Gets or sets the element for which events are subscribed to to detect gestures.
        /// </summary>
        UIElement InputElement { get; set; }
    }
}