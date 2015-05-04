namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;

    public struct GesturePoint
    {
        public readonly Point Point;

        public readonly int Time;

        public GesturePoint(Point point, int time)
        {
            Point = point;
            Time = time;
        }
    }
}