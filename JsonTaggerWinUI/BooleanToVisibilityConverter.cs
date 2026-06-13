using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace JsonTaggerWinUI
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            if (value is bool b && b) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            if (value is Visibility v) return v == Visibility.Visible;
            return false;
        }
    }
}
