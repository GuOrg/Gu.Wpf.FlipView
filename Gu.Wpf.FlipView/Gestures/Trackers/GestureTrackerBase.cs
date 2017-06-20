namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    public abstract class GestureTrackerBase<TArgs> : Freezable, IGestureTracker
    {
        public static readonly DependencyPropertyKey IsGesturingPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsGesturing",
            typeof(bool), 
            typeof(GestureTrackerBase<TArgs>),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsGesturingProperty = IsGesturingPropertyKey.DependencyProperty;

        private readonly List<GesturePoint> points = new List<GesturePoint>();

        private UIElement inputElement;
        private bool disposed = false;

        protected GestureTrackerBase(params EventSubscriber[] subscribers)
        {
            this.Subscribers = subscribers;
            this.Interpreter = new GestureInterpreter();
        }

        public event EventHandler<GestureEventArgs> Gestured;

        public bool IsGesturing
        {
            get => (bool)this.GetValue(IsGesturingProperty);
            set => this.SetValue(IsGesturingPropertyKey, value);
        }

        public IGestureInterpreter Interpreter { get; set; }

        public UIElement InputElement
        {
            get => this.inputElement;

            set
            {
                if (ReferenceEquals(this.inputElement, value))
                {
                    return;
                }

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
        /// Raise the gesture event to notify subscribers that a gesture was detected.
        /// </summary>
        /// <param name="gestureEventArgs">The detected gesture</param>
        protected internal void OnGestured(GestureEventArgs gestureEventArgs)
        {
            this.Gestured?.Invoke(this, gestureEventArgs);
        }

        /// <summary>
        /// Notify a gesture for <paramref name="eventArgs"/>
        /// </summary>
        /// <param name="eventArgs">The event args for the command.</param>
        protected void OnExecuted(ExecutedRoutedEventArgs eventArgs)
        {
            if (this.Gestured != null &&
                this.Interpreter != null)
            {
                if (this.Interpreter.TryGetGesture(eventArgs, out GestureEventArgs gesture))
                {
                    this.OnGestured(gesture);
                }
            }
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
                if (this.points.Count > 1 &&
                    this.Gestured != null &&
                    this.Interpreter != null)
                {
                    if (this.Interpreter.TryGetGesture(this.points, out GestureEventArgs gesture))
                    {
                        this.OnGestured(gesture);
                    }
                }
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
    }
}