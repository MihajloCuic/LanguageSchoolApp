using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;

namespace LanguageSchoolApp.repository.Courses
{
    public interface ICourseRepository
    {
        Dictionary<int, Course> GetAllCourses();
        List<Course> GetAllCoursesById(List<int> courseIds);
        Course GetCourse(int courseId);
        bool CourseExists(int courseId);
        List<Course> GetAllAvailableCourses();
        List<Course> GetAllFilteredCourses(List<Course> courses, string languageName, LanguageLevel? languageLevel, CourseType? courseType);
        List<Course> SortCourses(List<Course> courses, SortingDirection beginningDateSorting, SortingDirection durationSorting);
        List<Course> CheckIfCoursesMatch(int courseId, DateTime beginningDate, CourseType courseType, List<ClassPeriod> classPeriods);
        void AddCourse(Course course);
        void UpdateCourse(int courseId, Course course);
        void DeleteCourse(int courseId);
        void DeleteAllCoursesByIds(List<int> courseIds);
        void WriteToFile();
    }
}
