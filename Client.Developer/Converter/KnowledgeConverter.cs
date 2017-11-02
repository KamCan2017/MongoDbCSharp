using Developer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Client.Developer.Converter
{
    public class KnowledgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var knowledges = value as IEnumerable<KnowledgeModel>;
            if(knowledges != null && knowledges.Any())
            {
                string text = string.Empty;
                for (int i = 0; i < knowledges.Count(); i++)
                {
                    text += knowledges.ElementAt(i).Technology;
                    if (i != knowledges.Count() - 1)
                        text += ",";
                }

                return text;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
