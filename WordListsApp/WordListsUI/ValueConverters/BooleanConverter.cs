using System.Globalization;

namespace WordListsUI.ValueConverters;
internal class BooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolean)
        {
            return !boolean;
        }
        throw new InvalidOperationException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolean)
        {
            return !boolean;
        }
        throw new InvalidOperationException();
    }
}
