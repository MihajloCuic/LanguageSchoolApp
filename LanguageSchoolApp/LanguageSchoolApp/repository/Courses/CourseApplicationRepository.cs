using LanguageSchoolApp.model.Courses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model;

namespace LanguageSchoolApp.repository.Courses
{
    public class CourseApplicationRepository : ICourseApplicationRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "CourseApplications.json");
        private readonly Dictionary<int, CourseApplication> allCourseApplications;

        public CourseApplicationRepository()
        {
            allCourseApplications = ReadFromFile();
        }

        public Dictionary<int, CourseApplication> GetAllCourseApplications()
        { 
            return allCourseApplications;
        }

        public List<CourseApplication> GetAllCourseApplicationsByStudentId(string studentId)
        { 
            return allCourseApplications.Where(courseApplication => courseApplication.Value.StudentId == studentId && courseApplication.Value.Acceptance == AcceptationType.Pending).Select(courseApplication => courseApplication.Value).ToList();
        }

        public List<CourseApplication> GetAllCourseApplicationsByCourseId(int courseId)
        { 
            return allCourseApplications.Where(courseApplication => courseApplication.Value.CourseId == courseId).Select(courseApplication => courseApplication.Value).ToList();
        }

        public CourseApplication GetCourseApplication(int id)
        { 
            return allCourseApplications[id];
        }

        public bool CourseApplicationExists(int id) 
        { 
            return allCourseApplications.ContainsKey(id);
        }

        public void CreateCourseApplication(CourseApplication courseApplication)
        { 
            allCourseApplications.Add(courseApplication.Id, courseApplication);
            WriteToFile();
        }

        public void UpdateCourseApplication(CourseApplication courseApplication)
        { 
            allCourseApplications[courseApplication.Id] = courseApplication;
            WriteToFile();
        }

        public void DeleteCourseApplication(int id) 
        { 
            allCourseApplications.Remove(id);
            WriteToFile();
        }

        public void WriteToFile()
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
            Dictionary<int, CourseApplication> courseApplications = new Dictionary<int, CourseApplication>();
            try
            {
                string data = File.ReadAllText(filename);
                courseApplications = JsonConvert.DeserializeObject<Dictionary<int, CourseApplication>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return courseApplications;
        }
    }
}
