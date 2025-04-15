using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.service.Users.Teachers
{
    public interface ITeacherService
    {
        Dictionary<string, Teacher> GetAllTeachers();
        Teacher GetTeacher(string teacherId);
        Teacher GetTeacherByCourseId(int courseId);
        void CreateTeacher(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword, List<KeyValuePair<string, string>> languageProficienciesStr);
        void UpdateTeacher(string teacherId, string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, List<KeyValuePair<string, string>> languageProficienciesStr);
        void DeleteTeacher(string teacherId);
        bool ValidateTeacher(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string password, string confirmPassword, List<KeyValuePair<string, string>> languageProficienciesStr);
        void AddCourse(int courseId, string teacherId);
        void DeleteCourse(int courseId, string teacherId);
        void AddExam(int examId, string teacherId);
        void DeleteExam(int examId, string teacherId);
    }
}
