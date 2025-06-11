using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.service.Courses
{
    public interface ICourseService
    {
        Dictionary<int, Course> GetAllCourses();
        List<Course> GetAllCoursesById(List<int> courseIds);
        Course GetCourse(int courseId);
        bool CourseExists(int courseId);
        List<Course> GetAllAvailableCourses();
        List<Course> GetAllFilteredCourses(List<Course> courses, string languageName, string languageLevelStr, string courseTypeStr);
        List<Course> SortCourses(List<Course> courses, string beginningDateSortingStr, string durationSortingStr);
        Course CreateCourse(string languageName, string languageLevelStr, int maxParticipants, int duration, List<ClassPeriod> classPeriods, DateTime beginningDate, string courseTypeStr, Teacher teacher);
        void UpdateCourse(int courseId, string languageName, string languageLevelStr, int maxParticipants, int duration, List<ClassPeriod> classPeriods, DateTime beginningDate, string courseTypeStr);
        void DeleteCourse(int courseId);
        void DeleteAllCoursesByIds(List<int> courseIds);
        int GenerateId(LanguageProficiency languageProficiency, DateTime beginningDate, CourseType courseType, string teacherId);
        List<FinishedCourseDTO> GetFinishedCoursesDTO(List<FinishedCourse> finishedCourses);
        List<Course> GetTeachersPendingCourses(List<int> allTeacherCoursesIds);
        List<Course> GetTeacherActiveCourses(List<int> allTeacherCoursesIds);
        void RemoveStudentFromCourse(string studentId, int courseId);
        void AddStudentToCourse(string studentId, int courseId);
        void FinishCourse(int courseId);
        bool IsFinished(int courseId);
        bool CourseOverlap(List<Course> courses, Course course);
    }
}
