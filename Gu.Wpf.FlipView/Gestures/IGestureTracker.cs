namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Windows;

    public interface IGestureTracker : IDisposable
    {
        UIElement InputElement { get; set; }

        IGestureInterpreter Interpreter { get; set; }

        event EventHandler<GestureEventArgs> Gestured;
    }
}