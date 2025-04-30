using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LanguageSchoolApp.converter
{
    public class MethodToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Course course)
            {
                return course.ClassPeriodsToString();
            }
            else if (value is Teacher teacher)
            {
                return teacher.CalculateAverageGrade().ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Course course)
            {
                return course.ClassPeriodsToString();
            }
            else if (value is Teacher teacher)
            {
                return teacher.CalculateAverageGrade().ToString();
            }
            return string.Empty;
        }
    }
}
