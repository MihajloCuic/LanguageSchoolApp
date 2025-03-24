using LanguageSchoolApp.model;
using LanguageSchoolApp.repository.Users;
using LanguageSchoolApp.service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.service.Users.Students
{
    public interface IStudentService
    {
        Dictionary<string, Student> GetAllStudents();
        Student GetStudent(string studentId);
        void CreateStudent(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword, string professionalDegreeStr);
        void UpdateStudent(string studentId, string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, string professionalDegreeStr);
        void DeleteStudent(string studentId);
        bool ValidateStudent(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, string professionalDegree);
    }
}
