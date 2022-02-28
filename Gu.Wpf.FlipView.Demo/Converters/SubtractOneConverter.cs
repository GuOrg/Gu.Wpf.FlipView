namespace Gu.Wpf.FlipView.Demo.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(int), typeof(int))]
    public sealed class SubtractOneConverter : IValueConverter
    {
        public static readonly SubtractOneConverter Default = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
            {
                return i - 1;
            }

            throw new ArgumentException("Expected int", nameof(value));
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(SubtractOneConverter)} can only be used in OneWay bindings");
        }
    }
}
