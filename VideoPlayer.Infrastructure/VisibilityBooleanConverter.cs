using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VideoPlayer.Infrastructure
{
    [ValueConversion(typeof(Visibility), typeof(Boolean))]
    public class VisibilityBooleanConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var visibility = Visibility.Visible;
            Boolean isVisible;
            if (Boolean.TryParse(value.ToString(), out isVisible))
            {
                visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            return visibility;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}