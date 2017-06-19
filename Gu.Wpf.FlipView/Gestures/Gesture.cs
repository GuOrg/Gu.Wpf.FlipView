namespace Gu.Wpf.FlipView.Gestures
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    public class Gesture 
    {
        private readonly GesturePoint[] points;

        public Gesture(GesturePoint[] points)
        {
            this.points = points;
            var first = this.points[0];
            var last = this.points[this.points.Length - 1];
            var duration = last.Time - first.Time;
            this.Delta = last.Point - first.Point;
            this.Velocity = new Vector(this.Delta.X / duration, this.Delta.Y / duration);
        }

        public Gesture(ExecutedRoutedEventArgs commandArgs)
        {
            this.CommandArgs = commandArgs;
        }
   
        public ExecutedRoutedEventArgs CommandArgs { get; private set; }

        public IEnumerable<GesturePoint> Points
        {
            get
            {
                return this.points;
            }
        }

        public Vector Velocity { get; private set; }
        
        public Vector Delta { get; private set; }

        public override string ToString()
        {
            if (this.CommandArgs != null)
            {
                return string.Format("Command: {0}", this.CommandArgs.Command);
            }

            return string.Format("Delta: ({0:F0}, {1:F0}),Velocity: ({2:F1}, {3:F1})", this.Delta.X, this.Delta.Y, this.Velocity.X, this.Velocity.Y);
        }
    }
}
