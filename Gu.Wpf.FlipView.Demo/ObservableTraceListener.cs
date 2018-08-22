namespace Gu.Wpf.FlipView.Demo
{
    using System.Collections.ObjectModel;

    public class ObservableTraceListener : System.Diagnostics.TraceListener
    {
        public static readonly ObservableTraceListener Instance = new ObservableTraceListener();

        private ObservableTraceListener()
        {
        }

        public ObservableCollection<string> Messages { get;  } = new ObservableCollection<string>();

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
            this.Messages.Add(message);
        }
    }
}