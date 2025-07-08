using System.Globalization;

namespace TSV.Converters
{
    // =====================================================
    // STRING TO BOOL CONVERTER (für Validation Error Visibility)
    // =====================================================
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return !string.IsNullOrEmpty(str);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // =====================================================
    // STRING TO BORDER COLOR CONVERTER (für Validation Error Borders)
    // =====================================================
    public class StringToBorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                // Error state - rote Border
                return Colors.Red;
            }

            // Normal state - graue Border
            return Application.Current?.RequestedTheme == AppTheme.Dark
                ? Color.FromArgb("#555555")
                : Color.FromArgb("#E0E0E0");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // =====================================================
    // BOOL TO SAVE TEXT CONVERTER (für Save Button Text)
    // =====================================================
    public class BoolToSaveTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCreateMode)
            {
                return isCreateMode ? "💾 Erstellen" : "💾 Speichern";
            }
            return "💾 Speichern";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // =====================================================
    // INVERTED BOOL CONVERTER (falls benötigt)
    // =====================================================
    public class InvertedBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }
}