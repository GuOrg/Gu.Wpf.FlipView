namespace Gu.Wpf.FlipView.Tests.Misc_and_helpers
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    using Gu.Wpf.FlipView;

    public static class FlipViewTestExt
    {
        private static readonly FieldInfo IsAnimating;

        private static FieldInfo partSwipePanel;

        static FlipViewTestExt()
        {
            partSwipePanel = typeof(FlipView).GetField("_partSwipePanel", BindingFlags.Instance | BindingFlags.NonPublic);
            IsAnimating = typeof(FlipView).GetField("_animation", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static void SetIsAnimating(this FlipView flipView, bool isAnimating)
        {
            IsAnimating.SetValue(flipView, isAnimating ? new DoubleAnimation() : null);
        }

        public static void FakeTouchDown(this FlipView flipView, Point point)
        {
            var device = new FakeTouchDevice(new TouchPoint(FakeTouchDevice.Default, point, default(Rect), TouchAction.Down));
            var args = new TouchEventArgs(device, 0) { RoutedEvent = UIElement.TouchDownEvent };
            flipView.RaiseEvent(args);
        }

        public static void FakeTouchMove(this FlipView flipView, Point point)
        {
            var device = new FakeTouchDevice(new TouchPoint(FakeTouchDevice.Default, point, default(Rect), TouchAction.Down));
            var args = new TouchEventArgs(device, 0) { RoutedEvent = UIElement.TouchMoveEvent };
            flipView.RaiseEvent(args);
        }

        public static void FakeTouchUp(this FlipView flipView, Point point)
        {
            var device = new FakeTouchDevice(new TouchPoint(FakeTouchDevice.Default, point, default(Rect), TouchAction.Down));
            var args = new TouchEventArgs(device, 0) { RoutedEvent = UIElement.TouchUpEvent };
            flipView.RaiseEvent(args);
        }
    }
}