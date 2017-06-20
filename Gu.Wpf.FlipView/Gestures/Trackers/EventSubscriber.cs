namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Windows;

    /// <summary>
    /// A class for subscribing and unsubscribing to an event.
    /// </summary>
    public class EventSubscriber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventSubscriber"/> class.
        /// </summary>
        /// <param name="addHandler">e => e.AddHandler(routedEvent, handler)</param>
        /// <param name="removeHandler"> e => e.RemoveHandler(routedEvent, handler)</param>
        public EventSubscriber(Action<UIElement> addHandler, Action<UIElement> removeHandler)
        {
            this.AddHandler = addHandler;
            this.RemoveHandler = removeHandler;
        }

        /// <summary>
        /// Gets an action that adds a handler.
        /// </summary>
        public Action<UIElement> AddHandler { get; }

        /// <summary>
        /// Gets an action the removes a handler.
        /// </summary>
        public Action<UIElement> RemoveHandler { get; }

        /// <summary>
        /// Create an <see cref="EventSubscriber"/>
        /// </summary>
        /// <param name="routedEvent">The event.</param>
        /// <param name="handler">The event handler</param>
        /// <returns>A new <see cref="EventSubscriber"/></returns>
        public static EventSubscriber Create(RoutedEvent routedEvent, Delegate handler)
        {
            return new EventSubscriber(
                e => e.AddHandler(routedEvent, handler),
                e => e.RemoveHandler(routedEvent, handler));
        }
    }
}