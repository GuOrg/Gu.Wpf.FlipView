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
            this.From = @from;
            this.To = to;
            this.TimeStamp = DateTime.Now;
        }

        public Transition(int? otherIndex, int selectedIndex)
            : this(otherIndex != null ? otherIndex.Value : selectedIndex, selectedIndex)
        {
        }
    }
}