namespace WPF.FlipView
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Input;

    public class MouseGestureTracker : GestureTrackerBase<MouseEventArgs>
    {
        public MouseGestureTracker()
        {
            this.Patterns = new[]
                            {
                                new EventPattern(
                                    x => x.MouseLeftButtonDown += OnStart,
                                    x => x.MouseLeftButtonDown -= OnStart),
                                new EventPattern(
                                    x => x.MouseMove += OnMove,
                                    x => x.MouseMove -= OnMove),
                                new EventPattern(
                                    x => x.MouseLeftButtonUp += OnEnd,
                                    x => x.MouseLeftButtonDown -= OnEnd),
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
            this.Points.Add(new GesturePoint(args.GetPosition(inputElement), args.Timestamp));
            return true;
        }
    }
}