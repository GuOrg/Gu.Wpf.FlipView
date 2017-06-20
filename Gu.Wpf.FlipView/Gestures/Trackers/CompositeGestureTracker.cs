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

        protected virtual void OnGestured(GestureEventArgs e)
        {
            this.Gestured?.Invoke(this, e);
        }

        private void OnGestured(object sender, GestureEventArgs e)
        {
            this.Gestured?.Invoke(this, e);
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

        IEnumerator IEnumerable.GetEnumerator() => this.trackers?.GetEnumerator() ?? EmptyEnumerator.Instance;

        void ICollection.CopyTo(Array array, int index) => throw new NotSupportedException();

        int ICollection.Count => this.trackers?.Count ?? 0;

        object ICollection.SyncRoot => throw new NotSupportedException();

        bool ICollection.IsSynchronized => throw new NotSupportedException();

        int IList.Add(object value)
        {
            var item = (IEnumerable<IGestureTracker>)value;
            this.trackers = new List<IGestureTracker>(this.trackers?.Concat(item) ?? new[] { item });
            return this.trackers.Count - 1;
        }

        bool IList.Contains(object value) => this.trackers.Contains(value);

        void IList.Clear() => this.trackers = null;

        int IList.IndexOf(object value) => this.trackers?.Contains(value) ?? false;

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => this.trackers = t

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        object IList.this[int index]
        {
            get { return this.trackers[index]; }
            set { throw new NotImplementedException(); }
        }

        bool IList.IsReadOnly => false;

        bool IList.IsFixedSize => false;
    }
}