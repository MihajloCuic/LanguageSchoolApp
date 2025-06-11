using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.repository.Users.Directors
{
    public interface IDirectorRepository
    {
        Director GetDirector();
        Director UpdateDirector(string name, string surname, Gender gender, DateTime birthday, string phoneNumber, string email, string password);
        void AddFinishedCourse(int courseId);
        List<int> RemoveFinishedCourse(int courseId);
        void AddFinishedExam(int examId);
        List<int> RemoveFinishedExam(int examId);
        void WriteToFile();
    }
}
