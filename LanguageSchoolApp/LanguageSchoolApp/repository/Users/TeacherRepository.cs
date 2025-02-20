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
    public class TeacherRepository
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "Teachers.json");

        public static void WriteToFile(Dictionary<int, Teacher> allTeachers)
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

        public static Dictionary<int, Teacher> ReadFromFile()
        {
            Dictionary<int, Teacher> allTeachers = new Dictionary<int, Teacher>();
            try
            {
                string data = File.ReadAllText(filename);
                allTeachers = JsonConvert.DeserializeObject<Dictionary<int, Teacher>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return allTeachers;
        }
    }
}
