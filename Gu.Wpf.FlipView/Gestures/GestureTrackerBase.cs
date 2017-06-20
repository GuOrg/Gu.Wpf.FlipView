namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public abstract class GestureTrackerBase<TArgs> : Freezable, IGestureTracker
    {
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

        protected List<GesturePoint> Points { get; } = new List<GesturePoint>();

        protected virtual void OnStart(object sender, TArgs e)
        {
            this.Points.Clear();
            if (this.TryAddPoint(e, out GesturePoint point))
            {
                this.Points.Add(point);
                this.IsGesturing = true;
            }
        }

        protected virtual void OnMove(object sender, TArgs e)
        {
            if (this.TryAddPoint(e, out GesturePoint point))
            {
                this.Points.Add(point);
            }
        }

        protected virtual void OnEnd(object sender, TArgs e)
        {
            if (this.IsGesturing)
            {
                if (this.TryAddPoint(e, out GesturePoint point))
                {
                    this.Points.Add(point);
                }

                this.IsGesturing = false;
                this.OnGestured(new Gesture(this.Points));
            }

            this.Points.Clear();
        }

        protected abstract bool TryAddPoint(TArgs args, out GesturePoint point);

        internal virtual void OnGestured(Gesture e)
        {
            this.Gestured?.Invoke(this, new GestureEventArgs(this, e));
        }

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
        /// Protected implementation of Dispose pattern. 
        /// </summary>
        /// <param name="disposing">true: safe to free managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

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

            this.disposed = true;
        }
    }
}