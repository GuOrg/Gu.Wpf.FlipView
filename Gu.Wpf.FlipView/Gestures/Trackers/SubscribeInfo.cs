namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// A class for subscribing and unsubscribing to an event.
    /// </summary>
    public class SubscribeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeInfo"/> class.
        /// </summary>
        /// <param name="addHandler">e => e.AddHandler(routedEvent, handler)</param>
        /// <param name="removeHandler"> e => e.RemoveHandler(routedEvent, handler)</param>
        public SubscribeInfo(Action<UIElement> addHandler, Action<UIElement> removeHandler)
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
        /// Create an <see cref="SubscribeInfo"/>
        /// </summary>
        /// <param name="routedEvent">The event.</param>
        /// <param name="handler">The event handler</param>
        /// <returns>A new <see cref="SubscribeInfo"/></returns>
        public static SubscribeInfo Create(RoutedEvent routedEvent, Delegate handler)
        {
            return new SubscribeInfo(
                e => e.AddHandler(routedEvent, handler),
                e => e.RemoveHandler(routedEvent, handler));
        }

        /// <summary>
        /// Create an <see cref="SubscribeInfo"/>
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="onExecuted">The executed handler</param>
        /// <returns>A new <see cref="SubscribeInfo"/></returns>
        public static SubscribeInfo Create(RoutedCommand command, ExecutedRoutedEventHandler onExecuted)
        {
            var binding = new CommandBinding(command, onExecuted);
            return new SubscribeInfo(
                x => x.CommandBindings.Add(binding),
                x => x.CommandBindings.Remove(binding));
        }
    }
}