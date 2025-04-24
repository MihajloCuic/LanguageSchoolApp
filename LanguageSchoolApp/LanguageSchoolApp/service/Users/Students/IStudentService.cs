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
using LanguageSchoolApp.model.Exams;

namespace LanguageSchoolApp.service.Users.Students
{
    public interface IStudentService
    {
        Dictionary<string, Student> GetAllStudents();
        List<Student> GetAllStudentsByIds(List<string> ids);
        Student GetStudent(string studentId);
        bool StudentExists(string studentId);
        void CreateStudent(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword, string professionalDegreeStr);
        Student UpdateStudent(string studentId, string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, string professionalDegreeStr);
        void DeleteStudent(string studentId);
        bool ValidateStudent(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, string professionalDegree);
        void WithdrawStudentFromCourse(string studentId);
        void EnrollStudentToCourse(string studentId, int courseId);
        void AssignStudentPenaltyPoint(string studentId, int penaltyPointId);
        void DeleteStudentPenaltyPoint(string studentId, int penaltyPointId);
        GradedStudent GradeStudent(string studentId, int grade);
        void GradeStudentsExam(string studentId, ExamResults examResults);
    }
}
