namespace Gu.Wpf.FlipView.UiTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class MouseGesturePanelWindowTests
    {
        [Test]
        public void Swipes()
        {
            using (var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", "MouseGesturePanelWindow"))
            {
                var window = app.MainWindow;
                var gesturePanel = window.FindGroupBox("Gesture panel");
                var listBox = window.FindListBox("Gestures");
                var swipes = new List<string>();
                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.DragHorizontally(MouseButton.Left, gesturePanel.Bounds.Center(), 15);
                swipes.Add("SwipeRight Delta: (15, 0) Velocity: ∞");
                CollectionAssert.AreEqual(swipes, listBox.Items.Select(x => x.Text));

                Mouse.DragHorizontally(MouseButton.Left, gesturePanel.Bounds.Center(), -15);
                swipes.Add("SwipeLeft Delta: (-15, 0) Velocity: ∞");
                CollectionAssert.AreEqual(swipes, listBox.Items.Select(x => x.Text));
            }
        }
    }
}
