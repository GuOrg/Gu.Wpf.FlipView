namespace Wpf.FlipView.Tests
{
    /// <summary>
    /// Helper class for debugging tests, only purpose is ToString() :)
    /// </summary>
    public class DummyItem
    {
        public DummyItem(int number)
        {
            Number = number;
        }
        public int Number { get; private set; }
        public override string ToString()
        {
            return string.Format("{0}", Number);
        }
    }
}