using LanguageSchoolApp.model.Courses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.repository.Users
{
    public class StudentRepository
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "Students.json");

        public static void WriteToFile(Dictionary<int, Student> allStudents)
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

        public static Dictionary<int, Student> ReadFromFile()
        {
            Dictionary<int, Student> allStudents = new Dictionary<int, Student>();
            try
            {
                string data = File.ReadAllText(filename);
                allStudents = JsonConvert.DeserializeObject<Dictionary<int, Student>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return allStudents;
        }
    }
}
