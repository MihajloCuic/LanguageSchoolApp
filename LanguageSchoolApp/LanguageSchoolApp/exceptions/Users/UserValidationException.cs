using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.exceptions.Users
{
    public enum UserValidationExceptionType { 
        EmptyEmailField,
        EmptyPasswordField,
        EmptyConfirmPasswordField,
        InvalidName,
        InvalidSurname,
        InvalidGender,
        InvalidEmailFormat,
        EmailExists,
        PasswordsNotMatching,
        InvalidPhoneFormat,
        InvalidBirthdayInput,
        InvalidProfessionalDegree,
        InvalidLanguageProficiencyLevel
    }
    public class UserValidationException : Exception
    {
        public string Text { get; set; }
        public UserValidationExceptionType Type { get; set; }

        public UserValidationException(string _text, UserValidationExceptionType _type) {
            Text = _text;
            Type = _type;
        }
    }
}
