using LanguageSchoolApp.model.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.service.Courses
{
    public interface ICourseApplicationService
    {
        Dictionary<int, CourseApplication> GetAllCourseApplications();
        CourseApplication GetCourseApplication(int id);
        List<CourseApplication> GetCourseApplicationsByStudentId(string studentId);
        List<CourseApplication> GetAllCourseApplicationsByCourseId(int courseId);
        bool CourseApplicationExists(int id);
        void CreateCourseApplication(string studentId, int courseId);
        void AcceptCourseApplication(int id);
        void DenyCourseApplication(int id);
        void PauseCourseApplication(int id);
        void UnpauseCourseApplication(int id);
        void DeleteCourseApplication(int id);
        void DeleteAllCourseApplicationsByIds(List<int> ids);
        int GenerateId(string studentId, int courseId);
    }
}
