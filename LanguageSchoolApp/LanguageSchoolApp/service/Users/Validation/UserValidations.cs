using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.repository.Users.Students;
using LanguageSchoolApp.repository.Users.Teachers;

namespace LanguageSchoolApp.service.Users.Validation
{
    public class UserValidations
    {
        private static bool IsAlpha(string str)
        {
            return str.All(char.IsLetter);
        }

        private static bool IsNumeric(string str)
        {
            return str.All(char.IsDigit);
        }

        public static bool Validate(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(name) || !IsAlpha(name))
            {
                throw new UserValidationException("Invalid name", UserValidationExceptionType.InvalidName);
            }
            if (string.IsNullOrEmpty(surname) || !IsAlpha(surname))
            {
                throw new UserValidationException("Invalid surname", UserValidationExceptionType.InvalidSurname);
            }
            if (string.IsNullOrEmpty(phoneNumber) || !IsNumeric(phoneNumber))
            {
                throw new UserValidationException("Invalid phone number format", UserValidationExceptionType.InvalidPhoneFormat);
            }
            if (password != confirmPassword)
            {
                throw new UserValidationException("Passwords don't match", UserValidationExceptionType.PasswordsNotMatching);
            }
            try
            {
                DateTime birthday = DateTime.Parse(birthdayStr);
                if (birthday > DateTime.Now)
                {
                    throw new UserValidationException("Invalid birthday format", UserValidationExceptionType.InvalidBirthdayInput);
                }
            }
            catch (FormatException)
            {
                throw new UserValidationException("Invalid birthday format", UserValidationExceptionType.InvalidBirthdayInput);
            }
            try
            {
                Gender gender = Enum.Parse<Gender>(genderStr);
            }
            catch (ArgumentException) 
            {
                throw new UserValidationException("Invalid gender format", UserValidationExceptionType.InvalidGender);
            }

            return true;
        }

        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new UserValidationException("Email Field is Empty!", UserValidationExceptionType.EmptyField);
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new UserValidationException("Invalid email format!", UserValidationExceptionType.InvalidEmailFormat);
            }
            Dictionary<string, Teacher> allTeachers = TeacherRepository.ReadFromFile();
            if (allTeachers.ContainsKey(email))
            {
                throw new UserValidationException("Already exists teacher with email: " + email,
                    UserValidationExceptionType.EmailExists);
            }
            Dictionary<string, Student> allStudents = StudentRepository.ReadFromFile();
            if (allStudents.ContainsKey(email))
            {
                throw new UserValidationException("Already exists student with email: " + email,
                    UserValidationExceptionType.EmailExists);
            }
            return true;
        }
    }
}
