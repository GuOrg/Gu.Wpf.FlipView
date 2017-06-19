namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;
    using System.Windows.Input;

    public class MouseGestureTracker : GestureTrackerBase<MouseEventArgs>
    {
        public MouseGestureTracker()
        {
            this.Patterns = new[]
                            {
                                new EventPattern(
                                    x => x.PreviewMouseLeftButtonDown += this.OnStart,
                                    x => x.PreviewMouseLeftButtonDown -= this.OnStart),
                                new EventPattern(
                                    x => x.PreviewMouseMove += this.OnMove,
                                    x => x.PreviewMouseMove -= this.OnMove),
                                new EventPattern(
                                    x => x.PreviewMouseLeftButtonUp += this.OnEnd,
                                    x => x.PreviewMouseLeftButtonDown -= this.OnEnd),
                                new EventPattern(
                                    x => x.MouseLeave += this.OnEnd,
                                    x => x.MouseLeave -= this.OnEnd)
                            };
        }

        protected override Freezable CreateInstanceCore()
        {
            return new MouseGestureTracker();
        }

        protected override bool TryAddPoint(MouseEventArgs args)
        {
            var inputElement = this.InputElement;

            if (inputElement == null)
            {
                return false;
            }

            this.Points.Add(new GesturePoint(args.GetPosition(inputElement), args.Timestamp));
            return true;
        }
    }
}