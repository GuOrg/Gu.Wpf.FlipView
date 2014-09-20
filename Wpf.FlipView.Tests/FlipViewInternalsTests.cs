namespace Wpf.FlipView.Tests
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using NUnit.Framework;
    using WPF.FlipView;

    /// <summary>
    /// This is for testing animations. Don't really care about it being an antipattern.
    /// #pragmatic
    /// </summary>
    [RequiresSTA]
    public class FlipViewInternalsTests
    {
        private FlipView _flipView;

        [SetUp]
        public void SetUp()
        {
            this._flipView = new FlipView();
            this._flipView.Items.Add(new DummyItem(0));
            this._flipView.Items.Add(new DummyItem(1));
            this._flipView.Items.Add(new DummyItem(2));
        }

        [TestCase(1, 2, -100)]
        [TestCase(2, 1, 100)]
        [TestCase(1, 0, 100)]
        public void SelectNewItem(int fromIndex, int toIndex, double expectedX)
        {
            _flipView.SetActualWidth(Math.Abs(expectedX));// set actual width
            _flipView.SelectedIndex = fromIndex; // First time does nothing cos OldValue == -1
            _flipView.SelectedIndex = toIndex;
            Assert.IsNull(_flipView.OtherItem);
            Assert.AreSame(_flipView.Items[toIndex], _flipView.SelectedItem);
        }

        [TestCase(0, 1, 100)]
        [TestCase(1, 0, -100)]
        public void TransitionToBeforeAnimation(int from, int to, double nextX)
        {
            _flipView.SetActualWidth(Math.Abs(nextX));
            _flipView.SelectedIndex = from;
            _flipView.TransitionTo(from, to);
            Assert.AreSame(_flipView.Items[from], _flipView.OtherItem);
            Assert.AreSame(_flipView.Items[to], _flipView.SelectedItem);
            Assert.AreEqual(nextX, _flipView.SelectedItemTransform.X);
            Assert.AreEqual(0, _flipView.OtherItemTransform.Transform(new Point(0, 0)).X);
        }

        [TestCase(0, 1, -100)]
        [TestCase(1, 0, 100)]
        public void TransitionToAfterAnimation(int from, int to, double nextX)
        {
            _flipView.SetActualWidth(Math.Abs(nextX));
            _flipView.SelectedIndex = from;
            _flipView.TransitionTime = 0;
            var animation = _flipView.TransitionTo(@from, to);
            _flipView.AnimateTransition(animation);
            Assert.IsNull(_flipView.OtherItem);
            Assert.AreSame(_flipView.Items[to], _flipView.SelectedItem);
            Assert.AreEqual(0, _flipView.SelectedItemTransform.X);
            Assert.AreEqual(nextX, _flipView.OtherItemTransform.Transform(new Point(0, 0)).X);
        }

        [TestCase(0, 1, 100)]
        [TestCase(1, 0, -100)]
        public void CreateCurrentTransformSlideAnimation(int from, int to, double expectedX)
        {
            _flipView.SetActualWidth(Math.Abs(expectedX));
            _flipView.TransitionTime = 100;
            var animation = (DoubleAnimationUsingKeyFrames)this._flipView.CreateTransitionAnimation(new Transition(from, to));
            Assert.AreEqual(0, _flipView.OtherItemTransform.Transform(new Point(0,0)).X);
            
            Assert.AreEqual(expectedX, animation.KeyFrames[0].Value);
            Assert.AreEqual(0, animation.KeyFrames[1].Value);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void TransitionToWhenAlreadyAnimating(int from, int to)
        {
            _flipView.SetIsAnimating(true);
            _flipView.SetActualWidth(100);
            _flipView.SelectedIndex = from;
            var transitionTo = _flipView.TransitionTo(@from, to);
            Assert.IsNull(transitionTo);
            Assert.AreSame(_flipView.Items[from], _flipView.OtherItem);
            Assert.AreSame(_flipView.Items[to], _flipView.SelectedItem);
        }
    }
}
