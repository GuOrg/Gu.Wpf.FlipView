namespace Gu.Wpf.FlipView.UiTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class MouseGesturePanelWindowTests
    {
        private const string WindowName = "MouseGesturePanelWindow";

        [Test]
        public void Swipes()
        {
            using (var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", WindowName))
            {
                var window = app.MainWindow;
                var gesturePanel = window.FindGroupBox("Gesture panel");
                var listBox = window.FindListBox("Gestures");
                var swipes = new List<string>();
                var cp = gesturePanel.Bounds.Center();

                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.Drag(MouseButton.Left, cp, cp + new Vector(40, 0), TimeSpan.Zero);
                window.WaitUntilResponsive();
                swipes.Add("SwipeRight Delta: (40, 0) Velocity: ∞");
                CollectionAssert.AreEqual(swipes, listBox.Items.Select(x => x.Text));

                Mouse.Drag(MouseButton.Left, cp, cp + new Vector(-40, 0), TimeSpan.Zero);
                window.WaitUntilResponsive();
                swipes.Add("SwipeLeft Delta: (-40, 0) Velocity: ∞");
                CollectionAssert.AreEqual(swipes, listBox.Items.Select(x => x.Text));
            }
        }

        [Test]
        public void NoSwipesWhenTooShort()
        {
            using (var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", WindowName))
            {
                var window = app.MainWindow;
                var gesturePanel = window.FindGroupBox("Gesture panel");
                var listBox = window.FindListBox("Gestures");
                var cp = gesturePanel.Bounds.Center();

                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.Drag(MouseButton.Left, cp, cp + new Vector(39, 0), double.PositiveInfinity);
                window.WaitUntilResponsive();
                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.Drag(MouseButton.Left, cp, cp + new Vector(-39, 0), double.PositiveInfinity);
                window.WaitUntilResponsive();
                CollectionAssert.IsEmpty(listBox.Items);
            }
        }

        [Test]
        public void NoSwipesWhenTooSlow()
        {
            using (var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", WindowName))
            {
                var window = app.MainWindow;
                var gesturePanel = window.FindGroupBox("Gesture panel");
                var listBox = window.FindListBox("Gestures");
                var cp = gesturePanel.Bounds.Center();

                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.Drag(MouseButton.Left, cp, cp + new Vector(39, 0), 200);
                window.WaitUntilResponsive();
                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.Drag(MouseButton.Left, cp, cp + new Vector(-39, 0), 200);
                window.WaitUntilResponsive();
                CollectionAssert.IsEmpty(listBox.Items);
            }
        }
    }
}
