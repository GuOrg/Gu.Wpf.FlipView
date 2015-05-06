namespace Gu.Wpf.FlipView.Gestures
{
    public interface IGestureInterpreter
    {
        bool IsSwipeRight(GestureEventArgs args);

        bool IsSwipeLeft(GestureEventArgs args);

        bool IsSwipeRight(Gesture gesture);

        bool IsSwipeLeft(Gesture gesture);
    }
}