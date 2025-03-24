using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.model;
using LanguageSchoolApp.service.Users.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.repository.Users.Students;

namespace LanguageSchoolApp.service.Users.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;

        public StudentService(IStudentRepository _studentRepository)
        {
            studentRepository = _studentRepository;
        }

        public Dictionary<string, Student> GetAllStudents() 
        {
            return studentRepository.GetAllStudents();
        }

        public Student GetStudent(string studentId)
        { 
            return studentRepository.GetStudent(studentId);
        }

        public void CreateStudent(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword, string professionalDegreeStr) 
        { 
            ValidateStudent(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword, professionalDegreeStr);
            UserValidations.ValidateEmail(email);
            Gender gender = Enum.Parse<Gender>(genderStr);
            DateTime birthday = DateTime.Parse(birthdayStr);
            ProfessionalDegree professionalDegree = Enum.Parse<ProfessionalDegree>(professionalDegreeStr);
            Student newStudent = new Student(name, surname, gender, birthday, phoneNumber, email, password, professionalDegree);
            studentRepository.AddStudent(newStudent);
        }

        public void UpdateStudent(string studentId, string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, string professionalDegreeStr) 
        { 
            ValidateStudent(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword, professionalDegreeStr);
            Student student = GetStudent(studentId);
            student.Name = name;
            student.Surname = surname;
            student.Gender = Enum.Parse<Gender>(genderStr);
            student.Birthday = DateTime.Parse(birthdayStr);
            student.PhoneNumber = phoneNumber;
            student.Password = password;
            student.ProfessionalDegree = Enum.Parse<ProfessionalDegree>(professionalDegreeStr);
            studentRepository.UpdateStudent(studentId, student);
        }

        public void DeleteStudent(string studentId) 
        {
            GetStudent(studentId); //checks if user exists else throws exception "user not found"
            studentRepository.DeleteStudent(studentId);
        }

        public bool ValidateStudent(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, string professionalDegreeStr) 
        {
            UserValidations.Validate(name, surname, genderStr, birthdayStr, phoneNumber, password, confirmPassword);
            try
            { 
                ProfessionalDegree professionalDegree = Enum.Parse<ProfessionalDegree>(professionalDegreeStr);
            }
            catch (ArgumentException) 
            {
                throw new UserValidationException("Professional Degree is not valid", UserValidationExceptionType.InvalidProfessionalDegree);
            }
            return true;
        }
    }
}
