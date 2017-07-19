namespace Gu.Wpf.FlipView.UiTests
{
    using FlaUI.Core;
    using FlaUI.UIA3;
    using NUnit.Framework;

    public class FlipViewBoundWindowTests
    {
        [Test]
        public void BrowseBackAndForward()
        {
            using (var app = Application.Launch(Info.CreateStartInfo("FlipViewBoundWindow")))
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    var flipView = window.FindFirstDescendant(x => x.ByAutomationId("FlipView"));
                    var browseBack = flipView.FindButton("BrowseBackButton");
                    var browseForward = flipView.FindButton("BrowseForwardButton");
                    var status = window.FindTextBlock("Status");
                    Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    browseForward.Click();

                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual("SelectedIndex: 1 SelectedItem: Erik Svensson", status.Text);

                    browseBack.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);

                    browseForward.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual("SelectedIndex: 1 SelectedItem: Erik Svensson", status.Text);

                    browseForward.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual("SelectedIndex: 2 SelectedItem: Reed Forkmann", status.Text);

                    browseForward.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(false, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual("SelectedIndex: 3 SelectedItem: Cat Incremented", status.Text);

                    browseBack.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual("SelectedIndex: 2 SelectedItem: Reed Forkmann", status.Text);
                }
            }
        }

        [Test]
        public void WhenSourceIsCleared()
        {
            using (var app = Application.Launch(Info.CreateStartInfo("FlipViewBoundWindow")))
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    var flipView = window.FindFirstDescendant(x => x.ByAutomationId("FlipView"));
                    var browseBack = flipView.FindButton("BrowseBackButton");
                    var browseForward = flipView.FindButton("BrowseForwardButton");
                    var status = window.FindTextBlock("Status");
                    var clear = window.FindButton("Clear");
                    var reset = window.FindButton("Reset");
                    Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);

                    clear.Click();
                    Assert.AreEqual("SelectedIndex: -1 SelectedItem: ", status.Text);
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(false, browseForward.Properties.IsEnabled.Value);

                    reset.Click();
                    Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                }
            }
        }

        [Test]
        public void WhenSourceIsSetToEmpty()
        {
            using (var app = Application.Launch(Info.CreateStartInfo("FlipViewBoundWindow")))
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    var flipView = window.FindFirstDescendant(x => x.ByAutomationId("FlipView"));
                    var browseBack = flipView.FindButton("BrowseBackButton");
                    var browseForward = flipView.FindButton("BrowseForwardButton");
                    var status = window.FindTextBlock("Status");
                    var setEmpty = window.FindButton("Set empty");
                    var reset = window.FindButton("Reset");
                    Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);

                    setEmpty.Click();
                    Assert.AreEqual("SelectedIndex: -1 SelectedItem: ", status.Text);
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(false, browseForward.Properties.IsEnabled.Value);

                    reset.Click();
                    Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                }
            }
        }

        [Test]
        public void WhenSourceIsSetToNull()
        {
            using (var app = Application.Launch(Info.CreateStartInfo("FlipViewBoundWindow")))
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    var flipView = window.FindFirstDescendant(x => x.ByAutomationId("FlipView"));
                    var browseBack = flipView.FindButton("BrowseBackButton");
                    var browseForward = flipView.FindButton("BrowseForwardButton");
                    var status = window.FindTextBlock("Status");
                    var setEmpty = window.FindButton("Set null");
                    var reset = window.FindButton("Reset");
                    Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);

                    setEmpty.Click();
                    Assert.AreEqual("SelectedIndex: -1 SelectedItem: ", status.Text);
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(false, browseForward.Properties.IsEnabled.Value);

                    reset.Click();
                    Assert.AreEqual("SelectedIndex: 0 SelectedItem: Johan Larsson", status.Text);
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                }
            }
        }
    }
}