namespace Wpf.FlipView.Tests
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using NUnit.Framework;
    using WPF.FlipView;

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

        [TestCase(1, 2, -100)]
        [TestCase(2, 1, 100)]
        [TestCase(1, 0, 100)]
        public void SelectNewItem(int fromIndex, int toIndex, double expectedX)
        {
            _flipView.SetActualWidth(Math.Abs(expectedX));// set actual width
            _flipView.SelectedIndex = fromIndex; // First time does nothing cos OldValue == -1
            _flipView.SelectedIndex = toIndex;
            Assert.IsNull(_flipView.PreviousItem);
            Assert.AreSame(_flipView.Items[toIndex], _flipView.SelectedItem);
        }

        [TestCase(0, 1, 100)]
        [TestCase(1, 0, -100)]
        public void TransitionTo(int from, int to, double nextX)
        {
            _currentItem.RenderSize = new Size(Math.Abs(nextX), 0);// set actual width
            _flipView.SelectedIndex = from;
            var transitionTo =(DoubleAnimationUsingKeyFrames) _flipView.TransitionTo(@from, to);
            Assert.AreEqual(from, transitionTo.KeyFrames[0].Value);
            Assert.AreEqual(to, transitionTo.KeyFrames[1].Value);
            Assert.AreEqual(2, transitionTo.KeyFrames.Count);
            Assert.AreSame(_flipView.Items[from], _flipView.PreviousItem);
            Assert.AreSame(_flipView.Items[to],_flipView.SelectedItem );
            Assert.AreEqual(nextX, _flipView.CurrentTransform.X);
            Assert.AreEqual(0, _flipView.PreviousTransform.Transform(new Point(0, 0)).X);
        }

        [Test]
        public void CreateCurrentTransformSlideAnimation()
        {
            _currentItem.RenderSize = new Size(100, 0);// set actual width
            _flipView.TransitionTime = 100;
            var animation = (DoubleAnimationUsingKeyFrames)this._flipView.CreateCurrentTransformSlideAnimation(new Transition(0, 1));
            Assert.AreEqual(0, animation.KeyFrames[0].Value);
            Assert.AreEqual(-100, animation.KeyFrames[1].Value);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void TransitionToWhenAlreadyAnimating(int from, int to)
        {
            _flipView.SetIsAnimating(true);
            _currentItem.RenderSize = new Size(Math.Abs(100), 0);// set actual width
            _flipView.SelectedIndex = from;
            var transitionTo = _flipView.TransitionTo(@from, to);
            Assert.IsNull(transitionTo);
            Assert.AreSame(_flipView.Items[from], _flipView.PreviousItem);
            Assert.AreSame(_flipView.Items[to], _flipView.SelectedItem);
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
    }
}
