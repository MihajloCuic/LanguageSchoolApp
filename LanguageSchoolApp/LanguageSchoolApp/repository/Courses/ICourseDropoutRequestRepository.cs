using LanguageSchoolApp.model.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.repository.Courses
{
    public interface ICourseDropoutRequestRepository
    {
        Dictionary<int, CourseDropoutRequest> GetAllDropoutRequests();
        List<CourseDropoutRequest> GetAllRequestsByCourseId(int courseId);
        CourseDropoutRequest GetRequest(int id);
        bool DropoutRequestExists(int id);
        void CreateDropoutRequest(CourseDropoutRequest courseDropoutRequest);
        void EditDropoutRequest(CourseDropoutRequest courseDropoutRequest);
        void WriteToFile();
    }
}
