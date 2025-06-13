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

namespace LanguageSchoolApp.repository.Users.Students
{
    public class StudentRepository : IStudentRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "Students.json");
        private Dictionary<string, Student> allStudents;

        public StudentRepository() 
        {
            allStudents = ReadFromFile();
        }

        public Dictionary<string, Student> GetAllStudents() 
        {
            return allStudents;
        }

        public List<Student> GetAllStudentsByIds(List<string> ids)
        { 
            return allStudents.Where(student => ids.Contains(student.Key)).Select(student => student.Value).ToList();
        }

        public bool StudentExists(string studentId) 
        { 
            return allStudents.ContainsKey(studentId);
        }

        public void AddStudent(Student student)
        {
            allStudents.Add(student.Email, student);
            WriteToFile();
        }

        public void DeleteStudent(string studentId) 
        {
            allStudents.Remove(studentId);
            WriteToFile();
        }

        public void UpdateStudent(string studentId, Student student) 
        { 
            allStudents[studentId] = student;
            WriteToFile();
        }

        public Dictionary<int, int> PenaltyPointsStudentReport()
        {
            Dictionary<int, int> reportResults = new Dictionary<int, int>();
            reportResults.Add(0, 0);
            reportResults.Add(1, 0);
            reportResults.Add(2, 0);
            reportResults.Add(3, 0);
            foreach (Student student in allStudents.Values)
            {
                if (student.PenaltyPoints.Count > 3)
                {
                    throw new PenaltyPointException("Student cannot have more than 3 penalty points !", PenaltyPointExceptionType.StudentReportError);
                }
                reportResults[student.PenaltyPoints.Count] += 1;
            }
            return reportResults;
        }

        public Student GetStudent(string studentId) 
        {
            if (!allStudents.ContainsKey(studentId)) 
            {
                throw new UserException("User not found!", UserExceptionType.UserNotFound);
            }
            return allStudents[studentId];
        }

        public void WriteToFile()
        {
            try
            {
                string serializedStudents = JsonConvert.SerializeObject(allStudents, Formatting.Indented);
                File.WriteAllText(filename, serializedStudents);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<string, Student> ReadFromFile()
        {
            Dictionary<string, Student> students = new Dictionary<string, Student>();
            try
            {
                string data = File.ReadAllText(filename);
                students = JsonConvert.DeserializeObject<Dictionary<string, Student>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return students;
        }
    }
}
