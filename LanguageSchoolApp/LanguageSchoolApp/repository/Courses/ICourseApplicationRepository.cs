using LanguageSchoolApp.model.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.repository.Courses
{
    public interface ICourseApplicationRepository
    {
        Dictionary<int, CourseApplication> GetAllCourseApplications();
        CourseApplication GetCourseApplication(int id);
        List<CourseApplication> GetAllCourseApplicationsByStudentId(string studentId);
        List<CourseApplication> GetAllCourseApplicationsByCourseId(int courseId);
        bool CourseApplicationExists(int id);
        void CreateCourseApplication(CourseApplication courseApplication);
        void UpdateCourseApplication(CourseApplication courseApplication);
        void DeleteCourseApplication(int id);
        void WriteToFile();
    }
}
