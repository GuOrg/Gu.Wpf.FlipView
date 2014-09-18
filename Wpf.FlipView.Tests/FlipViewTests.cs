namespace Wpf.FlipView.Tests
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
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
            _flipView = new FlipView();
            _currentItem = new ContentPresenter();
            _flipView.SetCurrentItem(_currentItem);
            _nextItem = new ContentPresenter();
            _flipView.SetNextItem(_nextItem);
        }

        [Test]
        public void TestNameTest()
        {
            _flipView.SelectedIndex = 1;
            _currentItem.RenderSize = new Size(100, 0);// set actual width
            _flipView.InternalHandleTouchMove(new Vector(-1, 0));
            Assert.AreEqual(-1, ((TranslateTransform)_currentItem.RenderTransform).X);
            Assert.AreEqual(99, ((TranslateTransform)_nextItem.RenderTransform).X);
            Assert.AreEqual(2, _flipView.NextIndex);
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
