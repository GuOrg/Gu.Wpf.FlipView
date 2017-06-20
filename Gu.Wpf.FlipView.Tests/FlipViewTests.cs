namespace Gu.Wpf.FlipView.Tests
{
    using System.Threading;
    using System.Windows.Input;

    using Gu.Wpf.FlipView;
    using Gu.Wpf.FlipView.Tests.Misc_and_helpers;

    using NUnit.Framework;

    [Apartment(ApartmentState.STA)]
    public class FlipViewTests
    {
        private FlipView flipView;

        [SetUp]
        public void SetUp()
        {
            this.flipView = new FlipView();
            this.flipView.Items.Add(new DummyItem(0));
            this.flipView.Items.Add(new DummyItem(1));
            this.flipView.Items.Add(new DummyItem(2));
        }

        [TestCase(0, false, true)]
        [TestCase(1, true, true)]
        [TestCase(2, true, false)]
        public void CanExecutePreviousAndNext(int index, bool canPrevious, bool canNext)
        {
            this.flipView.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, index);
            Assert.AreEqual(canPrevious, NavigationCommands.BrowseBack.CanExecute(null, this.flipView));
            Assert.AreEqual(canNext, NavigationCommands.BrowseForward.CanExecute(null, this.flipView));
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        public void BrowseBack(int from, int expectedTo)
        {
            this.flipView.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, from);
            NavigationCommands.BrowseBack.Execute(null, this.flipView);
            Assert.AreSame(this.flipView.Items[expectedTo], this.flipView.SelectedItem);
        }

        [TestCase(0, 1)]
        [TestCase(2, 2)]
        public void BrowseForward(int from, int expectedTo)
        {
            this.flipView.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, from);
            NavigationCommands.BrowseForward.Execute(null, this.flipView);
            Assert.AreSame(this.flipView.Items[expectedTo], this.flipView.SelectedItem);
        }
    }
}