namespace Gu.Wpf.FlipView.Tests
{
    using System.Windows;
    using System.Windows.Input;

    using Gu.Wpf.FlipView;
    using Gu.Wpf.FlipView.Gestures;
    using Gu.Wpf.FlipView.Tests.MocksAndHelpers;
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
            this._flipView = new FlipView();
            this._flipView.Items.Add(new DummyItem(0));
            this._flipView.Items.Add(new DummyItem(1));
            this._flipView.Items.Add(new DummyItem(2));
            this._flipView.SetActualWidth(100);
        }

        [TestCase(0, false, true)]
        [TestCase(1, true, true)]
        [TestCase(2, true, false)]
        public void CanExecutePreviousAndNext(int index, bool canPrevious, bool canNext)
        {
            this._flipView.SelectedIndex = index;
            Assert.AreEqual(canPrevious, NavigationCommands.BrowseBack.CanExecute(null, this._flipView));
            Assert.AreEqual(canNext, NavigationCommands.BrowseForward.CanExecute(null, this._flipView));
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        public void BrowseBack(int from, int expectedTo)
        {
            this._flipView.SelectedIndex = from;
            NavigationCommands.BrowseBack.Execute(null, this._flipView);
            Assert.AreSame(this._flipView.Items[expectedTo], this._flipView.SelectedItem);
        }

        [TestCase(0, 1)]
        [TestCase(2, 2)]
        public void BrowseForward(int from, int expectedTo)
        {
            this._flipView.SelectedIndex = from;
            NavigationCommands.BrowseForward.Execute(null, this._flipView);
            Assert.AreSame(this._flipView.Items[expectedTo], this._flipView.SelectedItem);
        }


        [TestCase(1000, 1, 0)]
        [TestCase(1000, 0, 0)]
        [TestCase(-1000, 1, 2)]
        [TestCase(-1000, 2, 2)]
        public void ReactOnGestureTest(double dx, int from, int expectedTo)
        {
            var tracker = new TouchGestureTracker() { Interpreter = new GestureInterpreter() };
            this._flipView.GestureTracker = tracker;
            this._flipView.TransitionTime = 0;

            this._flipView.SelectedIndex = from;
            var gesture = new Gesture(new[] { new GesturePoint(new Point(0, 0), 0), new GesturePoint(new Point(dx, 0), 50) });
            tracker.OnGestured(gesture);
            Assert.AreSame(this._flipView.Items[expectedTo], this._flipView.SelectedItem);
        }
    }
}