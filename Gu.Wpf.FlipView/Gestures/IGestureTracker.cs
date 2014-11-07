namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Windows;

    public interface IGestureTracker : IDisposable
    {
        event EventHandler<GestureEventArgs> Gestured;

        UIElement InputElement { get; set; }

        IGestureInterpreter Interpreter { get; set; }
    }
}