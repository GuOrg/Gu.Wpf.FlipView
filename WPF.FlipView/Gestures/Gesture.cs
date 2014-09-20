namespace WPF.FlipView
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;


    public class Gesture 
    {
        private readonly GesturePoint[] _points;

        public Gesture(GesturePoint[] points)
        {
            this._points = points;
            var first = _points[0];
            var last = _points[_points.Length - 1];
            var duration = last.Time - first.Time;
            Delta = last.Point - first.Point;
            Velocity = new Vector(Delta.X / duration, Delta.Y / duration);
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
                return this._points;
            }
        }

        public Vector Velocity { get; private set; }
        
        public Vector Delta { get; private set; }

        public override string ToString()
        {
            if (this.CommandArgs != null)
            {
                return string.Format("Command: ", this.CommandArgs.Command);
            }
            return string.Format("Delta: ({0:F0}, {1:F0}),Velocity: ({2:F1}, {3:F1})", this.Delta.X, Delta.Y, this.Velocity.X, Velocity.Y);
        }
    }
}
