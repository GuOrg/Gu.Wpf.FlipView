namespace Gu.Wpf.FlipView.Tests.Misc_and_helpers
{
    /// <summary>
    /// Helper class for debugging tests, only purpose is ToString() :)
    /// </summary>
    public class DummyItem
    {
        public DummyItem(int number)
        {
            this.Number = number;
        }

        public int Number { get; }

        public override string ToString()
        {
            return $"{this.Number}";
        }
    }
}