namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public abstract class GestureTrackerBase<TArgs> : Freezable, IGestureTracker
    {
        protected readonly List<GesturePoint> Points = new List<GesturePoint>();
        protected bool IsGesturing;
        protected EventPattern[] Patterns;
        private bool _disposed = false;

        private readonly WeakReference<UIElement> _inputElement = new WeakReference<UIElement>(null);

        protected GestureTrackerBase(params EventPattern[] patterns)
        {
            this.Patterns = patterns;
            this.Interpreter = new GestureInterpreter();
        }

        public IGestureInterpreter Interpreter { get; set; }

        public event EventHandler<GestureEventArgs> Gestured;

        public UIElement InputElement
        {
            get
            {
                UIElement target;
                this._inputElement.TryGetTarget(out  target);
                return target;
            }
            set
            {
                var old = this.InputElement;
                if (old != null)
                {
                    foreach (var pattern in this.Patterns)
                    {
                        pattern.Remove(old);
                    }
                }
                if (value != null)
                {
                    foreach (var pattern in this.Patterns)
                    {
                        pattern.Add(value);
                    }
                }
                this._inputElement.SetTarget(value);
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
            var handler = this.Gestured;
            if (handler != null)
            {
                handler(this, new GestureEventArgs(this, e));
            }
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
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                var element = this.InputElement;
                if (element != null)
                {
                    foreach (var pattern in this.Patterns)
                    {
                        pattern.Remove(element);
                    }
                }
            }
            this._disposed = true;
        }
    }
}