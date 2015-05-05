namespace Gu.Wpf.FlipView.AttachedProperties
{
    using System.Windows;

    public static class Transform
    {
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
            new PropertyMetadata(default(double)));
       
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
            new PropertyMetadata(default(double)));

        public static void SetRelativeOffsetX(FrameworkElement element, double value)
        {
            element.SetValue(RelativeOffsetXProperty, value);
        }

        public static double GetRelativeOffsetX(FrameworkElement element)
        {
            return (double)element.GetValue(RelativeOffsetXProperty);
        }

        public static void SetOffsetX(DependencyObject element, double value)
        {
            element.SetValue(OffsetXProperty, value);
        }

        public static double GetOffsetX(DependencyObject element)
        {
            return (double)element.GetValue(OffsetXProperty);
        }

        public static void SetScaleX(DependencyObject element, double value)
        {
            element.SetValue(ScaleXProperty, value);
        }

        public static double GetScaleX(DependencyObject element)
        {
            return (double)element.GetValue(ScaleXProperty);
        }

        public static void SetRelativeOffsetY(DependencyObject element, double value)
        {
            element.SetValue(RelativeOffsetYProperty, value);
        }

        public static double GetRelativeOffsetY(DependencyObject element)
        {
            return (double)element.GetValue(RelativeOffsetYProperty);
        }
        
        public static void SetOffsetY(DependencyObject element, double value)
        {
            element.SetValue(OffsetYProperty, value);
        }

        public static double GetOffsetY(DependencyObject element)
        {
            return (double)element.GetValue(OffsetYProperty);
        }

        public static void SetScaleY(DependencyObject element, double value)
        {
            element.SetValue(ScaleYProperty, value);
        }

        public static double GetScaleY(DependencyObject element)
        {
            return (double)element.GetValue(ScaleYProperty);
        }

        private static void OnRelativeOffsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null)
            {
                return;
            }
            var value = (double)e.NewValue;
            fe.SetValue(OffsetXProperty,  value * fe.ActualWidth);
        }

        private static void OnRelativeOffsetYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null)
            {
                return;
            }
            var value = (double)e.NewValue;
            fe.SetValue(OffsetYProperty, value * fe.ActualHeight);
        }
    }
}
