using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DSManager.Converters {
    class ValueToHiddenConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value is string)
                return string.IsNullOrEmpty((string)value) ? Visibility.Hidden : Visibility.Visible;
            if (value is decimal)
                return (decimal)value == 0 ? Visibility.Hidden : Visibility.Visible;
            if (value is bool)
                return (bool) value ? Visibility.Visible : Visibility.Hidden;

            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
