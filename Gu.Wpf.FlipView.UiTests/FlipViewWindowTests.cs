namespace Gu.Wpf.FlipView.UiTests
{
    using FlaUI.Core;
    using FlaUI.UIA3;
    using NUnit.Framework;

    public class FlipViewWindowTests
    {
        [Test]
        public void BrowseBackAndForward()
        {
            using (var app = Application.Launch(Info.CreateStartInfo("FlipViewWindow")))
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    app.WaitWhileBusy();
                    window.WaitUntilResponsive();

                    var flipView = window.FindFirstDescendant(x => x.ByAutomationId("FlipView"));
                    var browseBack = flipView.FindButton("BrowseBackButton");
                    var browseForward = flipView.FindButton("BrowseForwardButton");
                    var selectedIndex = window.FindSlider("SelectedIndex");
                    Assert.AreEqual(0, selectedIndex.Value);
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    browseForward.Click();

                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual(1, selectedIndex.Value);

                    browseBack.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual(0, selectedIndex.Value);

                    browseForward.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual(1, selectedIndex.Value);

                    browseForward.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual(2, selectedIndex.Value);

                    browseForward.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(false, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual(3, selectedIndex.Value);

                    browseBack.Click();
                    window.WaitUntilResponsive();
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual(2, selectedIndex.Value);
                }
            }
        }

        [Test]
        [Explicit("For some reason this only works in debug")]
        public void ChangeSelectedIndex()
        {
            using (var app = Application.Launch(Info.CreateStartInfo("FlipViewWindow")))
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    var flipView = window.FindFirstDescendant(x => x.ByAutomationId("FlipView"));
                    var browseBack = flipView.FindButton("BrowseBackButton");
                    var browseForward = flipView.FindButton("BrowseForwardButton");
                    var selectedIndex = window.FindSlider("SelectedIndex");
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual(1, selectedIndex.Value);

                    selectedIndex.Value = 0;
                    window.WaitUntilResponsive();
                    Assert.AreEqual(false, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(true, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual(0, selectedIndex.Value);

                    selectedIndex.Value = 3;
                    window.WaitUntilResponsive();
                    Assert.AreEqual(true, browseBack.Properties.IsEnabled.Value);
                    Assert.AreEqual(false, browseForward.Properties.IsEnabled.Value);
                    Assert.AreEqual(3, selectedIndex.Value);
                }
            }
        }
    }
}