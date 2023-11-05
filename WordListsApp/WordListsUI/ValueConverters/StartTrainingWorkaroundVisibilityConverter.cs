using System.Globalization;

namespace WordListsUI.ValueConverters;
internal class StartTrainingWorkaroundVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string name)
        {
            if (name is "^^_$Placeholder$_^^")
            {
                return false;
            }
            return true;
        }
        throw new NotImplementedException("Must be type WordCollectionOwner");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
