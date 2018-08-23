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
                Mouse.MoveTo(gesturePanel.Bounds.Center());
                var swipes = new List<string>();
                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.Down(MouseButton.Left);
                Mouse.MoveBy(100, 0);
                Mouse.Up(MouseButton.Left);
                swipes.Add("SwipeRight Delta: (100, 0) Velocity: 0,3");
                CollectionAssert.AreEqual(swipes, listBox.Items.Select(x => x.Text));

                Mouse.Down(MouseButton.Left);
                Mouse.MoveBy(-100, 0);
                Mouse.Up(MouseButton.Left);
                swipes.Add("SwipeLeft Delta: (-100, 0) Velocity: 0,3");
                CollectionAssert.AreEqual(swipes, listBox.Items.Select(x => x.Text));
            }
        }
    }
}
