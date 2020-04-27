namespace Gu.Wpf.FlipView.Tests
{
    using System.Threading;
    using System.Windows.Input;

    using Gu.Wpf.FlipView;
    using NUnit.Framework;

    [Apartment(ApartmentState.STA)]
    public static class FlipViewTests
    {
        [TestCase(0, false, true)]
        [TestCase(1, true, true)]
        [TestCase(2, true, false)]
        public static void CanExecutePreviousAndNext(int index, bool canPrevious, bool canNext)
        {
            var flipView = new FlipView();
            flipView.Items.Add(new DummyItem(0));
            flipView.Items.Add(new DummyItem(1));
            flipView.Items.Add(new DummyItem(2));
            flipView.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, index);
            Assert.AreEqual(canPrevious, NavigationCommands.BrowseBack.CanExecute(null, flipView));
            Assert.AreEqual(canNext, NavigationCommands.BrowseForward.CanExecute(null, flipView));
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        public static void BrowseBack(int from, int expectedTo)
        {
            var flipView = new FlipView();
            flipView.Items.Add(new DummyItem(0));
            flipView.Items.Add(new DummyItem(1));
            flipView.Items.Add(new DummyItem(2));
            flipView.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, from);
            NavigationCommands.BrowseBack.Execute(null, flipView);
            Assert.AreSame(flipView.Items[expectedTo], flipView.SelectedItem);
        }

        [TestCase(0, 1)]
        [TestCase(2, 2)]
        public static void BrowseForward(int from, int expectedTo)
        {
            var flipView = new FlipView();
            flipView.Items.Add(new DummyItem(0));
            flipView.Items.Add(new DummyItem(1));
            flipView.Items.Add(new DummyItem(2));
            flipView.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, from);
            NavigationCommands.BrowseForward.Execute(null, flipView);
            Assert.AreSame(flipView.Items[expectedTo], flipView.SelectedItem);
        }
    }
}
