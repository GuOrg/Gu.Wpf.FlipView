namespace WPF.FlipView
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class GestureInterpreter : Freezable, IGestureInterpreter
    {
        public static readonly DependencyProperty MinSwipeVelocityProperty =
            DependencyProperty.Register(
                "MinSwipeVelocity",
                typeof(double),
                typeof(GestureInterpreter),
                new PropertyMetadata(1.0));

        public static readonly DependencyProperty MinSwipeLengthProperty = DependencyProperty.Register(
            "MinSwipeLength",
            typeof(double),
            typeof(GestureInterpreter),
            new PropertyMetadata(50.0));

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

        public bool IsBack(GestureEventArgs args)
        {
            IGestureInterpreter interpreter;
            if (!TryGetInterpreter(args, out interpreter))
            {
                return false;
            }
            var gesture = args.Gesture;
            if (IsCommand(gesture, NavigationCommands.BrowseBack))
            {
                return true;
            }
            return interpreter.IsBack(gesture);
        }

        public bool IsBack(Gesture gesture)
        {
            if (IsLongEnough(gesture, MinSwipeLength) && IsFastEnough(gesture, MinSwipeVelocity) && IsHorizontalEnough(gesture))
            {
                return gesture.Delta.X < 0;
            }
            return false;
        }

        public bool IsForward(GestureEventArgs args)
        {
            IGestureInterpreter interpreter;
            if (!TryGetInterpreter(args, out interpreter))
            {
                return false;
            }
            var gesture = args.Gesture;
            if (IsCommand(gesture, NavigationCommands.BrowseForward))
            {
                return true;
            }
            return interpreter.IsForward(gesture);
        }

        public bool IsForward(Gesture gesture)
        {
            if (IsLongEnough(gesture, MinSwipeLength) && IsFastEnough(gesture, MinSwipeVelocity) && IsHorizontalEnough(gesture))
            {
                return gesture.Delta.X > 0;
            }
            return false;
        }

        private static bool IsCommand(Gesture gesture, RoutedCommand command)
        {
            return gesture.CommandArgs != null && gesture.CommandArgs.Command == command;
        }

        private static bool TryGetInterpreter(GestureEventArgs args, out  IGestureInterpreter interpreter)
        {
            interpreter = null;
            var gesture = args.Gesture;
            if (gesture == null)
            {
                return false;
            }
            var tracker = args.GestureTrackers.LastOrDefault(x => x.Interpreter != null);
            if (tracker == null || tracker.Interpreter == null)
            {

                return false;
            }
            interpreter = tracker.Interpreter;
            return true;
        }

        internal static bool IsLongEnough(Gesture gesture, double minSwipeLength)
        {
            var delta = gesture.Delta;
            if (Math.Abs(delta.X) < minSwipeLength)
            {
                return false;
            }
            return true;
        }

        internal static bool IsFastEnough(Gesture gesture, double minSwipeVelocity)
        {
            var delta = gesture.Velocity;
            if (Math.Abs(delta.X) < minSwipeVelocity)
            {
                return false;
            }
            return true;
        }

        private bool IsHorizontalEnough(Gesture gesture)
        {
            var xs = new double[] { gesture.Delta.X, gesture.Velocity.X };
            var ys = new double[] { gesture.Delta.Y, gesture.Velocity.Y };
            for (int i = 0; i < 2; i++)
            {
                if (Math.Abs(xs[i]) < (2 * Math.Abs(ys[i])))
                {
                    return false;
                }
            }
            return true;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GestureInterpreter();
        }
    }
}
