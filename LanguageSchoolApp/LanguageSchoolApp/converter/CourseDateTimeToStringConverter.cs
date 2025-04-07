﻿using LanguageSchoolApp.model.Courses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LanguageSchoolApp.converter
{
    public class CourseDateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime beginningDate) 
            { 
                return (beginningDate - DateTime.Now).TotalDays <= 7 ? "Details" : "Edit";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime beginningDate)
            {
                return (beginningDate - DateTime.Now).TotalDays <= 7 ? "Details" : "Edit";
            }
            return string.Empty;
        }
    }
}
