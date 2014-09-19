namespace WPF.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class GestureFinder : Freezable
    {
        public static readonly DependencyProperty MinSwipeVelocityProperty = DependencyProperty.Register(
            "MinSwipeVelocity",
            typeof(double),
            typeof(FlipView),
            new PropertyMetadata(0.0));

        public IInputElement InputElement { get; set; }

        public double MinSwipeVelocity
        {
            get { return (double)GetValue(MinSwipeVelocityProperty); }
            set { SetValue(MinSwipeVelocityProperty, value); }
        }

        public bool IsSwipe(ManipulationDeltaEventArgs args)
        {
            if (args.ManipulationContainer != InputElement)
            {
                return false;
            }
            var delta = args.CumulativeManipulation.Translation;
            if (Math.Abs(delta.X) < 3)
            {
                return false;
            }
            //if (Math.Abs(args.Velocities.LinearVelocity.X) < MinSwipeVelocity)
            //{
            //    args.Handled = false;
            //    CurrentTransform.X = 0;
            //    return;
            //}
            if (Math.Abs(delta.X) < Math.Abs(delta.Y))
            {
                return false;
            }
            return true;
        }

        public bool IsSwipe(ManipulationCompletedEventArgs args)
        {
            if (args.ManipulationContainer != InputElement)
            {
                return false;
            }
            return true;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GestureFinder();
        }
    }
}
