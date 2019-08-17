namespace Gu.Wpf.FlipView.Demo.Misc
{
    using System.Windows.Input;
    using Gu.Wpf.FlipView.Gestures;

    /// <summary>
    /// Interaction logic for TouchBox.xaml.
    /// </summary>
    public partial class TouchBox : EventBox
    {
        private readonly TouchGestureTracker tracker;

        private bool gestureStarted;

        public TouchBox()
        {
            this.InitializeComponent();
            this.tracker = new TouchGestureTracker { InputElement = this.InputElement };
            this.tracker.Gestured += (_, g) => this.Args.Add(new ArgsVm(g));
        }

        protected override void OnStarted(object sender, InputEventArgs e)
        {
            this.gestureStarted = true;
            base.OnStarted(sender, e);
        }

        protected override void OnInput(object sender, InputEventArgs e)
        {
            if (this.gestureStarted)
            {
                base.OnInput(sender, e);
            }
        }

        protected override void OnEnded(object sender, InputEventArgs e)
        {
            this.gestureStarted = false;
            base.OnEnded(sender, e);
        }
    }
}
