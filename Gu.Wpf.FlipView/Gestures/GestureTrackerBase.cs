namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public abstract class GestureTrackerBase<TArgs> : Freezable, IGestureTracker
    {
        protected readonly List<GesturePoint> Points = new List<GesturePoint>();
        private readonly WeakReference<UIElement> _inputElement = new WeakReference<UIElement>(null);
        protected bool IsGesturing;
        protected EventPattern[] Patterns;
        private bool _disposed = false;

        protected GestureTrackerBase(params EventPattern[] patterns)
        {
            Patterns = patterns;
            Interpreter = new GestureInterpreter();
        }

        public event EventHandler<GestureEventArgs> Gestured;

        public IGestureInterpreter Interpreter { get; set; }

        public UIElement InputElement
        {
            get
            {
                UIElement target;
                _inputElement.TryGetTarget(out target);
                return target;
            }
            set
            {
                var old = InputElement;
                if (old != null)
                {
                    foreach (var pattern in Patterns)
                    {
                        pattern.Remove(old);
                    }
                }
                if (value != null)
                {
                    foreach (var pattern in Patterns)
                    {
                        pattern.Add(value);
                    }
                }
                _inputElement.SetTarget(value);
            }
        }

        protected virtual void OnStart(object sender, TArgs e)
        {
            Points.Clear();
            if (TryAddPoint(e))
            {
                IsGesturing = true;
            }
        }

        protected virtual void OnMove(object sender, TArgs e)
        {
            TryAddPoint(e);
        }

        protected virtual void OnEnd(object sender, TArgs e)
        {
            if (IsGesturing)
            {
                TryAddPoint(e);
                IsGesturing = false;
                OnGestured(new Gesture(Points.ToArray()));
            }
        }

        protected abstract bool TryAddPoint(TArgs args);

        internal virtual void OnGestured(Gesture e)
        {
            var handler = Gestured;
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern. 
        /// </summary>
        /// <param name="disposing">true: safe to free managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                var element = InputElement;
                if (element != null)
                {
                    foreach (var pattern in Patterns)
                    {
                        pattern.Remove(element);
                    }
                }
            }
            _disposed = true;
        }
    }
}