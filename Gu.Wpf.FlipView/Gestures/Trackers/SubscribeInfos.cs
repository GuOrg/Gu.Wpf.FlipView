namespace Gu.Wpf.FlipView.Gestures
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// A collection of <see cref="SubscribeInfo"/>.
    /// </summary>
    public class SubscribeInfos
    {
        private readonly IReadOnlyList<SubscribeInfo> infos;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeInfos"/> class.
        /// </summary>
        /// <param name="infos">Configuration for what events to track.</param>
        public SubscribeInfos(params SubscribeInfo[] infos)
            : this((IReadOnlyList<SubscribeInfo>)infos)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeInfos"/> class.
        /// </summary>
        /// <param name="infos">Configuration for what events to track.</param>
        public SubscribeInfos(IReadOnlyList<SubscribeInfo> infos)
        {
            this.infos = infos;
        }

        /// <summary>
        /// Add handlers for all events.
        /// </summary>
        /// <param name="element">The new element, can be null.</param>
        public void AddHandlers(UIElement element)
        {
            if (element != null)
            {
                foreach (var subscriber in this.infos)
                {
                    subscriber.AddHandler(element);
                }
            }
        }

        /// <summary>
        /// Remove handlers for all events.
        /// </summary>
        /// <param name="element">The old element, can be null.</param>
        public void RemoveHandlers(UIElement element)
        {
            if (element != null)
            {
                foreach (var subscriber in this.infos)
                {
                    subscriber.RemoveHandler(element);
                }
            }
        }
    }
}