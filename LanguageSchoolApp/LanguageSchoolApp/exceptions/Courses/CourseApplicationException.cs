using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.exceptions.Courses
{
    public enum CourseApplicationExceptionType
    { 
        CourseApplicationExists,
        CourseApplicationNotFound
    }

    public class CourseApplicationException : Exception
    {
        public string Text { get; set; }
        public CourseApplicationExceptionType Type { get; set; }

        public CourseApplicationException(string text, CourseApplicationExceptionType type)
        {
            Text = text;
            Type = type;
        }
    }
}
