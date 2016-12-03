﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DSManager.Converters {
    class ValueToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value is string)
                return string.IsNullOrEmpty((string)value) ? Visibility.Hidden : Visibility.Visible;

            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
