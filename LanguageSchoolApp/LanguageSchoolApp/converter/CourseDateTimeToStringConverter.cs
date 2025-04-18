using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.view;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LanguageSchoolApp.converter
{
    public class CourseDateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime beginningDate) 
            {
                if (DateTime.Now > beginningDate)
                {
                    return "Details";
                }
                else if ((beginningDate - DateTime.Now).TotalDays <= 7)
                {
                    return "Start it";
                }
                else
                {
                    return "Edit";
                }
                //return (beginningDate - DateTime.Now).TotalDays <= 7 ? "Details" : "Edit";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime beginningDate)
            {
                if (DateTime.Now > beginningDate)
                {
                    return "Details";
                }
                else if ((beginningDate - DateTime.Now).TotalDays <= 7)
                {
                    return "Start it";
                }
                else
                {
                    return "Edit";
                }
                //return (beginningDate - DateTime.Now).TotalDays <= 7 ? "Details" : "Edit";
            }
            return string.Empty;
        }
    }
}
