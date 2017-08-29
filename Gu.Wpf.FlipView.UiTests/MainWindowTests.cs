namespace Gu.Wpf.FlipView.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class MainWindowTests
    {
        [Test]
        public void ClickAllTabs()
        {
            // Just a smoke test so that everything builds.
            using (var app = Application.Launch(Info.ProcessStartInfo))
            {
                var window = app.MainWindow();
                var tab = window.FindFirstDescendant(x => x.ByControlType(ControlType.Tab));
                foreach (var element in tab.FindAllChildren(x => x.ByControlType(ControlType.TabItem)))
                {
                    var tabItem = element.AsTabItem();
                    tabItem.Click();
                }
            }
        }
    }
}
