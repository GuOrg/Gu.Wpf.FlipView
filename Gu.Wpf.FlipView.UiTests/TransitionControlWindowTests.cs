namespace Gu.Wpf.FlipView.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class TransitionControlWindowTests
    {
        [Test]
        public void Transitions()
        {
            using (var listener = new WpfTraceListener())
            {
                // Just testing that we don't crash here.
                using (var app = Application.Launch("Gu.Wpf.FlipView.Demo.exe", "TransitionControlWindow"))
                {
                    var window = app.MainWindow;
                    var selectedIndex = window.FindSlider("SelectedIndex");
                    selectedIndex.Value = 0;
                    selectedIndex.Value = 1;
                    selectedIndex.Value = 2;
                }

                CollectionAssert.IsEmpty(listener.Messages);
            }
        }
    }
}
