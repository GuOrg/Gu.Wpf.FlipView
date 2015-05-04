namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;
    using System.Windows.Input;

    public class MouseGestureTracker : GestureTrackerBase<MouseEventArgs>
    {
        public MouseGestureTracker()
        {
            Patterns = new[]
                            {
                                new EventPattern(
                                    x => x.PreviewMouseLeftButtonDown += OnStart,
                                    x => x.PreviewMouseLeftButtonDown -= OnStart),
                                new EventPattern(
                                    x => x.PreviewMouseMove += OnMove,
                                    x => x.PreviewMouseMove -= OnMove),
                                new EventPattern(
                                    x => x.PreviewMouseLeftButtonUp += OnEnd,
                                    x => x.PreviewMouseLeftButtonDown -= OnEnd),
                                new EventPattern(
                                    x => x.MouseLeave += OnEnd,
                                    x => x.MouseLeave -= OnEnd)
                            };
        }

        protected override Freezable CreateInstanceCore()
        {
            return new MouseGestureTracker();
        }

        protected override bool TryAddPoint(MouseEventArgs args)
        {
            var inputElement = InputElement;

            if (inputElement == null)
            {
                return false;
            }
            Points.Add(new GesturePoint(args.GetPosition(inputElement), args.Timestamp));
            return true;
        }
    }
}