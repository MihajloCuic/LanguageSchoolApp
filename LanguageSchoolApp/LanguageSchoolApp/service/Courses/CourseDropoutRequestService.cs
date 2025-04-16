using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model;
using LanguageSchoolApp.exceptions.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.repository.Courses;
using System.Security.Cryptography;

namespace LanguageSchoolApp.service.Courses
{
    public class CourseDropoutRequestService : ICourseDropoutRequestService
    {
        private readonly ICourseDropoutRequestRepository repository;

        public CourseDropoutRequestService(ICourseDropoutRequestRepository _repository)
        {
            repository = _repository;
        }

        public Dictionary<int, CourseDropoutRequest> GetAllDropoutRequests() 
        { 
            return repository.GetAllDropoutRequests();
        }

        public List<CourseDropoutRequest> GetAllRequestsByCourseId(int courseId) 
        { 
            return repository.GetAllRequestsByCourseId(courseId);
        }

        public CourseDropoutRequest GetRequest(int id) 
        { 
            return repository.GetRequest(id);
        }

        public bool DropoutRequestExists(int id) 
        { 
            return repository.DropoutRequestExists(id);
        }

        public void CreateDropoutRequest(string studentId, int courseId, DropoutReason reason, string details) 
        { 
            int requestId = GenerateId(studentId, courseId, reason);
            if (DropoutRequestExists(requestId)) 
            {
                throw new CourseDropoutRequestException("Request already exists", CourseDropoutRequestExceptionType.DropoutRequestExists);
            }

            CourseDropoutRequest request = new CourseDropoutRequest(requestId, studentId, courseId, reason, details);
            repository.CreateDropoutRequest(request);
        }

        public void ProcessDropoutRequest(int requestId) 
        {
            if (!DropoutRequestExists(requestId)) 
            { 
                throw new CourseDropoutRequestException("Request not found", CourseDropoutRequestExceptionType.DropoutRequestNotFound);
            }

            CourseDropoutRequest request = GetRequest(requestId);
            request.Pending = false;
            repository.EditDropoutRequest(request);
        }

        public int GenerateId(string studentId, int courseId, DropoutReason reason) 
        { 
            string combination = studentId + courseId.ToString() + reason.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combination));
                int hashCode = BitConverter.ToInt32(hashBytes, 0);

                return hashCode == int.MinValue ? 0 : Math.Abs(hashCode);
            }
        }
    }
}
