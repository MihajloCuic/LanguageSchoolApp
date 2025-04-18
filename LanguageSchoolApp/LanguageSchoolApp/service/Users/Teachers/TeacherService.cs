﻿using System;
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
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.service.Exams;

namespace LanguageSchoolApp.service.Users.Teachers
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly ICourseService courseService;
        private readonly IExamService examService;

        public TeacherService(ITeacherRepository _teacherRepository, ICourseService _courseService, IExamService _examService)
        {
            teacherRepository = _teacherRepository;
            courseService = _courseService;
            examService = _examService;
        }

        public Dictionary<string, Teacher> GetAllTeachers() 
        { 
            return teacherRepository.GetAllTeachers();
        }

        public Teacher GetTeacher(string teacherId) 
        {
            return teacherRepository.GetTeacher(teacherId);
        }

        public bool TeacherExists(string teacherId)
        { 
            return teacherRepository.TeacherExists(teacherId);
        }

        public Teacher GetTeacherByCourseId(int courseId) 
        { 
            return teacherRepository.GetTeacherByCourseId(courseId);
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
            if (TeacherExists(teacherId))
            {
                throw new UserException("Teacher not found !", UserExceptionType.UserNotFound);
            }
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
            if (TeacherExists(teacherId))
            {
                throw new UserException("Teacher not found !", UserExceptionType.UserNotFound);
            }
            Teacher teacher = GetTeacher(teacherId);
            courseService.DeleteAllCoursesByIds(teacher.MyCoursesIds);
            examService.DeleteAllExamsByIds(teacher.MyExamsIds);
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

        public void GradeTeacher(int grade, string teacherId)
        {
            if (!TeacherExists(teacherId))
            { 
                throw new UserException("Teacher not found !", UserExceptionType.UserNotFound);
            }
            if (grade < 1 || grade > 10)
            {
                throw new UserException("Grade must be between 1-10 !", UserExceptionType.InvalidGrade);
            }

            Teacher teacher = GetTeacher(teacherId);
            teacher.MyGrades.Add(grade);
            teacherRepository.UpdateTeacher(teacherId, teacher);
        }
    }
}
