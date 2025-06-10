using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.service.Users.Directors
{
    public interface IDirectorService
    {
        Director GetDirector();
        Director UpdateDirector(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword);
        void AddFinishedCourse(int courseId);
        List<int> RemoveFinishedCourse(int courseId);
        void AddFinishedExam(int examId);
        List<int> RemoveFinishedExam(int examId);
        bool ValidateDirector(string name, string surname, string genderStr, string birthdayStr, string phoneNumber, string email, string password, string confirmPassword);
    }
}
