using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DSManager.Converters {
    class ValueToCollapsedConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value is string)
                return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;
            if (value is decimal)
                return (decimal)value == 0 ? Visibility.Collapsed : Visibility.Visible;

            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
