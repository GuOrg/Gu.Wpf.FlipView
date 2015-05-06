namespace Gu.Wpf.FlipView.Tests
{
    using System.Windows;
    using System.Windows.Input;

    using Gu.Wpf.FlipView;
    using Gu.Wpf.FlipView.Gestures;
    using Gu.Wpf.FlipView.Tests.Misc_and_helpers;

    using NUnit.Framework;

    /// <summary>
    /// Tests for the external api
    /// </summary>
    [RequiresSTA]
    public class FlipViewTests
    {
        private FlipView _flipView;

        [SetUp]
        public void SetUp()
        {
            _flipView = new FlipView();
            _flipView.Items.Add(new DummyItem(0));
            _flipView.Items.Add(new DummyItem(1));
            _flipView.Items.Add(new DummyItem(2));
        }

        [TestCase(0, false, true)]
        [TestCase(1, true, true)]
        [TestCase(2, true, false)]
        public void CanExecutePreviousAndNext(int index, bool canPrevious, bool canNext)
        {
            _flipView.SelectedIndex = index;
            Assert.AreEqual(canPrevious, NavigationCommands.BrowseBack.CanExecute(null, _flipView));
            Assert.AreEqual(canNext, NavigationCommands.BrowseForward.CanExecute(null, _flipView));
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        public void BrowseBack(int from, int expectedTo)
        {
            _flipView.SelectedIndex = from;
            NavigationCommands.BrowseBack.Execute(null, _flipView);
            Assert.AreSame(_flipView.Items[expectedTo], _flipView.SelectedItem);
        }

        [TestCase(0, 1)]
        [TestCase(2, 2)]
        public void BrowseForward(int from, int expectedTo)
        {
            _flipView.SelectedIndex = from;
            NavigationCommands.BrowseForward.Execute(null, _flipView);
            Assert.AreSame(_flipView.Items[expectedTo], _flipView.SelectedItem);
        }
    }
}