namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// A tracker that tracks both mouse and touch.
    /// </summary>
    public class CompositeGestureTracker : Collection<IGestureTracker>, IGestureTracker
    {
        /// <summary>
        /// The key for the default resource.
        /// </summary>
        public static readonly ComponentResourceKey MouseAndTouchResourceKey = new ComponentResourceKey(typeof(CompositeGestureTracker), typeof(CompositeGestureTracker));

        private UIElement inputElement;

        /// <inheritdoc />
        public event EventHandler<GestureEventArgs> Gestured;

        /// <inheritdoc />
        public UIElement InputElement
        {
            get => this.inputElement;

            set
            {
                if (ReferenceEquals(this.inputElement, value))
                {
                    return;
                }

                this.inputElement = value;
                foreach (var tracker in this.Where(x => x != null))
                {
                    tracker.InputElement = value;
                }
            }
        }

        /// <summary>
        /// Notify that a gesture was detected to any subscribers.
        /// </summary>
        protected virtual void OnGestured(GestureEventArgs e)
        {
            this.Gestured?.Invoke(this, e);
        }

        /// <inheritdoc />
        protected override void ClearItems()
        {
            foreach (var tracker in this)
            {
                tracker.InputElement = null;
                tracker.Gestured -= this.OnGestured;
            }
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, IGestureTracker item)
        {
            item.InputElement = this.inputElement;
            item.Gestured += this.OnGestured;
            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            var tracker = this.Items[index];
            tracker.InputElement = null;
            tracker.Gestured -= this.OnGestured;
            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void SetItem(int index, IGestureTracker item)
        {
            item.InputElement = this.inputElement;
            item.Gestured += this.OnGestured;
            base.SetItem(index, item);
        }

        private void OnGestured(object sender, GestureEventArgs e)
        {
            this.OnGestured(e);
        }
    }
}
