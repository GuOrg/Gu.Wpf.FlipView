namespace WPF.FlipView
{
    using System.Windows;
    using System.Windows.Input;

    public interface IGestureFinder
    {
        IInputElement InputElement { get; set; }
        Swipe Find(ManipulationDeltaEventArgs args);
        Swipe Find(ManipulationCompletedEventArgs args);
    }
}