namespace Gu.Wpf.FlipView.UiTests
{
    using FlaUI.Core;
    using FlaUI.UIA3;
    using NUnit.Framework;

    public class TransitionControlWindowTests
    {
        [Test]
        public void Transitions()
        {
            // Just testing that we don't crash here.
            using (var app = Application.Launch(Info.CreateStartInfo("TransitionControlWindow")))
            {
                using (var automation = new UIA3Automation())
                {
                    var window = app.GetMainWindow(automation);
                    var selectedIndex = window.FindSlider("SelectedIndex");
                    selectedIndex.Value = 0;
                    selectedIndex.Value = 1;
                    selectedIndex.Value = 2;
                }
            }
        }
    }
}