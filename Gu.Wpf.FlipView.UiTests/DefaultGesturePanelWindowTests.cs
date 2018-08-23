namespace Gu.Wpf.FlipView.UiTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    [Explicit("Not sure how we want this, default no mouse swipe is perhaps the most sane but that is a breaking change.")]
    public class DefaultGesturePanelWindowTests
    {
        private const string WindowName = "DefaultGesturePanelWindow";

        [Test]
        public void Swipes()
        {
            using (var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", WindowName))
            {
                var window = app.MainWindow;
                var gesturePanel = window.FindGroupBox("Gesture panel");
                var listBox = window.FindListBox("Gestures");
                var swipes = new List<string>();
                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.DragHorizontally(MouseButton.Left, gesturePanel.Bounds.Center(), 40);
                window.WaitUntilResponsive();
                swipes.Add("SwipeRight Delta: (40, 0) Velocity: ∞");
                CollectionAssert.AreEqual(swipes, listBox.Items.Select(x => x.Text));

                Mouse.DragHorizontally(MouseButton.Left, gesturePanel.Bounds.Center(), -40);
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
                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.DragHorizontally(MouseButton.Left, gesturePanel.Bounds.Center(), 39);
                window.WaitUntilResponsive();
                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.DragHorizontally(MouseButton.Left, gesturePanel.Bounds.Center(), -39);
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
                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.Position = gesturePanel.Bounds.Center();
                Mouse.Down(MouseButton.Left);
                Wait.For(TimeSpan.FromMilliseconds(200));
                Mouse.Position += new Vector(40, 0);
                Mouse.Up(MouseButton.Left);
                window.WaitUntilResponsive();
                CollectionAssert.IsEmpty(listBox.Items);

                Mouse.Position = gesturePanel.Bounds.Center();
                Mouse.Down(MouseButton.Left);
                Wait.For(TimeSpan.FromMilliseconds(200));
                Mouse.Position += new Vector(-40, 0);
                Mouse.Up(MouseButton.Left);
                window.WaitUntilResponsive();
                CollectionAssert.IsEmpty(listBox.Items);
            }
        }
    }
}
