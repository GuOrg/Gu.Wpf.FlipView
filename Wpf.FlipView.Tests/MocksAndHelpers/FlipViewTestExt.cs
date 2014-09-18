namespace Wpf.FlipView.Tests
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using WPF.FlipView;

    public static class FlipViewTestExt
    {
        public static FieldInfo PartCurrentItem;
        public static FieldInfo PartNextItem;

        static FlipViewTestExt()
        {
            const string PART_CurrentItem = "PART_CurrentItem";
            const string PART_NextItem = "PART_NextItem";
            PartCurrentItem = typeof(FlipView).GetField(PART_CurrentItem, BindingFlags.Instance | BindingFlags.NonPublic);
            PartNextItem = typeof(FlipView).GetField(PART_NextItem, BindingFlags.Instance | BindingFlags.NonPublic);
        }
        public static void SetCurrentItem(this FlipView flipView, ContentPresenter contentPresenter)
        {
            PartCurrentItem.SetValue(flipView, contentPresenter);
        }

        public static void SetNextItem(this FlipView flipView, ContentPresenter contentPresenter)
        {
            PartNextItem.SetValue(flipView, contentPresenter);
        }

        public static void FakeTouchDown(this FlipView flipView, Point point)
        {
            var device = new FakeTouchDevice(new TouchPoint(FakeTouchDevice.Default, point, new Rect(), TouchAction.Down));
            var args = new TouchEventArgs(device,0) { RoutedEvent = UIElement.TouchDownEvent };
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