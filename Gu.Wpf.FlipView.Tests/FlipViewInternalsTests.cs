namespace Gu.Wpf.FlipView.Tests
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;
    using Gu.Wpf.FlipView;
    using Gu.Wpf.FlipView.Tests.MocksAndHelpers;

    using NUnit.Framework;

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
            this._flipView.SetActualWidth(Math.Abs(expectedX));// set actual width
            this._flipView.SelectedIndex = fromIndex; // First time does nothing cos OldValue == -1
            this._flipView.SelectedIndex = toIndex;
            Assert.IsNull(this._flipView.OtherItem);
            Assert.AreSame(this._flipView.Items[toIndex], this._flipView.SelectedItem);
        }

        [TestCase(0, 1, 100)]
        [TestCase(1, 0, -100)]
        public void TransitionToBeforeAnimation(int from, int to, double nextX)
        {
            this._flipView.SetActualWidth(Math.Abs(nextX));
            this._flipView.SelectedIndex = from;
            this._flipView.TransitionTo(from, to);
            Assert.AreSame(this._flipView.Items[from], this._flipView.OtherItem);
            Assert.AreSame(this._flipView.Items[to], this._flipView.SelectedItem);
            Assert.AreEqual(nextX, this._flipView.SelectedItemTransform.X);
            Assert.AreEqual(0, this._flipView.OtherItemTransform.Transform(new Point(0, 0)).X);
        }

        [TestCase(0, 1, -100)]
        [TestCase(1, 0, 100)]
        public void TransitionToAfterAnimation(int from, int to, double nextX)
        {
            this._flipView.SetActualWidth(Math.Abs(nextX));
            this._flipView.SelectedIndex = from;
            this._flipView.TransitionTime = 0;
            var animation = this._flipView.TransitionTo(@from, to);
            this._flipView.AnimateTransition(animation);
            Assert.IsNull(this._flipView.OtherItem);
            Assert.AreSame(this._flipView.Items[to], this._flipView.SelectedItem);
            Assert.AreEqual(0, this._flipView.SelectedItemTransform.X);
            Assert.AreEqual(nextX, this._flipView.OtherItemTransform.Transform(new Point(0, 0)).X);
        }

        [TestCase(0, 1, 100)]
        [TestCase(1, 0, -100)]
        public void CreateCurrentTransformSlideAnimation(int from, int to, double expectedX)
        {
            this._flipView.SetActualWidth(Math.Abs(expectedX));
            this._flipView.TransitionTime = 100;
            var animation = (DoubleAnimationUsingKeyFrames)this._flipView.CreateTransitionAnimation(new Transition(from, to));
            Assert.AreEqual(0, this._flipView.OtherItemTransform.Transform(new Point(0, 0)).X);

            Assert.AreEqual(expectedX, animation.KeyFrames[0].Value);
            Assert.AreEqual(0, animation.KeyFrames[1].Value);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void TransitionToWhenAlreadyAnimating(int from, int to)
        {
            this._flipView.SetIsAnimating(true);
            this._flipView.SetActualWidth(100);
            this._flipView.SelectedIndex = from;
            var transitionTo = this._flipView.TransitionTo(@from, to);
            Assert.IsNull(transitionTo);
            Assert.AreSame(this._flipView.Items[from], this._flipView.OtherItem);
            Assert.AreSame(this._flipView.Items[to], this._flipView.SelectedItem);
        }
    }
}
