namespace Wpf.FlipView.Tests
{
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;

    using NUnit.Framework;
    using WPF.FlipView;

    [RequiresSTA]
    public class FlipViewTests
    {
        private FlipView _flipView;
        private ContentPresenter _currentItem;
        private ContentPresenter _nextItem;

        private object _item1;
        private object _item2;
        private object _item3;

        [SetUp]
        public void SetUp()
        {
            _flipView = new FlipView();
            _currentItem = new ContentPresenter();
            _flipView.SetCurrentItem(_currentItem);
            _nextItem = new ContentPresenter();
            _flipView.SetNextItem(_nextItem);
            _item1 = new object();
            _item2 = new object();
            _item3 = new object();
            _flipView.Items.Add(_item1);
            _flipView.Items.Add(_item2);
            _flipView.Items.Add(_item2);
        }

        [Test]
        public void TouchMoveNegative()
        {
            _flipView.SelectedIndex = 1;
            _currentItem.RenderSize = new Size(100, 0);// set actual width
            _flipView.InternalHandleTouchMove(new Vector(-1, 0));
            Assert.AreEqual(-1, _flipView.CurrentTransform.X);
            Assert.AreEqual(99, _flipView.NextTransform.Transform(new Point(0,0)).X);
            Assert.AreEqual(2, _flipView.NextIndex);
        }

        [Test]
        public void TouchMovePositive()
        {
            _flipView.SelectedIndex = 1;
            _currentItem.RenderSize = new Size(100, 0);// set actual width
            _flipView.InternalHandleTouchMove(new Vector(1, 0));
            Assert.AreEqual(1, _flipView.CurrentTransform.X);
            Assert.AreEqual(-99, _flipView.NextTransform.Transform(new Point(0,0)).X);
            Assert.AreEqual(0, _flipView.NextIndex);
        }

        [Test]
        public void RunSlideAnimation()
        {
            DispatcherUtil.DoEvents();
            _flipView.SelectedIndex = 1;
            _currentItem.RenderSize = new Size(100, 0);// set actual width
            _flipView.RunSlideAnimation(new Transition(0, 1));
            DispatcherUtil.DoEvents();
            Assert.AreEqual(-1, _flipView.CurrentTransform.X);
            Assert.AreEqual(99, _flipView.NextTransform.Transform(new Point(0,0)).X);
        }
    }

    public static class FlipViewTestExt
    {
        public static FieldInfo PartCurrentItem;
        public static FieldInfo PartNextItem;

        static FlipViewTestExt()
        {
            const string PART_CurrentItem = "PART_CurrentItem";
            const string PART_NextItem = "PART_NextItem";
            PartCurrentItem = typeof(FlipView).GetField(PART_CurrentItem, BindingFlags.Instance | BindingFlags.NonPublic);
            PartNextItem = typeof(FlipView).GetField(PART_NextItem, BindingFlags.Instance | BindingFlags.NonPublic);
        }
        public static void SetCurrentItem(this FlipView flipView, ContentPresenter contentPresenter)
        {
            PartCurrentItem.SetValue(flipView, contentPresenter);
        }

        public static void SetNextItem(this FlipView flipView, ContentPresenter contentPresenter)
        {
            PartNextItem.SetValue(flipView, contentPresenter);
        }
    }
}
