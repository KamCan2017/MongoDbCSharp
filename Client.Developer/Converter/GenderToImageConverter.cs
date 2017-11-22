using Developer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.Developer.Converter
{
    public class GenderToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == Gender.Female)
            {
                var image =  Application.Current.FindResource("UserFemaleIcon") as Image;
                return new Image() { Source = image.Source };
            }

            if ((string)value == Gender.Male)
            {
                var image =  Application.Current.FindResource("UserMaleIcon") as Image;
                return new Image() { Source = image.Source};
            }
            return new Image();
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
