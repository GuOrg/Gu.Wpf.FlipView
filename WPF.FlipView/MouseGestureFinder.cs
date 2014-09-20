namespace WPF.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class MouseGestureFinder : Freezable, IGestureFinder
    {
        public static readonly DependencyProperty MinSwipeVelocityProperty = DependencyProperty.Register(
            "MinSwipeVelocity",
            typeof(double),
            typeof(FlipView),
            new PropertyMetadata(0.0));

        public static readonly DependencyProperty MinSwipeLengthProperty = DependencyProperty.Register(
            "MinSwipeLength",
            typeof(double),
            typeof(ManipulationGestureFinder),
            new PropertyMetadata(default(double)));

        private WeakReference<IInputElement> _inputElement;

        public IInputElement InputElement
        {
            get
            {
                IInputElement target;
                _inputElement.TryGetTarget(out  target);
                return target;
            }
            set
            {
                _inputElement.SetTarget(value);
            }
        }

        public double MinSwipeVelocity
        {
            get { return (double)GetValue(MinSwipeVelocityProperty); }
            set { SetValue(MinSwipeVelocityProperty, value); }
        }

        public double MinSwipeLength
        {
            get { return (double)GetValue(MinSwipeLengthProperty); }
            set { SetValue(MinSwipeLengthProperty, value); }
        }

        public Swipe Find(ManipulationDeltaEventArgs args)
        {
            if (args.ManipulationContainer != InputElement)
            {
                return Swipe.None;
            }
            var delta = args.CumulativeManipulation.Translation;
            if (Math.Abs(delta.X) < 3)
            {
                return Swipe.None;
            }
            //if (Math.Abs(args.Velocities.LinearVelocity.X) < MinSwipeVelocity)
            //{
            //    args.Handled = false;
            //    CurrentTransform.X = 0;
            //    return;
            //}
            if (Math.Abs(delta.X) < Math.Abs(delta.Y))
            {
                return Swipe.None;
            }
            return delta.X < 0 ? Swipe.Left : Swipe.Right;
        }

        public Swipe Find(ManipulationCompletedEventArgs args)
        {
            if (args.ManipulationContainer != InputElement)
            {
                return Swipe.None;
            }
            return args.TotalManipulation.Translation.X < 0 ? Swipe.Left : Swipe.Right;

        }

        protected override Freezable CreateInstanceCore()
        {
            return new ManipulationGestureFinder();
        }
    }
}