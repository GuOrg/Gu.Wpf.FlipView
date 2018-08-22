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
        /// Gets or sets the offset relative to the size of the element in the x direction.
        /// 0 means <see cref="OffsetXProperty"/> is zero.
        /// 1 means <see cref="OffsetXProperty"/> is ActualWidth
        /// -1 means <see cref="OffsetXProperty"/> is -ActualWidth
        /// </summary>
        public static readonly DependencyProperty RelativeOffsetXProperty = DependencyProperty.RegisterAttached(
            "RelativeOffsetX",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(
                default(double),
                OnRelativeOffsetXChanged));

        /// <summary>
        /// The absolute offset in the x direction.
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.RegisterAttached(
            "OffsetX",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(default(double)));

        /// <summary>
        /// The scale in the x direction.
        /// </summary>
        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.RegisterAttached(
            "ScaleX",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(1.0));

        /// <summary>
        /// The offset relative to the size of the element in the y direction.
        /// 1 means the position is ActualHeight
        /// </summary>
        public static readonly DependencyProperty RelativeOffsetYProperty = DependencyProperty.RegisterAttached(
            "RelativeOffsetY",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(
                default(double),
                OnRelativeOffsetYChanged));

        /// <summary>
        /// The absolute offset in the y direction.
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.RegisterAttached(
            "OffsetY",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(default(double)));

        /// <summary>
        /// The scale in the y direction.
        /// </summary>
        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.RegisterAttached(
            "ScaleY",
            typeof(double),
            typeof(Transform),
            new PropertyMetadata(1.0));

        /// <summary>Helper for getting <see cref="RelativeOffsetXProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to read <see cref="RelativeOffsetXProperty"/> from.</param>
        /// <returns>RelativeOffsetX property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static double GetRelativeOffsetX(FrameworkElement element)
        {
            return (double)element.GetValue(RelativeOffsetXProperty);
        }

        /// <summary>Helper for setting <see cref="RelativeOffsetXProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to set <see cref="RelativeOffsetXProperty"/> on.</param>
        /// <param name="value">RelativeOffsetX property value.</param>
        public static void SetRelativeOffsetX(FrameworkElement element, double value)
        {
            element.SetValue(RelativeOffsetXProperty, value);
        }

        /// <summary>Helper for getting <see cref="OffsetXProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="OffsetXProperty"/> from.</param>
        /// <returns>OffsetX property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static double GetOffsetX(DependencyObject element)
        {
            return (double)element.GetValue(OffsetXProperty);
        }

        /// <summary>Helper for setting <see cref="OffsetXProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="OffsetXProperty"/> on.</param>
        /// <param name="value">OffsetX property value.</param>
        public static void SetOffsetX(DependencyObject element, double value)
        {
            element.SetValue(OffsetXProperty, value);
        }

        /// <summary>Helper for getting <see cref="ScaleXProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ScaleXProperty"/> from.</param>
        /// <returns>ScaleX property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static double GetScaleX(DependencyObject element)
        {
            return (double)element.GetValue(ScaleXProperty);
        }

        /// <summary>Helper for setting <see cref="ScaleXProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="ScaleXProperty"/> on.</param>
        /// <param name="value">ScaleX property value.</param>
        public static void SetScaleX(DependencyObject element, double value)
        {
            element.SetValue(ScaleXProperty, value);
        }

        /// <summary>Helper for getting <see cref="RelativeOffsetYProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="RelativeOffsetYProperty"/> from.</param>
        /// <returns>RelativeOffsetY property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static double GetRelativeOffsetY(DependencyObject element)
        {
            return (double)element.GetValue(RelativeOffsetYProperty);
        }

        /// <summary>Helper for setting <see cref="RelativeOffsetYProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="RelativeOffsetYProperty"/> on.</param>
        /// <param name="value">RelativeOffsetY property value.</param>
        public static void SetRelativeOffsetY(DependencyObject element, double value)
        {
            element.SetValue(RelativeOffsetYProperty, value);
        }

        /// <summary>Helper for getting <see cref="OffsetYProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="OffsetYProperty"/> from.</param>
        /// <returns>OffsetY property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static double GetOffsetY(DependencyObject element)
        {
            return (double)element.GetValue(OffsetYProperty);
        }

        /// <summary>Helper for setting <see cref="OffsetYProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="OffsetYProperty"/> on.</param>
        /// <param name="value">OffsetY property value.</param>
        public static void SetOffsetY(DependencyObject element, double value)
        {
            element.SetValue(OffsetYProperty, value);
        }

        /// <summary>Helper for getting <see cref="ScaleYProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ScaleYProperty"/> from.</param>
        /// <returns>ScaleY property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static double GetScaleY(DependencyObject element)
        {
            return (double)element.GetValue(ScaleYProperty);
        }

        /// <summary>Helper for setting <see cref="ScaleYProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="ScaleYProperty"/> on.</param>
        /// <param name="value">ScaleY property value.</param>
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
