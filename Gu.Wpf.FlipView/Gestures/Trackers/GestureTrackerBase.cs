namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// A base class for <see cref="IGestureTracker"/>
    /// </summary>
    /// <typeparam name="TArgs">The type of the event args.</typeparam>
    public abstract class GestureTrackerBase<TArgs> : Freezable, IGestureTracker
    {
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable SA1202 // Elements must be ordered by access

        private static readonly DependencyPropertyKey IsGesturingPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IsGesturing),
            typeof(bool),
            typeof(GestureTrackerBase<TArgs>),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsGesturingProperty = IsGesturingPropertyKey.DependencyProperty;

        public static readonly DependencyProperty InterpreterProperty = DependencyProperty.Register(
            nameof(Interpreter),
            typeof(IGestureInterpreter),
            typeof(GestureTrackerBase<TArgs>),
            new PropertyMetadata(default(IGestureInterpreter)));

        public static readonly DependencyProperty InputElementProperty = DependencyProperty.Register(
            nameof(InputElement),
            typeof(UIElement),
            typeof(GestureTrackerBase<TArgs>),
            new PropertyMetadata(default(UIElement), OnInputElementChanged));

#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1202 // Elements must be ordered by access

        private readonly List<GesturePoint> points = new List<GesturePoint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureTrackerBase{TArgs}"/> class.
        /// </summary>
        /// <param name="subscribers">Descriptor for how to subscribe and unsubscribe</param>
        protected GestureTrackerBase(params SubscribeInfo[] subscribers)
        {
            this.Subscribers = subscribers;
            this.Interpreter = new GestureInterpreter();
        }

        /// <inheritdoc />
        public event EventHandler<GestureEventArgs> Gestured;

        /// <summary>
        /// Gets or sets a value indicating whether gets a value indicating if a potential gesture is started.
        /// </summary>
        public bool IsGesturing
        {
            get => (bool)this.GetValue(IsGesturingProperty);
            protected set => this.SetValue(IsGesturingPropertyKey, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="IGestureInterpreter"/> that evaluates the detected events.
        /// </summary>
        public IGestureInterpreter Interpreter
        {
            get => (IGestureInterpreter)this.GetValue(InterpreterProperty);
            set => this.SetValue(InterpreterProperty, value);
        }

        /// <summary>
        /// Gets or sets the input element being tracked. Setting it to null removes subscriptions.
        /// </summary>
        public UIElement InputElement
        {
            get => (UIElement)this.GetValue(InputElementProperty);
            set => this.SetValue(InputElementProperty, value);
        }

        protected IReadOnlyList<SubscribeInfo> Subscribers { get; set; }

        /// <summary>
        /// Raise the gesture event to notify subscribers that a gesture was detected.
        /// </summary>
        /// <param name="gestureEventArgs">The detected gesture</param>
        protected void OnGestured(GestureEventArgs gestureEventArgs)
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

        private static void OnInputElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tracker = (GestureTrackerBase<TArgs>)d;
            if (e.OldValue is UIElement oldElement)
            {
                foreach (var subscriber in tracker.Subscribers)
                {
                    subscriber.RemoveHandler(oldElement);
                }
            }

            if (e.NewValue is UIElement newElement)
            {
                foreach (var subscriber in tracker.Subscribers)
                {
                    subscriber.AddHandler(newElement);
                }
            }
        }
    }
}