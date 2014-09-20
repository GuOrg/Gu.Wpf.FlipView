using System;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.FlipView.Tests
{
    using System.Windows.Input;

    using WPF.FlipView;

    /// <summary>
    /// Interaction logic for MouseBox.xaml
    /// </summary>
    public partial class MouseBox : EventBox
    {
        private bool _gestureStarted;

        private MouseGestureFinder _finder;

        public MouseBox()
        {
            InitializeComponent();
            this._finder = new MouseGestureFinder { InputElement = InputElement };
            _finder.Gestured += (_, g) => Args.Add(new ArgsVm(g));
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
