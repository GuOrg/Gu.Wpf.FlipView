//namespace WPF.FlipView
//{
//    using System;
//    using System.Windows;
//    using System.Windows.Input;

//    public class ManipulationGestureFinder : IGestureFinder
//    {
//        public UIElement InputElement { get; set; }

//        public Swipe Find(ManipulationDeltaEventArgs args)
//        {
//            if (args.ManipulationContainer != InputElement)
//            {
//                return Swipe.None;
//            }
//            var delta = args.CumulativeManipulation.Translation;
//            if (Math.Abs(delta.X) < 3)
//            {
//                return Swipe.None;
//            }
//            //if (Math.Abs(args.Velocities.LinearVelocity.X) < MinSwipeVelocity)
//            //{
//            //    args.Handled = false;
//            //    CurrentTransform.X = 0;
//            //    return;
//            //}
//            if (Math.Abs(delta.X) < Math.Abs(delta.Y))
//            {
//                return Swipe.None;
//            }
//            return delta.X<0 ? Swipe.Left : Swipe.Right;
//        }

//        public Swipe Find(ManipulationCompletedEventArgs args)
//        {
//            if (args.ManipulationContainer != InputElement)
//            {
//                return Swipe.None;
//            }
//            return args.TotalManipulation.Translation.X < 0 ? Swipe.Left : Swipe.Right;

//        }

//        protected override Freezable CreateInstanceCore()
//        {
//            return new ManipulationGestureFinder();
//        }
//    }
//}
