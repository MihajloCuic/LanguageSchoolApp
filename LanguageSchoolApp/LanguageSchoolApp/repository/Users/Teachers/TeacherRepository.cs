using LanguageSchoolApp.model.Courses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.exceptions.Users;

namespace LanguageSchoolApp.repository.Users.Teachers
{
    public class TeacherRepository : ITeacherRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "Teachers.json");
        private readonly Dictionary<string, Teacher> allTeachers;

        public TeacherRepository() 
        {
            allTeachers = ReadFromFile();
        }

        public Dictionary<string, Teacher> GetAllTeachers()
        { 
            return allTeachers;
        }

        public Teacher GetTeacher(string teacherId)
        {
            if (!allTeachers.ContainsKey(teacherId)) 
            {
                throw new UserException("Teacher not found!", UserExceptionType.UserNotFound);
            }
            return allTeachers[teacherId];
        }

        public Teacher GetTeacherByCourseId(int courseId)
        { 
            return allTeachers.Where(teacher => teacher.Value.MyCoursesIds.Contains(courseId)).Select(teacher => teacher.Value).First();
        }

        public bool TeacherExists(string teacherId)
        { 
            return allTeachers.ContainsKey(teacherId);
        }

        public void AddTeacher(Teacher teacher)
        { 
            allTeachers.Add(teacher.Email, teacher);
            WriteToFile();
        }

        public void UpdateTeacher(string teacherId, Teacher teacher)
        { 
            allTeachers[teacherId] = teacher;
            WriteToFile();
        }

        public void DeleteTeacher(string teacherId)
        { 
            allTeachers.Remove(teacherId);
            WriteToFile();
        }

        public void WriteToFile()
        {
            try
            {
                string serializedTeachers = JsonConvert.SerializeObject(allTeachers, Formatting.Indented);
                File.WriteAllText(filename, serializedTeachers);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<string, Teacher> ReadFromFile()
        {
            Dictionary<string, Teacher> teachers = new Dictionary<string, Teacher>();
            try
            {
                string data = File.ReadAllText(filename);
                teachers = JsonConvert.DeserializeObject<Dictionary<string, Teacher>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return teachers;
        }

        public void AddCourse(int courseId, string teacherId)
        {
            Teacher teacher = GetTeacher(teacherId);
            teacher.MyCoursesIds.Add(courseId);
            allTeachers[teacherId] = teacher;
            WriteToFile();
        }

        public void DeleteCourse(int courseId, string teacherId)
        {
            Teacher teacher = GetTeacher(teacherId);
            teacher.MyCoursesIds.Remove(courseId);
            allTeachers[teacherId] = teacher;
            WriteToFile();
        }

        public void AddExam(int examId, string teacherId)
        {
            Teacher teacher = GetTeacher(teacherId);
            teacher.MyExamsIds.Add(examId);
            allTeachers[teacherId] = teacher;
            WriteToFile();
        }

        public void DeleteExam(int examId, string teacherId)
        {
            Teacher teacher = GetTeacher(teacherId);
            teacher.MyExamsIds.Remove(examId);
            allTeachers[teacherId] = teacher;
            WriteToFile();
        }
    }
}
