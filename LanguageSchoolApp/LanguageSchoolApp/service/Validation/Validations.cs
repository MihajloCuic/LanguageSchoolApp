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
using LanguageSchoolApp.exceptions.Courses;
using LanguageSchoolApp.exceptions.Exams;
using LanguageSchoolApp.repository.Users.Directors;

namespace LanguageSchoolApp.service.Validation
{
    public class Validations
    {
        private static bool IsAlpha(string str)
        {
            return str.All(char.IsLetter);
        }

        private static bool IsNumeric(string str)
        {
            return str.All(char.IsDigit);
        }

        public static bool ValidateUser(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword)
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
            if (string.IsNullOrEmpty(password))
            {
                throw new UserValidationException("Password is Empty", UserValidationExceptionType.EmptyPasswordField);
            }
            if (string.IsNullOrEmpty(confirmPassword))
            {
                throw new UserValidationException("Password is Empty", UserValidationExceptionType.EmptyConfirmPasswordField);
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
                throw new UserValidationException("Email Field is Empty!", UserValidationExceptionType.EmptyEmailField);
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

        public static bool ValidateCourse(string languageName, string languageLevelStr, int maxParticipants, int duration, DateTime beginningDate, string courseTypeStr)
        {
            if (String.IsNullOrEmpty(languageName) || !IsAlpha(languageName)) 
            {
                throw new CourseException("Invalid language name", CourseExceptionType.InvalidLanguageName);
            }
            try
            {
                Enum.Parse<LanguageLevel>(languageLevelStr);
            }
            catch (ArgumentException) 
            { 
                throw new CourseException("Invalid language level format", CourseExceptionType.InvalidLanguageLevel);
            }
            if (maxParticipants <= 0) 
            { 
                throw new CourseException("Max participants must be greater than 0", CourseExceptionType.InvalidMaxParticipants);
            }
            if (duration < 0) 
            {
                throw new CourseException("Duration must be greater than 0", CourseExceptionType.InvalidDuration);
            }
            if (beginningDate < DateTime.Now)
            {
                throw new CourseException("Course cannot begin in past", CourseExceptionType.InvalidBeginningDate);
            }
            try
            { 
                Enum.Parse<CourseType>(courseTypeStr);
            }
            catch (ArgumentException) 
            { 
                throw new CourseException("Invalid course type format", CourseExceptionType.InvalidCourseType);
            }
            return true;
        }

        public static bool ValidateExam(string languageName, string languageLevelStr, string examDateStr, int maxParticipants) 
        {
            if (String.IsNullOrEmpty(languageName) || !IsAlpha(languageName)) 
            {
                throw new ExamException("Invalid language name", ExamExceptionType.InvalidLanguageName);
            }
            try
            {
                Enum.Parse<LanguageLevel>(languageLevelStr);
            }
            catch(ArgumentException)
            {
                throw new ExamException("Invalid language level", ExamExceptionType.InvalidLanguageLevel);
            }
            try
            {
                DateTime.Parse(examDateStr);
            }
            catch (FormatException) 
            {
                throw new ExamException("Invalid exam date format", ExamExceptionType.InvalidExamDate);
            }
            if (maxParticipants < 0) 
            {
                throw new ExamException("Max participants must be greater than 0", ExamExceptionType.InvalidMaxParticipants);
            }
            return true;
        }

        public static User Login(string email, string password) 
        {
            if (String.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) 
            {
                throw new UserException("Email or password not correct", UserExceptionType.InvalidLoginInput);
            }
            Dictionary<string, Student> allStudents = StudentRepository.ReadFromFile();
            if (allStudents.TryGetValue(email, out Student student) && student.Password == password) 
            { 
                return allStudents[email];
            }
            Dictionary<string, Teacher> allTeachers = TeacherRepository.ReadFromFile();
            if (allTeachers.TryGetValue(email, out Teacher teacher) && teacher.Password ==password)
            {
                return allTeachers[email];
            }
            Director director = DirectorRepository.ReadFromFile();
            if (director.Email == email && director.Password == password) 
            { 
                return director;
            }
            
            throw new UserException("Email or password not correct", UserExceptionType.InvalidLoginInput);
        }
    }
}
