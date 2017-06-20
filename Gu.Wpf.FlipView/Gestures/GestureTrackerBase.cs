namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public abstract class GestureTrackerBase<TArgs> : Freezable, IGestureTracker
    {
        protected readonly List<GesturePoint> Points = new List<GesturePoint>();
        protected bool IsGesturing;
        protected EventSubscriber[] Subscribers;

        private readonly WeakReference<UIElement> inputElement = new WeakReference<UIElement>(null);

        private bool disposed = false;

        protected GestureTrackerBase(params EventSubscriber[] subscribers)
        {
            this.Subscribers = subscribers;
            this.Interpreter = new GestureInterpreter();
        }

        public event EventHandler<GestureEventArgs> Gestured;

        public IGestureInterpreter Interpreter { get; set; }

        public UIElement InputElement
        {
            get
            {
                this.inputElement.TryGetTarget(out UIElement target);
                return target;
            }

            set
            {
                var old = this.InputElement;
                if (old != null)
                {
                    foreach (var pattern in this.Subscribers)
                    {
                        pattern.RemoveHandler(old);
                    }
                }

                if (value != null)
                {
                    foreach (var pattern in this.Subscribers)
                    {
                        pattern.AddHandler(value);
                    }
                }

                this.inputElement.SetTarget(value);
            }
        }

        protected virtual void OnStart(object sender, TArgs e)
        {
            this.Points.Clear();
            if (this.TryAddPoint(e))
            {
                this.IsGesturing = true;
            }
        }

        protected virtual void OnMove(object sender, TArgs e)
        {
            this.TryAddPoint(e);
        }

        protected virtual void OnEnd(object sender, TArgs e)
        {
            if (this.IsGesturing)
            {
                this.TryAddPoint(e);
                this.IsGesturing = false;
                this.OnGestured(new Gesture(this.Points.ToArray()));
            }
        }

        protected abstract bool TryAddPoint(TArgs args);

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