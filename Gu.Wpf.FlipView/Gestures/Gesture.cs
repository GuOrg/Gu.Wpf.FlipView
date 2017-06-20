namespace Gu.Wpf.FlipView.Gestures
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    public class Gesture
    {
        public Gesture(IReadOnlyList<GesturePoint> points)
        {
            var first = points[0];
            var last = points[points.Count - 1];
            var duration = last.Time - first.Time;
            this.Delta = last.Point - first.Point;
            this.Velocity = new Vector(this.Delta.X / duration, this.Delta.Y / duration);
        }

        public Gesture(ExecutedRoutedEventArgs commandArgs)
        {
            this.CommandArgs = commandArgs;
        }

        public ExecutedRoutedEventArgs CommandArgs { get; }

        public Vector Velocity { get; }

        public Vector Delta { get; }

        public override string ToString()
        {
            if (this.CommandArgs != null)
            {
                return $"Command: {this.CommandArgs.Command}";
            }

            return $"Delta: ({this.Delta.X:F0}, {this.Delta.Y:F0}),Velocity: ({this.Velocity.X:F1}, {this.Velocity.Y:F1})";
        }
    }
}
