#pragma warning disable SA1600 // Elements must be documented
namespace Gu.Wpf.FlipView.AttachedProperties
{
    using System.Windows;

    /// <summary>
    /// Attached properties for animating position during transitions.
    /// </summary>
    public static class Transform
    {
        /// <summary>
        /// The offset relative to the size of the element in the x direction.
        /// 1 means the position is ActualWidth
        /// </summary>
        public static readonly DependencyProperty RelativeOffsetXProperty = DependencyProperty.RegisterAttached(
            "RelativeOffsetX",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(
                default(double),
                OnRelativeOffsetXChanged));

        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.RegisterAttached(
            "OffsetX",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.RegisterAttached(
            "ScaleX",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(1.0));

        public static readonly DependencyProperty RelativeOffsetYProperty = DependencyProperty.RegisterAttached(
            "RelativeOffsetY",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(
                default(double),
                OnRelativeOffsetYChanged));

        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.RegisterAttached(
            "OffsetY",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.RegisterAttached(
            "ScaleY",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(1.0));

        public static double GetRelativeOffsetX(FrameworkElement element)
        {
            return (double)element.GetValue(RelativeOffsetXProperty);
        }

        /// <summary>
        /// Set the offset relative to the size of the element in the x direction.
        /// 1 means the position is ActualWidth
        /// </summary>
        public static void SetRelativeOffsetX(FrameworkElement element, double value)
        {
            element.SetValue(RelativeOffsetXProperty, value);
        }

        /// <summary>
        /// Get the absolute offset in the x direction.
        /// </summary>
        public static double GetOffsetX(DependencyObject element)
        {
            return (double)element.GetValue(OffsetXProperty);
        }

        /// <summary>
        /// Set the absolute offset in the x direction.
        /// </summary>
        public static void SetOffsetX(DependencyObject element, double value)
        {
            element.SetValue(OffsetXProperty, value);
        }

        /// <summary>
        /// Get the scale in the x direction.
        /// </summary>
        public static double GetScaleX(DependencyObject element)
        {
            return (double)element.GetValue(ScaleXProperty);
        }

        /// <summary>
        /// Set the scale in the x direction.
        /// </summary>
        public static void SetScaleX(DependencyObject element, double value)
        {
            element.SetValue(ScaleXProperty, value);
        }

        /// <summary>
        /// Get the offset relative to the size of the element in the y direction.
        /// 1 means the position is ActualHeight
        /// </summary>
        public static double GetRelativeOffsetY(DependencyObject element)
        {
            return (double)element.GetValue(RelativeOffsetYProperty);
        }

        /// <summary>
        /// Set the offset relative to the size of the element in the y direction.
        /// 1 means the position is ActualHeight
        /// </summary>
        public static void SetRelativeOffsetY(DependencyObject element, double value)
        {
            element.SetValue(RelativeOffsetYProperty, value);
        }

        /// <summary>
        /// Get the absolute offset in the y direction.
        /// </summary>
        public static double GetOffsetY(DependencyObject element)
        {
            return (double)element.GetValue(OffsetYProperty);
        }

        /// <summary>
        /// Set the absolute offset in the y direction.
        /// </summary>
        public static void SetOffsetY(DependencyObject element, double value)
        {
            element.SetValue(OffsetYProperty, value);
        }

        /// <summary>
        /// Get the scale in the y direction.
        /// </summary>
        public static double GetScaleY(DependencyObject element)
        {
            return (double)element.GetValue(ScaleYProperty);
        }

        /// <summary>
        /// Set the scale in the y direction.
        /// </summary>
        public static void SetScaleY(DependencyObject element, double value)
        {
            element.SetValue(ScaleYProperty, value);
        }

        private static void OnRelativeOffsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe)
            {
                var value = (double)e.NewValue;
                fe.SetCurrentValue(OffsetXProperty, value * fe.ActualWidth);
            }
        }

        private static void OnRelativeOffsetYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe)
            {
                var value = (double)e.NewValue;
                fe.SetCurrentValue(OffsetYProperty, value * fe.ActualHeight);
            }
        }
    }
}
