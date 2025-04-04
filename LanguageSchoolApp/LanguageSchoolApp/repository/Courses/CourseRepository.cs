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
using System.Collections.Immutable;

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

        public List<Course> GetAllCoursesById(List<int> courseIds)
        { 
            return courseIds.Where(key => allCourses.ContainsKey(key))
                            .Select(key => allCourses[key])
                            .ToList();
        }

        public Course GetCourse(int courseId) 
        {
            if (!allCourses.ContainsKey(courseId)) 
            { 
                throw new CourseException("Course not found!", CourseExceptionType.CourseNotFound);
            }
            return allCourses[courseId];
        }

        public bool CourseExists(int courseId) 
        { 
            return allCourses.ContainsKey(courseId);    
        }

        public List<Course> GetAllAvailableCourses()
        { 
            List<Course> foundCourses = new List<Course>();
            foreach (Course course in allCourses.Values) 
            {
                if (course.ParticipantsIds.Count < course.MaxParticipants && (course.BeginningDate - DateTime.Now).TotalDays >= 7) 
                { 
                    foundCourses.Add(course);
                }
            }
            return foundCourses;

        }

        public List<Course> GetAllFilteredCourses(string languageName, LanguageLevel? languageLevel, CourseType? courseType)
        {
            List<Course> foundCourses = new List<Course>();
            foreach (Course course in allCourses.Values)
            {
                if (!string.IsNullOrEmpty(languageName) && course.LanguageProficiency.LanguageName.ToLower() != languageName.ToLower()) 
                {
                    continue;
                }
                if (languageLevel != null && !course.LanguageProficiency.LanguageLevel.Equals(languageLevel)) 
                {
                    continue;
                }
                if (courseType != null && !course.CourseType.Equals(courseType)) 
                {
                    continue;
                }

                foundCourses.Add(course);
            }

            return foundCourses;
        }

        public List<Course> SortCourses(List<Course> courses, SortingDirection beginningDateSorting, SortingDirection durationSorting)
        {
            if (beginningDateSorting.Equals(SortingDirection.None) && durationSorting.Equals(SortingDirection.None)) 
            {
                return courses;
            }
            if (beginningDateSorting.Equals(SortingDirection.Ascending) && durationSorting.Equals(SortingDirection.None))
            {
                return courses.OrderBy(course => course.BeginningDate).ToList();
            }
            if (beginningDateSorting.Equals(SortingDirection.Descending) && durationSorting.Equals(SortingDirection.None))
            {
                return courses.OrderByDescending(course => course.BeginningDate).ToList();
            }
            if (beginningDateSorting.Equals(SortingDirection.None) && durationSorting.Equals(SortingDirection.Ascending))
            {
                return courses.OrderBy(course => course.Duration).ToList();
            }
            if (beginningDateSorting.Equals(SortingDirection.None) && durationSorting.Equals(SortingDirection.Descending))
            {
                return courses.OrderByDescending(course => course.Duration).ToList();
            }
            if (beginningDateSorting.Equals(SortingDirection.Ascending) && durationSorting.Equals(SortingDirection.Ascending)) 
            { 
                return courses.OrderBy(course => course.BeginningDate).ThenBy(course => course.Duration).ToList();
            }
            if(beginningDateSorting.Equals(SortingDirection.Ascending) && durationSorting.Equals(SortingDirection.Descending)) 
            { 
                return courses.OrderBy(course => course.BeginningDate).ThenByDescending(course => course.Duration).ToList();
            }
            if(beginningDateSorting.Equals(SortingDirection.Descending) && durationSorting.Equals(SortingDirection.Ascending)) 
            { 
                return courses.OrderByDescending(course => course.BeginningDate).ThenBy(course => course.Duration).ToList();
            }
            if(beginningDateSorting.Equals(SortingDirection.Descending) && durationSorting.Equals(SortingDirection.Descending)) 
            { 
                return courses.OrderByDescending(course => course.BeginningDate).ThenByDescending(course => course.Duration).ToList();
            }
            return new List<Course>();
        }

        public List<Course> CheckIfCoursesMatch(int courseId, DateTime beginningDate, CourseType courseType, List<ClassPeriod> classPeriods) 
        { 
            List<Course> matchingCourses = new List<Course>();
            foreach (Course course in allCourses.Values) 
            {
                if (course.Id == courseId) 
                {
                    continue;
                }
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
