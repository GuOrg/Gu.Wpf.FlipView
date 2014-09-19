//namespace Wpf.FlipView.Tests
//{
//    using System;
//    using System.Windows;
//    using System.Windows.Controls;
//    using System.Windows.Media.Animation;
//    using NUnit.Framework;
//    using WPF.FlipView;

//    [RequiresSTA]
//    public class FlipViewTests
//    {
//        private FlipView _flipView;
//        private ContentPresenter _currentItem;
//        private ContentPresenter _nextItem;

//        [SetUp]
//        public void SetUp()
//        {
//            _flipView = new FlipView { TransitionTime = 0 };

//            _currentItem = new ContentPresenter();
//            _flipView.SetCurrentItem(_currentItem);
//            _nextItem = new ContentPresenter();
//            _flipView.SetNextItem(_nextItem);
//            _flipView.Items.Add(new DummyItem(0));
//            _flipView.Items.Add(new DummyItem(1));
//            _flipView.Items.Add(new DummyItem(2));
//        }

//        [TestCase(1, 1, -99, 0)]
//        [TestCase(0, -1, 99, 1)]
//        public void OnTouchMoveInternal(int startIndex, double deltaX, double nextX, int nextIndex)
//        {
//            _currentItem.RenderSize = new Size(100, 0);// set actual width
//            _flipView.SelectedIndex = startIndex;
//            _flipView.OnPan(new Vector(deltaX, 0));
//            Assert.AreEqual(deltaX, _flipView.CurrentTransform.X);
//            Assert.AreEqual(nextX, _flipView.NextTransform.Transform(new Point(0, 0)).X);
//            Assert.AreEqual(_flipView.NextIndex, nextIndex);
//            Assert.AreSame(_flipView.Items[nextIndex], _flipView.NextItem);
//            Assert.AreSame(_flipView.Items[startIndex], _flipView.CurrentItem);
//        }

//        [Ignore("Subscribing to events on Root panel now")]
//        [TestCase(1, 1, -99, 0)]
//        [TestCase(0, -1, 99, 1)]
//        public void TouchMoveRegretRetry(int startIndex, double deltaX, double expectedX, int nextIndex)
//        {
//            _flipView.SelectedIndex = startIndex;
//            _flipView.CurrentItem = _flipView.Items[startIndex];
//            _currentItem.RenderSize = new Size(100, 0);// set actual width
//            int x = 50;
//            _flipView.FakeTouchDown(new Point(x, 0));
//            _flipView.FakeTouchMove(new Point(x + deltaX, 0));
//            Assert.AreEqual(deltaX, _flipView.CurrentTransform.X);
//            Assert.AreEqual(expectedX, _flipView.NextTransform.Transform(new Point(0, 0)).X);
//            Assert.AreEqual(nextIndex, _flipView.NextIndex);
//            Assert.AreSame(_flipView.Items[startIndex], _flipView.CurrentItem);
//            Assert.AreSame(_flipView.Items[nextIndex], _flipView.NextItem);
//            _flipView.FakeTouchUp(new Point(x + deltaX, 0));
//            Assert.AreEqual(0, _flipView.CurrentTransform.X);
//            Assert.AreSame(_flipView.Items[startIndex], _flipView.CurrentItem);
//            Assert.IsNull(_flipView.NextItem);

//            _flipView.FakeTouchDown(new Point(x, 0));
//            _flipView.FakeTouchMove(new Point(x + deltaX, 0));
//            Assert.AreEqual(deltaX, _flipView.CurrentTransform.X);
//            Assert.AreEqual(expectedX, _flipView.NextTransform.Transform(new Point(0, 0)).X);
//            Assert.AreEqual(nextIndex, _flipView.NextIndex);
//            Assert.AreSame(_flipView.Items[startIndex], _flipView.CurrentItem);
//            Assert.AreSame(_flipView.Items[nextIndex], _flipView.NextItem);
//        }

//        [Test]
//        public void OnPanEndedBeforeFirst()
//        {
//            _flipView.SelectedIndex = 0;
//            _currentItem.RenderSize = new Size(100, 0);// set actual width
//            _flipView.CurrentTransform.X = 50;
//            _flipView.OnPanEnded(new Vector(50,0),new Vector(5,0));
//            Assert.AreEqual(0,_flipView.CurrentTransform.X);
//        }

//        [TestCase(1, 2, -100)]
//        [TestCase(2, 1, 100)]
//        [TestCase(1, 0, 100)]
//        public void SelectNewItem(int fromIndex, int toIndex, double expectedX)
//        {
//            _currentItem.RenderSize = new Size(Math.Abs(expectedX), 0);// set actual width
//            _flipView.SelectedIndex = fromIndex; // First time does nothing cos OldValue == -1
//            _flipView.SelectedIndex = toIndex;
//            Assert.IsNull(_flipView.NextItem);
//            Assert.AreSame(_flipView.Items[toIndex], _flipView.CurrentItem);
//        }

//        [TestCase(0, 1, 100)]
//        [TestCase(1, 0, -100)]
//        public void TransitionTo(int from, int to, double nextX)
//        {
//            _currentItem.RenderSize = new Size(Math.Abs(nextX), 0);// set actual width
//            _flipView.SelectedIndex = from;
//            Transition? transitionTo = _flipView.TransitionTo(@from, to);
//            Assert.AreEqual(from, transitionTo.Value.From);
//            Assert.AreEqual(to, transitionTo.Value.To);
//            Assert.AreSame(_flipView.Items[from], _flipView.CurrentItem);
//            Assert.AreSame(_flipView.Items[to], _flipView.NextItem);
//            Assert.AreEqual(nextX, _flipView.NextOffsetTransform.Transform(new Point(0, 0)).X);
//        }

//        [Test]
//        public void CreateCurrentTransformSlideAnimation()
//        {
//            _currentItem.RenderSize = new Size(100, 0);// set actual width
//            _flipView.TransitionTime = 100;
//            var animation = (DoubleAnimationUsingKeyFrames)this._flipView.CreateCurrentTransformSlideAnimation(new Transition(0, 1));
//            Assert.AreEqual(0, animation.KeyFrames[0].Value);
//            Assert.AreEqual(-100, animation.KeyFrames[1].Value);
//        }

//        [TestCase(0, 1)]
//        [TestCase(1, 0)]
//        public void TransitionToWhenAlreadyAnimating(int from, int to)
//        {
//            _flipView.SetIsAnimating(true);
//            _currentItem.RenderSize = new Size(Math.Abs(100), 0);// set actual width
//            _flipView.SelectedIndex = from;
//            Transition? transitionTo = _flipView.TransitionTo(@from, to);
//            Assert.IsNull(transitionTo);
//            Assert.AreSame(_flipView.Items[from], _flipView.CurrentItem);
//            Assert.AreSame(_flipView.Items[to], _flipView.NextItem);
//        }

//        [TestCase(0, false, true)]
//        [TestCase(1, true, true)]
//        [TestCase(2, true, false)]
//        public void CanExecutePreviousAndNext(int index, bool canPrevious, bool canNext)
//        {
//            _flipView.SelectedIndex = index;
//            Assert.AreEqual(canPrevious, FlipView.PreviousCommand.CanExecute(null, _flipView));
//            Assert.AreEqual(canNext, FlipView.NextCommand.CanExecute(null, _flipView));
//        }
//    }
//}
