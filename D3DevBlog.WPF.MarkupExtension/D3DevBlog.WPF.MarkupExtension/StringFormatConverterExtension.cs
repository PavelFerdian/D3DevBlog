using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace D3DevBlog.WPF.MarkupExtension
{
    [ValueConversion(typeof(object), typeof(string))]
    public class StringFormatConverterExtension : System.Windows.Markup.MarkupExtension, IValueConverter
    {
        public string Format { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string format = parameter as string;
            if (string.IsNullOrEmpty(format))
                format = Format;

            if (!string.IsNullOrEmpty(format))
            {
                return string.Format(culture, format, value);
            }
            else
            {
                return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
