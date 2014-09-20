namespace WPF.FlipView
{
    using System.Windows;

    public struct GesturePoint
    {
        public readonly Point Point;

        public readonly int Time;

        public GesturePoint(Point point, int time)
        {
            this.Point = point;
            this.Time = time;
        }
    }
}