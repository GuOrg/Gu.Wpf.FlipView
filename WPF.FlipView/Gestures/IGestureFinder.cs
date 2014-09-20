namespace WPF.FlipView
{
    using System.Windows;
    using System.Windows.Input;

    public interface IGestureFinder
    {
        UIElement InputElement { get; set; }
    }
}