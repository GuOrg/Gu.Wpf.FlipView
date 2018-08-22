namespace Gu.Wpf.FlipView.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class MainWindowTests
    {
        [Test]
        public void ClickAllTabs()
        {
            using (var listener = new WpfTraceListener())
            {
                // Just a smoke test so that we don't explode.
                using (var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe"))
                {
                    var window = app.MainWindow;
                    var tab = window.FindTabControl();
                    foreach (var tabItem in tab.Items)
                    {
                        _ = tabItem.Select();
                    }
                }

                CollectionAssert.IsEmpty(listener.Messages);
            }
        }
    }
}
