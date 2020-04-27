// ReSharper disable StaticMemberInGenericType
namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// A base class for <see cref="IGestureTracker"/>.
    /// </summary>
    /// <typeparam name="TArgs">The type of the event args.</typeparam>
    public abstract class AbstractGestureTracker<TArgs> : IGestureTracker
    {
        private readonly List<GesturePoint> points = new List<GesturePoint>();
        private bool isGesturing;
        private IGestureInterpreter interpreter;
        private UIElement? inputElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGestureTracker{TArgs}"/> class.
        /// </summary>
        /// <param name="interpreter">The interpreter to use. If null <see cref="DefaultGestureInterpreter"/> is used.</param>
        protected AbstractGestureTracker(IGestureInterpreter? interpreter = null)
        {
            this.interpreter = interpreter ?? new DefaultGestureInterpreter();
        }

        /// <inheritdoc />
        public event EventHandler<GestureEventArgs>? Gestured;

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether gets a value indicating if a potential gesture is started.
        /// </summary>
        public bool IsGesturing
        {
            get => this.isGesturing;
            protected set
            {
                if (value == this.isGesturing)
                {
                    return;
                }

                this.isGesturing = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IGestureInterpreter"/> that evaluates the detected events.
        /// </summary>
        public IGestureInterpreter Interpreter
        {
            get => this.interpreter;
            set
            {
                if (ReferenceEquals(value, this.interpreter))
                {
                    return;
                }

                this.interpreter = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the input element being tracked. Setting it to null removes subscriptions.
        /// </summary>
        public UIElement? InputElement
        {
            get => this.inputElement;
            set
            {
                if (ReferenceEquals(value, this.inputElement))
                {
                    return;
                }

                var old = value;
                this.inputElement = value;
                this.OnPropertyChanged();
                this.OnInputElementChanged(old, value);
            }
        }

        /// <summary>
        /// Raise the gesture event to notify subscribers that a gesture was detected.
        /// </summary>
        /// <param name="gestureEventArgs">The detected gesture.</param>
        protected void OnGestured(GestureEventArgs gestureEventArgs)
        {
            this.Gestured?.Invoke(this, gestureEventArgs);
        }

        /// <summary>
        /// Notify a gesture for <paramref name="eventArgs"/>.
        /// </summary>
        /// <param name="eventArgs">The event args for the command.</param>
        protected void OnExecuted(ExecutedRoutedEventArgs eventArgs)
        {
            if (this.Gestured != null &&
                this.Interpreter != null)
            {
                if (this.Interpreter.TryGetGesture(eventArgs, out var gesture))
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
        protected virtual void OnStart(object? sender, TArgs e)
        {
            this.points.Clear();
            if (this.TryGetPoint(e, out var point))
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
        protected virtual void OnMove(object? sender, TArgs e)
        {
            if (this.TryGetPoint(e, out var point))
            {
                this.points.Add(point);
            }
        }

        /// <summary>
        /// Called at the end of a possible gesture.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event argument.</param>
        protected virtual void OnEnd(object? sender, TArgs e)
        {
            if (this.IsGesturing)
            {
                if (this.TryGetPoint(e, out var point))
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
        /// Try creating a <see cref="GesturePoint"/> from <paramref name="args"/>.
        /// </summary>
        /// <param name="args">The event argument.</param>
        /// <param name="point">The created point.</param>
        /// <returns>True if a point could be created.</returns>
        protected abstract bool TryGetPoint(TArgs args, out GesturePoint point);

        /// <summary>This method is invoked when the <see cref="InputElement"/> changes.</summary>
        /// <param name="oldElement">The old value of <see cref="InputElement"/>.</param>
        /// <param name="newElement">The new value of <see cref="InputElement"/>.</param>
        protected abstract void OnInputElementChanged(UIElement? oldElement, UIElement? newElement);

        /// <summary>Raises the <see cref="PropertyChanged"/> event.</summary>
        /// <param name="propertyName">The name of the property to notify for. String.Empty or null signals to WPF that all properties have changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
