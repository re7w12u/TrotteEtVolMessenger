using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TrotteEtVolSMS
{
    class RecipientConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.ToString().ToLower() == "pilote")
            {
                return "paraglider30x30.jpg";
            }
            else if (value.ToString().ToLower() == "assistant")
            {
                return "assistant30x30.jpg";
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
