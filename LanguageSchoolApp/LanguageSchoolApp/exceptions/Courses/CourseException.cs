using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.exceptions.Courses
{
    public enum CourseExceptionType 
    { 
        CourseNotFound,
        CourseAlreadyExists,
        InvalidLanguageProficiency,
        InvalidLanguageName,
        InvalidLanguageLevel,
        InvalidMaxParticipants,
        InvalidDuration,
        InvalidClassDay,
        InvalidClassTime,
        InvalidBeginningDate,
        InvalidCourseType,
        ClassroomsFull
    }
    public class CourseException : Exception
    {
        public string Text { get; set; }
        public CourseExceptionType Type { get; set; }

        public CourseException(string _text, CourseExceptionType _type) 
        { 
            Text = _text;
            Type = _type;
        }
    }
}
