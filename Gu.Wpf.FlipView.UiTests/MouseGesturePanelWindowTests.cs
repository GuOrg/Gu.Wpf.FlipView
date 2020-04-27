namespace Gu.Wpf.FlipView.UiTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class MouseGesturePanelWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.FlipView.Demo.exe";
        private const string WindowName = "MouseGesturePanelWindow";

        [SetUp]
        public void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            Wait.UntilInputIsProcessed();
            app.MainWindow.FindButton("Clear").Click();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Close the shared window after the last test.
            Application.KillLaunched(ExeFileName);
        }

        [Test]
        public void Swipes()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var gesturePanel = window.FindGroupBox("Gesture panel");
            var listBox = window.FindListBox("Gestures");
            var swipes = new List<string>();
            var cp = gesturePanel.Bounds.Center();

            CollectionAssert.IsEmpty(listBox.Items);

            Mouse.Drag(MouseButton.Left, cp, cp + new System.Windows.Vector(40, 0), TimeSpan.Zero);
            window.WaitUntilResponsive();
            swipes.Add("SwipeRight Delta: (40, 0) Velocity: ∞");
            CollectionAssert.AreEqual(swipes, listBox.Items.Select(x => x.Text));

            Mouse.Drag(MouseButton.Left, cp, cp + new System.Windows.Vector(-40, 0), TimeSpan.Zero);
            window.WaitUntilResponsive();
            swipes.Add("SwipeLeft Delta: (-40, 0) Velocity: ∞");
            CollectionAssert.AreEqual(swipes, listBox.Items.Select(x => x.Text));
        }

        [Test]
        public void NoSwipesWhenTooShort()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var gesturePanel = window.FindGroupBox("Gesture panel");
            var listBox = window.FindListBox("Gestures");
            var cp = gesturePanel.Bounds.Center();

            CollectionAssert.IsEmpty(listBox.Items);

            Mouse.Drag(MouseButton.Left, cp, cp + new System.Windows.Vector(39, 0), double.PositiveInfinity);
            window.WaitUntilResponsive();
            CollectionAssert.IsEmpty(listBox.Items);

            Mouse.Drag(MouseButton.Left, cp, cp + new System.Windows.Vector(-39, 0), double.PositiveInfinity);
            window.WaitUntilResponsive();
            CollectionAssert.IsEmpty(listBox.Items);
        }

        [Test]
        public void NoSwipesWhenTooSlow()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var gesturePanel = window.FindGroupBox("Gesture panel");
            var listBox = window.FindListBox("Gestures");
            var cp = gesturePanel.Bounds.Center();

            CollectionAssert.IsEmpty(listBox.Items);

            Mouse.Drag(MouseButton.Left, cp, cp + new System.Windows.Vector(39, 0), 200);
            window.WaitUntilResponsive();
            CollectionAssert.IsEmpty(listBox.Items);

            Mouse.Drag(MouseButton.Left, cp, cp + new System.Windows.Vector(-39, 0), 200);
            window.WaitUntilResponsive();
            CollectionAssert.IsEmpty(listBox.Items);
        }
    }
}
