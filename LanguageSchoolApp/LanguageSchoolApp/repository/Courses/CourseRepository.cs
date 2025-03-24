using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LanguageSchoolApp.model.Courses;
using System.IO;
using LanguageSchoolApp.exceptions.Courses;
using LanguageSchoolApp.model;

namespace LanguageSchoolApp.repository.Courses
{
    public class CourseRepository : ICourseRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "Courses.json");
        private readonly Dictionary<int, Course> allCourses;

        public CourseRepository() 
        {
            allCourses = ReadFromFile();
        }

        public Dictionary<int, Course> GetAllCourses() 
        { 
            return allCourses;
        }

        public Course GetCourse(int courseId) 
        {
            if (!allCourses.ContainsKey(courseId)) 
            { 
                throw new CourseException("Course not found!", CourseExceptionType.CourseNotFound);
            }
            return allCourses[courseId];
        }

        public List<Course> CheckIfCoursesMatch(DateTime beginningDate, CourseType courseType, List<ClassPeriod> classPeriods) 
        { 
            List<Course> matchingCourses = new List<Course>();
            foreach (Course course in allCourses.Values) 
            {
                if (course.BeginningDate.Equals(beginningDate) && 
                    course.CourseType.Equals(courseType) &&
                    course.ClassPeriods.SequenceEqual(classPeriods)) 
                { 
                    matchingCourses.Add(course);
                }
            }
            return matchingCourses;
        }

        public void AddCourse(Course course) 
        { 
            allCourses.Add(course.Id, course);
            WriteToFile();
        }

        public void UpdateCourse(int courseId, Course course) 
        {
            allCourses[courseId] = course;
            WriteToFile();
        }

        public void DeleteCourse(int courseId) 
        { 
            allCourses.Remove(courseId);
            WriteToFile();
        }

        public void WriteToFile() 
        {
            try {
                string serializedCourses = JsonConvert.SerializeObject(allCourses, Formatting.Indented);
                File.WriteAllText(filename, serializedCourses);
            } catch (IOException e) { 
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<int, Course> ReadFromFile() { 
            Dictionary<int, Course> courses = new Dictionary<int, Course>();
            try {
                string data = File.ReadAllText(filename);
                courses = JsonConvert.DeserializeObject<Dictionary<int, Course>>(data);
            }
            catch (IOException e) {
                throw new Exception(e.Message);
            }
            return courses;
        }
    }
}
