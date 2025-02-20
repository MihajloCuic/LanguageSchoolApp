using LanguageSchoolApp.model.Courses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.repository.Courses
{
    public class CourseApplicationRepository
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "CourseApplications.json");

        public static void WriteToFile(Dictionary<int, CourseApplication> allCourseApplications)
        {
            try
            {
                string serializedCourseApplications = JsonConvert.SerializeObject(allCourseApplications, Formatting.Indented);
                File.WriteAllText(filename, serializedCourseApplications);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<int, CourseApplication> ReadFromFile()
        {
            Dictionary<int, CourseApplication> allCourseApplications = new Dictionary<int, CourseApplication>();
            try
            {
                string data = File.ReadAllText(filename);
                allCourseApplications = JsonConvert.DeserializeObject<Dictionary<int, CourseApplication>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return allCourseApplications;
        }
    }
}
