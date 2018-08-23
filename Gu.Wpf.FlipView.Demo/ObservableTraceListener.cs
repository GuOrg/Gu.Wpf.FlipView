namespace Gu.Wpf.FlipView.Demo
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Text;

    public class ObservableTraceListener : System.Diagnostics.TraceListener
    {
        public static readonly ObservableTraceListener Instance = new ObservableTraceListener();

        private readonly StringBuilder StringBuilder = new StringBuilder();

        private ObservableTraceListener()
        {
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

        public ObservableCollection<string> Messages { get;  } = new ObservableCollection<string>();

        public static void Initialize()
        {
            // NOP ctor runs
        }

        public override void Write(string message)
        {
            this.StringBuilder.Append(message);
        }

        public override void WriteLine(string message)
        {
            this.StringBuilder.Append(message);
            this.Messages.Add(this.StringBuilder.ToString());
            this.StringBuilder.Clear();
        }
    }
}
