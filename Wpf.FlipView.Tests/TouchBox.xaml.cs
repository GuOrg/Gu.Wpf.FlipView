namespace Wpf.FlipView.Tests
{
    using System.Windows.Input;

    using WPF.FlipView;

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
            this._tracker = new TouchGestureTracker { InputElement = InputElement };
            this._tracker.Gestured += (_, g) => Args.Add(new ArgsVm(g));
        }

        protected override void OnStarted(object sender, InputEventArgs e)
        {
            _gestureStarted = true;
            base.OnStarted(sender, e);
        }

        protected override void OnInput(object sender, InputEventArgs e)
        {
            if (_gestureStarted)
            {
                base.OnInput(sender, e);
            }
        }

        protected override void OnEnded(object sender, InputEventArgs e)
        {
            _gestureStarted = false;
            base.OnEnded(sender, e);
        }
    }
}
