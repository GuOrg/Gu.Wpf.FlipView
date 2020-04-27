namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
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
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets returns a new instance of <see cref="MouseGestureTracker"/> with default settings.
        /// </summary>
        public static CompositeGestureTracker DefaultMouseAndTouch => new CompositeGestureTracker
        {
            MouseGestureTracker.Default,
            TouchGestureTracker.Default,
        };

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

                this.OnPropertyChanged();
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
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

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
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.InputElement = this.inputElement;
            item.Gestured += this.OnGestured;
            base.SetItem(index, item);
        }

        /// <summary>Raises the <see cref="PropertyChanged"/> event.</summary>
        /// <param name="propertyName">The name of the property to notify for. String.Empty or null signals to WPF that all properties have changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnGestured(object sender, GestureEventArgs e)
        {
            this.OnGestured(e);
        }
    }
}
