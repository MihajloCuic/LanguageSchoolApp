﻿using LanguageSchoolApp.repository.Users.Directors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.model;
using LanguageSchoolApp.service.Validation;

namespace LanguageSchoolApp.service.Users.Directors
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository directorRepository;

        public DirectorService(IDirectorRepository repository)
        {
            directorRepository = repository;
        }

        public Director GetDirector() 
        { 
            return directorRepository.GetDirector();
        }

        public Director UpdateDirector(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword) 
        {
            ValidateDirector(name, surname, genderStr, birthdayStr, phoneNumber, email, password, confirmPassword);
            Gender gender = Enum.Parse<Gender>(genderStr);
            DateTime birthday = DateTime.Parse(birthdayStr);

            return directorRepository.UpdateDirector(name, surname, gender, birthday, phoneNumber, email, password);
        }

        public void AddFinishedCourse(int courseId)
        { 
            directorRepository.AddFinishedCourse(courseId);
        }

        public List<int> RemoveFinishedCourse(int courseId)
        { 
            return directorRepository.RemoveFinishedCourse(courseId);
        }

        public void AddFinishedExam(int examId)
        { 
            directorRepository.AddFinishedExam(examId);
        }

        public List<int> RemoveFinishedExam(int examId)
        { 
            return directorRepository.RemoveFinishedExam(examId);
        }

        public bool ValidateDirector(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword) 
        {
            Validations.ValidateUser(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword);
            Validations.ValidateEmail(email);
            return true;
        }
    }
}
