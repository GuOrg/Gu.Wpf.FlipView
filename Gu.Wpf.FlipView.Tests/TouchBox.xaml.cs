namespace Gu.Wpf.FlipView.Tests
{
    using System.Windows.Input;

    using Gu.Wpf.FlipView.Gestures;
    using Gu.Wpf.FlipView.Tests.MocksAndHelpers;

    /// <summary>
    /// Interaction logic for TouchBox.xaml
    /// </summary>
    public partial class TouchBox : EventBox
    {
        private bool _gestureStarted;
        private TouchGestureTracker _tracker;
        public TouchBox()
        {
            InitializeComponent();
            this._tracker = new TouchGestureTracker { InputElement = this.InputElement };
            this._tracker.Gestured += (_, g) => this.Args.Add(new ArgsVm(g));
        }

        protected override void OnStarted(object sender, InputEventArgs e)
        {
            this._gestureStarted = true;
            base.OnStarted(sender, e);
        }

        protected override void OnInput(object sender, InputEventArgs e)
        {
            if (this._gestureStarted)
            {
                base.OnInput(sender, e);
            }
        }

        protected override void OnEnded(object sender, InputEventArgs e)
        {
            this._gestureStarted = false;
            base.OnEnded(sender, e);
        }
    }
}
