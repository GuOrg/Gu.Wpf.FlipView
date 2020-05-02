namespace Gu.Wpf.FlipView.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class FlipViewBoundWindowTests
    {
        [Test]
        public void BrowseBackAndForward()
        {
            using var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", "FlipViewBoundWindow");
            var window = app.MainWindow;
            window.WaitUntilResponsive();
            var flipView = window.FindFirstDescendant(Conditions.ByAutomationId("FlipView"));
            var browseBack = flipView.FindButton("BrowseBackButton");
            var browseForward = flipView.FindButton("BrowseForwardButton");
            var dummyButton = window.FindButton("DummyButton");
            var status = window.FindTextBlock("Status");
            dummyButton.Click();
            window.WaitUntilResponsive();
            Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);
            Assert.AreEqual(false, browseBack.IsEnabled);
            Assert.AreEqual(true, browseForward.IsEnabled);

            browseForward.Click();
            window.WaitUntilResponsive();
            Assert.AreEqual(true, browseBack.IsEnabled);
            Assert.AreEqual(true, browseForward.IsEnabled);
            Assert.AreEqual("SelectedIndex: 1 SelectedItem: Erik Svensson", status.Text);

            browseBack.Click();
            window.WaitUntilResponsive();
            Assert.AreEqual(false, browseBack.IsEnabled);
            Assert.AreEqual(true, browseForward.IsEnabled);
            Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);

            browseForward.Click();
            window.WaitUntilResponsive();
            Assert.AreEqual(true, browseBack.IsEnabled);
            Assert.AreEqual(true, browseForward.IsEnabled);
            Assert.AreEqual("SelectedIndex: 1 SelectedItem: Erik Svensson", status.Text);

            browseForward.Click();
            window.WaitUntilResponsive();
            Assert.AreEqual(true, browseBack.IsEnabled);
            Assert.AreEqual(true, browseForward.IsEnabled);
            Assert.AreEqual("SelectedIndex: 2 SelectedItem: Reed Forkmann", status.Text);

            browseForward.Click();
            Wait.UntilInputIsProcessed();
            Assert.AreEqual(true, browseBack.IsEnabled);
            Assert.AreEqual(false, browseForward.IsEnabled);
            Assert.AreEqual("SelectedIndex: 3 SelectedItem: Cat Incremented", status.Text);

            browseBack.Click();
            window.WaitUntilResponsive();
            Assert.AreEqual(true, browseBack.IsEnabled);
            Assert.AreEqual(true, browseForward.IsEnabled);
            Assert.AreEqual("SelectedIndex: 2 SelectedItem: Reed Forkmann", status.Text);
        }

        [Test]
        public void WhenSourceIsCleared()
        {
            using var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", "FlipViewBoundWindow");
            var window = app.MainWindow;
            var flipView = window.FindFirstDescendant(Conditions.ByAutomationId("FlipView"));
            var browseBack = flipView.FindButton("BrowseBackButton");
            var browseForward = flipView.FindButton("BrowseForwardButton");
            var status = window.FindTextBlock("Status");
            var clear = window.FindButton("Clear");
            var reset = window.FindButton("Reset");
            Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);

            clear.Click();
            Assert.AreEqual("SelectedIndex: -1 SelectedItem: ", status.Text);
            Assert.AreEqual(false, browseBack.IsEnabled);
            Assert.AreEqual(false, browseForward.IsEnabled);

            reset.Click();
            Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);
            Assert.AreEqual(false, browseBack.IsEnabled);
            Assert.AreEqual(true, browseForward.IsEnabled);
        }

        [Test]
        public void WhenSourceIsSetToEmpty()
        {
            using var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", "FlipViewBoundWindow");
            var window = app.MainWindow;
            var flipView = window.FindFirstDescendant(Conditions.ByAutomationId("FlipView"));
            var browseBack = flipView.FindButton("BrowseBackButton");
            var browseForward = flipView.FindButton("BrowseForwardButton");
            var status = window.FindTextBlock("Status");
            var setEmpty = window.FindButton("Set empty");
            var reset = window.FindButton("Reset");
            Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);

            setEmpty.Click();
            Assert.AreEqual("SelectedIndex: -1 SelectedItem: ", status.Text);
            Assert.AreEqual(false, browseBack.IsEnabled);
            Assert.AreEqual(false, browseForward.IsEnabled);

            reset.Click();
            Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);
            Assert.AreEqual(false, browseBack.IsEnabled);
            Assert.AreEqual(true, browseForward.IsEnabled);
        }

        [Test]
        public void WhenSourceIsSetToNull()
        {
            using var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", "FlipViewBoundWindow");
            var window = app.MainWindow;
            var flipView = window.FindFirstDescendant(Conditions.ByAutomationId("FlipView"));
            var browseBack = flipView.FindButton("BrowseBackButton");
            var browseForward = flipView.FindButton("BrowseForwardButton");
            var status = window.FindTextBlock("Status");
            var setEmpty = window.FindButton("Set null");
            var reset = window.FindButton("Reset");
            Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);

            setEmpty.Click();
            Assert.AreEqual("SelectedIndex: -1 SelectedItem: ", status.Text);
            Assert.AreEqual(false, browseBack.IsEnabled);
            Assert.AreEqual(false, browseForward.IsEnabled);

            reset.Click();
            Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);
            Assert.AreEqual(false, browseBack.IsEnabled);
            Assert.AreEqual(true, browseForward.IsEnabled);
        }
    }
}
