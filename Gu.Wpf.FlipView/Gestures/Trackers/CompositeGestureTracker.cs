namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Gu.Wpf.FlipView.Internals;

    /// <summary>
    /// A tracker that tracks both mouse and touch.
    /// </summary>
    public class CompositeGestureTracker : IGestureTracker, IList
    {
        private UIElement inputElement;
        private IReadOnlyList<IGestureTracker> trackers;

        /// <inheritdoc />
        public event EventHandler<GestureEventArgs> Gestured;

        public IReadOnlyList<IGestureTracker> Trackers
        {
            get => this.trackers;

            set
            {
                if (ReferenceEquals(this.trackers, value))
                {
                    return;
                }

                this.TrackersChanged(this.trackers, value);
                this.trackers = value;
            }
        }

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
                foreach (var tracker in this.trackers.Where(x => x != null))
                {
                    tracker.InputElement = value;
                }
            }
        }

        /// <inheritdoc />
        int ICollection.Count => this.trackers?.Count ?? 0;

        /// <inheritdoc />
        object ICollection.SyncRoot => throw new NotSupportedException();

        /// <inheritdoc />
        bool ICollection.IsSynchronized => throw new NotSupportedException();

        /// <inheritdoc />
        bool IList.IsReadOnly => false;

        /// <inheritdoc />
        bool IList.IsFixedSize => false;

        /// <inheritdoc />
        object IList.this[int index]
        {
            get => this.trackers[index];
            set
            {
                var temp = this.trackers == null
                    ? new List<IGestureTracker>()
                    : new List<IGestureTracker>(this.trackers);
                temp[index] = (IGestureTracker)value;
                this.Trackers = temp;
            }
        }

        /// <inheritdoc />
        void IList.Insert(int index, object value) => throw new NotSupportedException();

        /// <inheritdoc />
        void IList.Remove(object value)
        {
            var temp = new List<IGestureTracker>(this.trackers);
            temp.Remove((IGestureTracker)value);
            this.Trackers = temp;
        }

        /// <inheritdoc />
        void IList.RemoveAt(int index)
        {
            var temp = new List<IGestureTracker>(this.trackers);
            temp.RemoveAt(index);
            this.Trackers = temp;
        }

        /// <inheritdoc />
        int IList.Add(object value)
        {
            var temp = this.trackers == null
                ? new List<IGestureTracker>()
                : new List<IGestureTracker>(this.trackers);
            temp.Add((IGestureTracker)value);
            this.Trackers = temp;
            return temp.Count - 1;
        }

        /// <inheritdoc />
        void IList.Clear() => this.Trackers = null;

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => this.trackers?.GetEnumerator() ?? EmptyEnumerator.Instance;

        /// <inheritdoc />
        void ICollection.CopyTo(Array array, int index)
        {
            if (this.trackers == null)
            {
                return;
            }

            for (var i = 0; i < this.trackers.Count; i++)
            {
                array.SetValue(this.trackers[i], i + index);
            }
        }

        /// <inheritdoc />
        bool IList.Contains(object value) => this.trackers?.Contains(value) == true;

        /// <inheritdoc />
        int IList.IndexOf(object value)
        {
            if (this.trackers == null)
            {
                return -1;
            }

            for (var i = 0; i < this.trackers.Count; i++)
            {
                if (Equals(value, this.trackers[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Notify that a gesture was detected to any subscribers.
        /// </summary>
        protected virtual void OnGestured(GestureEventArgs e)
        {
            this.Gestured?.Invoke(this, e);
        }

        private void OnGestured(object sender, GestureEventArgs e)
        {
            this.OnGestured(e);
        }

        private void TrackersChanged(IReadOnlyList<IGestureTracker> old, IReadOnlyList<IGestureTracker> @new)
        {
            if (old != null)
            {
                foreach (var tracker in old)
                {
                    if (tracker != null)
                    {
                        tracker.InputElement = null;
                        tracker.Gestured -= this.OnGestured;
                    }
                }
            }

            if (@new != null)
            {
                foreach (var tracker in @new)
                {
                    tracker.InputElement = this.InputElement;
                    tracker.Gestured += this.OnGestured;
                }
            }
        }
    }
}