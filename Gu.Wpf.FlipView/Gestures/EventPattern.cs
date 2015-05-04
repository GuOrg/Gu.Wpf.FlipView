namespace Gu.Wpf.FlipView.Gestures
{
    using System;
    using System.Windows;

    public class EventPattern
    {
        public EventPattern(Action<UIElement> add, Action<UIElement> remove)
        {
            Add = add;
            Remove = remove;
        }
        public Action<UIElement> Add { get; private set; }
        public Action<UIElement> Remove { get; private set; }
    }
}