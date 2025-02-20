using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LanguageSchoolApp.model.Courses;
using System.IO;

namespace LanguageSchoolApp.repository.Courses
{
    public class CourseRepository
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "Courses.json");

        public static void WriteToFile(Dictionary<int, Course> allCourses) {
            try {
                string serializedCourses = JsonConvert.SerializeObject(allCourses, Formatting.Indented);
                File.WriteAllText(filename, serializedCourses);
            } catch (IOException e) { 
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<int, Course> ReadFromFile() { 
            Dictionary<int, Course> allCourses = new Dictionary<int, Course>();
            try {
                string data = File.ReadAllText(filename);
                allCourses = JsonConvert.DeserializeObject<Dictionary<int, Course>>(data);
            }
            catch (IOException e) {
                throw new Exception(e.Message);
            }
            return allCourses;
        }
    }
}
