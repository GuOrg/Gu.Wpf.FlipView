namespace WPF.FlipView
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    public abstract class GestureFinderBase<TArgs> : Freezable, IGestureFinder
    {
        public static readonly DependencyProperty MinSwipeVelocityProperty = DependencyProperty.Register(
            "MinSwipeVelocity", 
            typeof(double),
            typeof(GestureFinderBase<TArgs>), 
            new PropertyMetadata(0.0));

        public static readonly DependencyProperty MinSwipeLengthProperty = DependencyProperty.Register(
            "MinSwipeLength",
            typeof(double),
            typeof(GestureFinderBase<TArgs>),
            new PropertyMetadata(default(double)));

        protected readonly List<GesturePoint> Points = new List<GesturePoint>();
        protected bool IsGesturing;
        protected EventPattern[] Patterns;

        private readonly WeakReference<UIElement> _inputElement = new WeakReference<UIElement>(null);

        protected GestureFinderBase(params EventPattern[] patterns)
        {
            this.Patterns = patterns;
        }

        public event EventHandler<Gesture> Gestured;

        public double MinSwipeVelocity
        {
            get { return (double)GetValue(MinSwipeVelocityProperty); }
            set { SetValue(MinSwipeVelocityProperty, value); }
        }

        public double MinSwipeLength
        {
            get { return (double)GetValue(MinSwipeLengthProperty); }
            set { SetValue(MinSwipeLengthProperty, value); }
        }

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
                var old = InputElement;
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
            if (TryAddPoint(e))
            {
                this.IsGesturing = true;
            }
        }

        protected virtual void OnMove(object sender, TArgs e)
        {
            TryAddPoint(e);
        }

        protected virtual void OnEnd(object sender, TArgs e)
        {
            if (this.IsGesturing)
            {
                TryAddPoint(e);
                this.IsGesturing = false;
                this.OnGestured(new Gesture(this.Points.ToArray()));
            }
        }

        protected abstract bool TryAddPoint(TArgs args);

        protected virtual void OnGestured(Gesture e)
        {
            var handler = this.Gestured;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}