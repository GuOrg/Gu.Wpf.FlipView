namespace Wpf.FlipView.Tests
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Moq;
    using NUnit.Framework;
    using WPF.FlipView;

    [RequiresSTA]
    public class FlipViewTests
    {
        private FlipView _flipView;
        private ContentPresenter _currentItem;
        private ContentPresenter _nextItem;

        [SetUp]
        public void SetUp()
        {
            _flipView = new FlipView { TransitionTime = 0 };

            _currentItem = new ContentPresenter();
            _flipView.SetCurrentItem(_currentItem);
            _nextItem = new ContentPresenter();
            _flipView.SetNextItem(_nextItem);
            _flipView.Items.Add(new DummyItem(0));
            _flipView.Items.Add(new DummyItem(1));
            _flipView.Items.Add(new DummyItem(2));
        }

        [TestCase(1, 1, -99, 0)]
        [TestCase(0, -1, 99, 1)]
        public void TouchMoveRegretRetry(int startIndex, double deltaX, double expectedX, int nextIndex)
        {
            _flipView.SelectedIndex = startIndex;
            _flipView.CurrentItem = _flipView.Items[startIndex];
            _currentItem.RenderSize = new Size(100, 0);// set actual width
            int x = 50;
            _flipView.FakeTouchDown(new Point(x, 0));
            _flipView.FakeTouchMove(new Point(x + deltaX, 0));
            Assert.AreEqual(deltaX, _flipView.CurrentTransform.X);
            Assert.AreEqual(expectedX, _flipView.NextTransform.Transform(new Point(0, 0)).X);
            Assert.AreEqual(nextIndex, _flipView.NextIndex);
            Assert.AreSame(_flipView.Items[startIndex], _flipView.CurrentItem);
            Assert.AreSame(_flipView.Items[nextIndex], _flipView.NextItem);
            _flipView.FakeTouchUp(new Point(x + deltaX, 0));
            Assert.AreEqual(0, _flipView.CurrentTransform.X);
            Assert.AreSame(_flipView.Items[startIndex], _flipView.CurrentItem);
            Assert.IsNull( _flipView.NextItem);

            _flipView.FakeTouchDown(new Point(x, 0));
            _flipView.FakeTouchMove(new Point(x + deltaX, 0));
            Assert.AreEqual(deltaX, _flipView.CurrentTransform.X);
            Assert.AreEqual(expectedX, _flipView.NextTransform.Transform(new Point(0, 0)).X);
            Assert.AreEqual(nextIndex, _flipView.NextIndex);
            Assert.AreSame(_flipView.Items[startIndex], _flipView.CurrentItem);
            Assert.AreSame(_flipView.Items[nextIndex], _flipView.NextItem);
        }

        [TestCase(1, 2, -100)]
        [TestCase(2, 1, 100)]
        [TestCase(1, 0, 100)]
        public void RunSlideAnimation(int fromIndex, int toIndex, double expectedX)
        {
            _currentItem.RenderSize = new Size(Math.Abs(expectedX), 0);// set actual width
            _flipView.SelectedIndex = fromIndex; // First time does nothing cos OldValue == -1
            _flipView.SelectedIndex = toIndex;
            //_flipView.TransitionTo(fromIndex, toIndex);
            Assert.AreEqual(expectedX, _flipView.CurrentTransform.X);
            Assert.AreEqual(0, _flipView.NextTransform.Transform(new Point(0, 0)).X);
            //Assert.AreSame(_flipView.Items[toIndex], _flipView.NextItem);
            //Assert.AreSame(_flipView.Items[fromIndex], _flipView.CurrentItem);
            //_flipView.OnAnimationCompleted(null, null);
            Assert.IsNull(_flipView.NextItem);
            Assert.AreSame(_flipView.Items[toIndex], _flipView.CurrentItem);
        }
    }
}
