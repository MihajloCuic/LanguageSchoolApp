using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;

namespace LanguageSchoolApp.service.Courses
{
    public interface ICourseService
    {
        Dictionary<int, Course> GetAllCourses();
        List<Course> GetAllCoursesById(List<int> courseIds);
        Course GetCourse(int courseId);
        bool CourseExists(int courseId);
        List<Course> GetAllAvailableCourses();
        List<Course> GetAllFilteredCourses(string languageName, string languageLevelStr, string courseTypeStr);
        List<Course> SortCourses(List<Course> courses, string beginningDateSortingStr, string durationSortingStr);
        void CreateCourse(string languageName, string languageLevel, int maxParticipants, int duration, List<KeyValuePair<string, string>> classPeriods, string beginningDate, string courseType, string teacherId);
        void UpdateCourse(int courseId, string languageName, string languageLevel, int maxParticipants, int duration, List<KeyValuePair<string, string>> classPeriods, string beginningDate, string courseType);
        void DeleteCourse(int courseId);
        int GenerateId(LanguageProficiency languageProficiency, DateTime beginningDate, CourseType courseType, string teacherId);
        List<FinishedCourseDTO> GetFinishedCoursesDTO(List<FinishedCourse> finishedCourses);
    }
}
