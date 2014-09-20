namespace WPF.FlipView
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public interface IGestureTracker : IDisposable
    {
        UIElement InputElement { get; set; }

        IGestureInterpreter Interpreter { get; set; }

        event EventHandler<GestureEventArgs> Gestured;
    }
}