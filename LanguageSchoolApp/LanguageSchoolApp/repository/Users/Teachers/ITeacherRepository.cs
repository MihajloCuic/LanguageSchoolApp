using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.repository.Users.Teachers
{
    public interface ITeacherRepository
    {
        Dictionary<string, Teacher> GetAllTeachers();
        Teacher GetTeacher(string teacherId);
        Teacher GetTeacherByCourseId(int courseId);
        bool TeacherExists(string teacherId);
        void AddTeacher(Teacher teacher);
        void UpdateTeacher(string teacherId, Teacher teacher);
        void DeleteTeacher(string teacherId);
        void WriteToFile();
        void AddCourse(int courseId, string teacherId);
        void DeleteCourse(int courseId, string teacherId);
        void AddExam(int examId, string teacherId);
        void DeleteExam(int examId, string teacherId);
        List<Teacher> FilteredTeachers(string languageName, LanguageLevel languageLevel, int grade);
        List<Teacher> SortTeachers(List<Teacher> teachers, SortingDirection name, SortingDirection grade);
        Teacher SelectTeacherForCourse(Course course);
        Teacher SelectTeacherForExam(Exam exam);
    }
}
