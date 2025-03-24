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
        Course GetCourse(int courseId);
        List<Course> CheckIfCoursesMatch(DateTime beginningDate, CourseType courseType, List<ClassPeriod> classPeriods);
        void AddCourse(Course course);
        void UpdateCourse(int courseId, Course course);
        void DeleteCourse(int courseId);
        void WriteToFile();
    }
}
