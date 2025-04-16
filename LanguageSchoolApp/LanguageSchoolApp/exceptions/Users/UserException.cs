using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.exceptions.Users
{
    public enum UserExceptionType 
    {
        UserNotFound,
        InvalidLoginInput,
        InvalidGrade
    }
    public class UserException : Exception
    {
        public string Text { get; set; }
        public UserExceptionType Type { get; set; }

        public UserException(string _text, UserExceptionType _type) 
        { 
            Text = _text;
            Type = _type;
        }
    }
}
