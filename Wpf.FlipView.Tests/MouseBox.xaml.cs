using System;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.FlipView.Tests
{
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MouseBox.xaml
    /// </summary>
    public partial class MouseBox : EventBox
    {
        private bool _gestureStarted;
        public MouseBox()
        {
            InitializeComponent();
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
