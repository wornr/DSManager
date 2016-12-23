using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DSManager.Converters {
    class ValueToCollapsedNegativeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value is string)
                return string.IsNullOrEmpty((string)value) ? Visibility.Visible : Visibility.Collapsed;
            if (value is decimal)
                return (decimal)value == 0 ? Visibility.Visible : Visibility.Collapsed;
            if (value is bool)
                return (bool) value ? Visibility.Collapsed : Visibility.Visible;

            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
