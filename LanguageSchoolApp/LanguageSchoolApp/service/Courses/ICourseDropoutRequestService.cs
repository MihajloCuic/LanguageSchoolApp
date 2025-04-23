using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.service.Courses
{
    public interface ICourseDropoutRequestService
    {
        Dictionary<int, CourseDropoutRequest> GetAllDropoutRequests();
        List<CourseDropoutRequest> GetAllPendingRequestsByCourseId(int courseId);
        CourseDropoutRequest GetRequest(int id);
        bool DropoutRequestExists(int id);
        void CreateDropoutRequest(string studentId, int courseId, DropoutReason reason, string details);
        void ProcessDropoutRequest(int requestId);
        int GenerateId(string studentId, int courseId);
    }
}
