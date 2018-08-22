namespace Gu.Wpf.FlipView.UiTests
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class WpfTraceListener : TraceListener
    {
        private readonly List<string> messages = new List<string>();

        public WpfTraceListener()
        {
            PresentationTraceSources.Refresh();
            Register(PresentationTraceSources.AnimationSource);
            Register(PresentationTraceSources.DataBindingSource);
            Register(PresentationTraceSources.DocumentsSource);
            Register(PresentationTraceSources.DependencyPropertySource);
            Register(PresentationTraceSources.FreezableSource);
            Register(PresentationTraceSources.HwndHostSource);
            Register(PresentationTraceSources.MarkupSource);
            Register(PresentationTraceSources.NameScopeSource);
            Register(PresentationTraceSources.ResourceDictionarySource);
            Register(PresentationTraceSources.RoutedEventSource);
            Register(PresentationTraceSources.ShellSource);

            void Register(TraceSource source)
            {
                source.Listeners.Add(this);
                source.Switch.Level = SourceLevels.Warning | SourceLevels.Error;
            }
        }

        public IReadOnlyList<string> Messages => this.messages;

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
            this.messages.Add(message);
        }
    }
}
