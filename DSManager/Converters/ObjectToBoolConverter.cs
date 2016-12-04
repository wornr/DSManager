using System;
using System.Globalization;
using System.Windows.Data;

namespace DSManager.Converters {
    public class ObjectToBoolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is string)
                return !string.IsNullOrEmpty((string)value);

            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
