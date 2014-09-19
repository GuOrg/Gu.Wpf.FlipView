using System.Collections.Generic;
using System.Linq;

namespace Wpf.FlipView.Tests
{
    using System.Windows.Automation.Peers;

    using NUnit.Framework;
    /// <summary>
    /// http://miketwo.blogspot.se/2007/03/unit-testing-wpf-controls-with.html
    /// </summary>
    public class FlipViewUiTests
    {
        [Test, RequiresSTA, Explicit("Playing with ui automation")]
        public void TestNameTest()
        {
            var window = new MainWindow();
            var windowPeer = new WindowAutomationPeer(window);
            window.Show();
            List<AutomationPeer> automationPeers = windowPeer.GetChildren();
        }
    }
}
