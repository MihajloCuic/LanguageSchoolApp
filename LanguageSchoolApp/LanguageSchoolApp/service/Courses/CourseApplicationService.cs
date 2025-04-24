using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.repository.Courses;
using LanguageSchoolApp.exceptions.Courses;
using LanguageSchoolApp.model;
using System.Security.Cryptography;
using System.Text;

namespace LanguageSchoolApp.service.Courses
{
    public class CourseApplicationService : ICourseApplicationService
    {
        private readonly ICourseApplicationRepository courseApplicationRepository;

        public CourseApplicationService(ICourseApplicationRepository _courseApplicationRepository)
        {
            courseApplicationRepository = _courseApplicationRepository;
        }

        public Dictionary<int, CourseApplication> GetAllCourseApplications() 
        { 
            return courseApplicationRepository.GetAllCourseApplications();
        }

        public CourseApplication GetCourseApplication(int id) 
        { 
            return courseApplicationRepository.GetCourseApplication(id);
        }

        public List<CourseApplication> GetCourseApplicationsByStudentId(string studentId) 
        {
            return courseApplicationRepository.GetAllCourseApplicationsByStudentId(studentId);
        }

        public List<CourseApplication> GetAllCourseApplicationsByCourseId(int courseId) 
        { 
            return courseApplicationRepository.GetAllCourseApplicationsByCourseId(courseId);
        }

        public bool CourseApplicationExists(int id) 
        { 
            return courseApplicationRepository.CourseApplicationExists(id);
        }

        public void CreateCourseApplication(string studentId, int courseId) 
        { 
            int id = GenerateId(studentId, courseId);
            if (CourseApplicationExists(id)) 
            {
                throw new CourseApplicationException("Course Application already exists", CourseApplicationExceptionType.CourseApplicationExists);
            }
            CourseApplication newCourseApplication = new CourseApplication(id, studentId, courseId, false, AcceptationType.Pending);
            courseApplicationRepository.CreateCourseApplication(newCourseApplication);
        }

        public void AcceptCourseApplication(int id) 
        {
            if (!CourseApplicationExists(id)) 
            { 
                throw new CourseApplicationException("Course application not found", CourseApplicationExceptionType.CourseApplicationNotFound);
            }
            CourseApplication courseApplication = GetCourseApplication(id);
            courseApplication.Acceptance = AcceptationType.Accepted;
            courseApplicationRepository.UpdateCourseApplication(courseApplication);
        }

        public void DenyCourseApplication(int id)
        {
            if (!CourseApplicationExists(id))
            {
                throw new CourseApplicationException("Course application not found", CourseApplicationExceptionType.CourseApplicationNotFound);
            }
            CourseApplication courseApplication = GetCourseApplication(id);
            courseApplication.Acceptance = AcceptationType.Denied;
            courseApplicationRepository.UpdateCourseApplication(courseApplication);
        }

        public void PauseCourseApplication(int id)
        {
            if (!CourseApplicationExists(id)) 
            {
                throw new CourseApplicationException("Course application not found", CourseApplicationExceptionType.CourseApplicationNotFound);
            }
            CourseApplication courseApplication = GetCourseApplication(id);
            courseApplication.Paused = true;
            courseApplicationRepository.UpdateCourseApplication(courseApplication);
        }

        public void UnpauseCourseApplication(int id)
        {
            if (!CourseApplicationExists(id))
            {
                throw new CourseApplicationException("Course application not found", CourseApplicationExceptionType.CourseApplicationNotFound);
            }
            CourseApplication courseApplication = GetCourseApplication(id);
            courseApplication.Paused = false;
            courseApplicationRepository.UpdateCourseApplication(courseApplication);
        }

        public void DeleteCourseApplication(int id) 
        {
            if (!CourseApplicationExists(id))
            {
                throw new CourseApplicationException("Course application not found", CourseApplicationExceptionType.CourseApplicationNotFound);
            }
            courseApplicationRepository.DeleteCourseApplication(id);
        }

        public void DeleteAllCourseApplicationsByIds(List<int> ids)
        {
            foreach (int id in ids)
            { 
                DeleteCourseApplication(id);
            }
        }

        public int GenerateId(string studentId, int courseId) 
        {
            string combination = studentId + courseId.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combination));
                int hashCode = BitConverter.ToInt32(hashBytes, 0);

                return hashCode == int.MinValue ? 0 : Math.Abs(hashCode);
            }
        }
    }
}
