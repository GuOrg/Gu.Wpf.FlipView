namespace Gu.Wpf.FlipView
{
    using System;

    public struct Transition
    {
        public int From;

        public int To;

        public DateTime TimeStamp;

        public Transition(int @from, int to)
        {
            From = @from;
            To = to;
            TimeStamp = DateTime.Now;
        }

        public Transition(int? otherIndex, int selectedIndex)
            : this(otherIndex != null ? otherIndex.Value : selectedIndex, selectedIndex)
        {
        }
    }
}