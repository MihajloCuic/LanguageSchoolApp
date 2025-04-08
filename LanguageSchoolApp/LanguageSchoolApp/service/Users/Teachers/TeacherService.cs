using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.repository.Users.Teachers;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Validation;

namespace LanguageSchoolApp.service.Users.Teachers
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository teacherRepository;

        public TeacherService(ITeacherRepository _teacherRepository)
        {
            teacherRepository = _teacherRepository;
        }

        public Dictionary<string, Teacher> GetAllTeachers() 
        { 
            return teacherRepository.GetAllTeachers();
        }

        public Teacher GetTeacher(string teacherId) 
        {
            return teacherRepository.GetTeacher(teacherId);
        }

        public void CreateTeacher(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword, List<KeyValuePair<string, string>> languageProficienciesStr) 
        {
            ValidateTeacher(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword, languageProficienciesStr);
            Validations.ValidateEmail(email);
            Gender gender = Enum.Parse<Gender>(genderStr);
            DateTime birthday = DateTime.Parse(birthdayStr);
            List<LanguageProficiency> languageProficiencies = new List<LanguageProficiency>();
            foreach (KeyValuePair<string, string> langProficiency in languageProficienciesStr)
            {
                LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(langProficiency.Value);
                LanguageProficiency languageProficiency = new LanguageProficiency(langProficiency.Key, languageLevel);
                languageProficiencies.Add(languageProficiency);
            }
            Teacher newTeacher = new Teacher(name, surname, gender, birthday, phoneNumber, email, password, languageProficiencies);
            teacherRepository.AddTeacher(newTeacher);
        }

        public void UpdateTeacher(string teacherId, string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, List<KeyValuePair<string, string>> languageProficienciesStr) 
        { 
            ValidateTeacher(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword, languageProficienciesStr);
            Teacher teacher = GetTeacher(teacherId);
            teacher.Name = name;
            teacher.Surname = surname;
            teacher.Gender = Enum.Parse<Gender>(genderStr);
            teacher.Birthday = DateTime.Parse(birthdayStr);
            teacher.PhoneNumber = phoneNumber;
            teacher.Password = password;
            List<LanguageProficiency> languageProficiencies = new List<LanguageProficiency>();
            foreach (KeyValuePair<string, string> langProficiency in languageProficienciesStr)
            {
                LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(langProficiency.Value);
                LanguageProficiency languageProficiency = new LanguageProficiency(langProficiency.Key, languageLevel);
                languageProficiencies.Add(languageProficiency);
            }
            teacher.LanguageProficiencies = languageProficiencies;
            teacherRepository.UpdateTeacher(teacherId, teacher);
        }
        
        public void DeleteTeacher(string teacherId) 
        {
            GetTeacher(teacherId); //checks if teacher exists else throws exception
            teacherRepository.DeleteTeacher(teacherId);
        }
        
        public bool ValidateTeacher(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, List<KeyValuePair<string, string>> languageProficienciesStr) 
        {
            Validations.ValidateUser(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword);
            try
            {
                foreach (KeyValuePair<string, string> langProficiency in languageProficienciesStr) 
                {
                    LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(langProficiency.Value);
                }
            }
            catch(ArgumentException) 
            {
                throw new UserValidationException("Proficiency level is not valid!", UserValidationExceptionType.InvalidLanguageProficiencyLevel);
            }
            return true;
        }

        public void AddCourse(int courseId, string teacherId) 
        { 
            teacherRepository.AddCourse(courseId, teacherId);
        }

        public void DeleteCourse(int courseId, string teacherId)
        {
            teacherRepository.DeleteCourse(courseId, teacherId);
        }

        public void AddExam(int examId, string teacherId) 
        { 
            teacherRepository.AddExam(examId, teacherId);
        }

        public void DeleteExam(int examId, string teacherId) 
        { 
            teacherRepository.DeleteExam(examId, teacherId);
        }
    }
}
