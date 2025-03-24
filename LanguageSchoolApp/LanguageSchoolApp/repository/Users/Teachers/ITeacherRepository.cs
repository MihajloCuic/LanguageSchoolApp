using LanguageSchoolApp.model.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.repository.Users.Teachers
{
    public interface ITeacherRepository
    {
        Dictionary<string, Teacher> GetAllTeachers();
        Teacher GetTeacher(string teacherId);
        void AddTeacher(Teacher teacher);
        void UpdateTeacher(string teacherId, Teacher teacher);
        void DeleteTeacher(string teacherId);
        void WriteToFile();
    }
}
