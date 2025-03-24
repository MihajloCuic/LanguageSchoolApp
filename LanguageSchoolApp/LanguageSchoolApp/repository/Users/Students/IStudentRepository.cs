using LanguageSchoolApp.model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.repository.Users.Students
{
    public interface IStudentRepository
    {
        Dictionary<string, Student> GetAllStudents();
        public Student GetStudent(string studentId);
        void AddStudent(Student student);
        public void UpdateStudent(string studentId, Student student);
        public void DeleteStudent(string studentId);
        void WriteToFile();
    }
}
