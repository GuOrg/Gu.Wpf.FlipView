namespace WPF.FlipView
{
    public interface IGestureInterpreter
    {
        bool IsBack(GestureEventArgs args);

        bool IsForward(GestureEventArgs args);

        bool IsBack(Gesture gesture);

        bool IsForward(Gesture gesture);
    }
}