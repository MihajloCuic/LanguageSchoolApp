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
        Teacher GetTeacherByCourseId(int courseId);
        bool TeacherExists(string teacherId);
        void AddTeacher(Teacher teacher);
        void UpdateTeacher(string teacherId, Teacher teacher);
        void DeleteTeacher(string teacherId);
        void WriteToFile();
        void AddCourse(int courseId, string teacherId);
        void DeleteCourse(int courseId, string teacherId);
        void AddExam(int examId, string teacherId);
        void DeleteExam(int examId, string teacherId);
    }
}
