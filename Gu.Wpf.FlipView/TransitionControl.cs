using System.Windows;
using System.Windows.Controls;

namespace Gu.Wpf.FlipView
{
    public class TransitionControl : ContentControl
    {
        static TransitionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionControl), new FrameworkPropertyMetadata(typeof(TransitionControl)));
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
        }
    }
}
