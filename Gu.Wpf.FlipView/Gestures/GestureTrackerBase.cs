namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public abstract class GestureTrackerBase<TArgs> : Freezable, IGestureTracker
    {
        private readonly List<GesturePoint> points = new List<GesturePoint>();

        private UIElement inputElement;
        private bool disposed = false;

        protected GestureTrackerBase(params EventSubscriber[] subscribers)
        {
            this.Subscribers = subscribers;
            this.Interpreter = new GestureInterpreter();
        }

        public event EventHandler<GestureEventArgs> Gestured;

        public IGestureInterpreter Interpreter { get; set; }

        public bool IsGesturing { get; protected set; }

        public UIElement InputElement
        {
            get => this.inputElement;

            set
            {
                var old = this.inputElement;
                if (old != null)
                {
                    foreach (var subscriber in this.Subscribers)
                    {
                        subscriber.RemoveHandler(old);
                    }
                }

                if (value != null)
                {
                    foreach (var subscriber in this.Subscribers)
                    {
                        subscriber.AddHandler(value);
                    }
                }

                this.inputElement = value;
            }
        }

        protected IReadOnlyList<EventSubscriber> Subscribers { get; set; }

        /// <summary>
        /// Dispose(true); //I am calling you from Dispose, it's safe
        /// GC.SuppressFinalize(this); //Hey, GC: don't bother calling finalize later
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Raise the gesture event to notify subscribers that a gesture was detected.
        /// </summary>
        /// <param name="gesture">The detected gesture</param>
        protected internal virtual void OnGestured(Gesture gesture)
        {
            this.Gestured?.Invoke(this, new GestureEventArgs(this, gesture));
        }

        /// <summary>
        /// Called at the start of a possible gesture.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event argument.</param>
        protected virtual void OnStart(object sender, TArgs e)
        {
            this.points.Clear();
            if (this.TryGetPoint(e, out GesturePoint point))
            {
                this.points.Add(point);
                this.IsGesturing = true;
            }
        }

        /// <summary>
        /// Called when position changed during a possible gesture.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event argument.</param>
        protected virtual void OnMove(object sender, TArgs e)
        {
            if (this.TryGetPoint(e, out GesturePoint point))
            {
                this.points.Add(point);
            }
        }

        /// <summary>
        /// Called at the end of a possible gesture.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event argument.</param>
        protected virtual void OnEnd(object sender, TArgs e)
        {
            if (this.IsGesturing)
            {
                if (this.TryGetPoint(e, out GesturePoint point))
                {
                    this.points.Add(point);
                }

                this.IsGesturing = false;
                this.OnGestured(new Gesture(this.points));
            }

            this.points.Clear();
        }

        /// <summary>
        /// Try creating a <see cref="GesturePoint"/> from <paramref name="args"/>
        /// </summary>
        /// <param name="args">The event argument.</param>
        /// <param name="point">The created point</param>
        /// <returns>True if a point could be created.</returns>
        protected abstract bool TryGetPoint(TArgs args, out GesturePoint point);

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">true: safe to free managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            if (disposing)
            {
                var element = this.InputElement;
                if (element != null)
                {
                    foreach (var pattern in this.Subscribers)
                    {
                        pattern.RemoveHandler(element);
                    }
                }
            }
        }
    }
}