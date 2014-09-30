namespace Gu.Wpf.FlipView.Tests.MocksAndHelpers
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using Gu.Wpf.FlipView;
    public static class FlipViewTestExt
    {
        public static FieldInfo IsAnimating;

        public static FieldInfo PartSwipePanel;

        static FlipViewTestExt()
        {
            PartSwipePanel = typeof(FlipView).GetField("_partSwipePanel", BindingFlags.Instance | BindingFlags.NonPublic);
            IsAnimating = typeof(FlipView).GetField("_animation", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static void SetIsAnimating(this FlipView flipView, bool isAnimating)
        {
            IsAnimating.SetValue(flipView, isAnimating ? new DoubleAnimation() : null);
        }
        public static void SetActualWidth(this FlipView flipView, double width)
        {
            var panel = (Panel)PartSwipePanel.GetValue(flipView);
            if (panel == null)
            {
                panel = new Grid { RenderSize = new Size(width, 0) };
                PartSwipePanel.SetValue(flipView, panel);
            }
            else
            {
                panel.RenderSize = new Size(width, 0);
            }
        }

        public static void FakeTouchDown(this FlipView flipView, Point point)
        {
            var device = new FakeTouchDevice(new TouchPoint(FakeTouchDevice.Default, point, new Rect(), TouchAction.Down));
            var args = new TouchEventArgs(device, 0) { RoutedEvent = UIElement.TouchDownEvent };
            flipView.RaiseEvent(args);
        }

        public static void FakeTouchMove(this FlipView flipView, Point point)
        {
            var device = new FakeTouchDevice(new TouchPoint(FakeTouchDevice.Default, point, new Rect(), TouchAction.Down));
            var args = new TouchEventArgs(device, 0) { RoutedEvent = UIElement.TouchMoveEvent };
            flipView.RaiseEvent(args);
        }

        public static void FakeTouchUp(this FlipView flipView, Point point)
        {
            var device = new FakeTouchDevice(new TouchPoint(FakeTouchDevice.Default, point, new Rect(), TouchAction.Down));
            var args = new TouchEventArgs(device, 0) { RoutedEvent = UIElement.TouchUpEvent };
            flipView.RaiseEvent(args);
        }


    }
}