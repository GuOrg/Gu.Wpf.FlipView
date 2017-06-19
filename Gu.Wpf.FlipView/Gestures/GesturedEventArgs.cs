namespace Gu.Wpf.FlipView.Gestures
{
    using System.Windows;

    public class GesturedEventArgs : RoutedEventArgs
    {
        public GesturedEventArgs(GestureType gesture, GestureEventArgs gestureEventArgs)
            : base(GesturePanel.GesturedEvent)
        {
            this.Gesture = gesture;
            this.GestureEventArgs = gestureEventArgs;
        }

        public GestureType Gesture { get; }

        public GestureEventArgs GestureEventArgs { get; }
    }

}