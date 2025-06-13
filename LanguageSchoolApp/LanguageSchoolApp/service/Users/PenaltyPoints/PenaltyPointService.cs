using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.model;
using LanguageSchoolApp.repository.Users.PenaltyPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using LanguageSchoolApp.exceptions.Users;
using LanguageSchoolApp.service.Users.Students;
using LanguageSchoolApp.service.Courses;
using LanguageSchoolApp.model.Courses;

namespace LanguageSchoolApp.service.Users.PenaltyPoints
{
    public class PenaltyPointService : IPenaltyPointService
    {
        private readonly IPenaltyPointsRepository penaltyPointsRepository;
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;

        public PenaltyPointService(IPenaltyPointsRepository _penaltyPointsRepository, IStudentService _studentService, ICourseService _courseService)
        {
            penaltyPointsRepository = _penaltyPointsRepository;
            studentService = _studentService;
            courseService = _courseService;
        }

        public Dictionary<int, PenaltyPoint> GetAllPenaltyPoints() 
        { 
            return penaltyPointsRepository.GetAllPenaltyPoints();
        }

        public List<PenaltyPoint> GetAllPenaltyPointsByIds(List<int> penaltyPointsIds) 
        { 
            return penaltyPointsRepository.GetAllPenaltyPointsByIds(penaltyPointsIds);
        }

        public PenaltyPoint GetPenaltyPoint(int id) 
        { 
            return penaltyPointsRepository.GetPenaltyPoint(id);
        }

        public bool PenaltyPointExists(int id) 
        { 
            return penaltyPointsRepository.PenaltyPointExists(id);
        }

        public void CreatePenaltyPoint(string studentId, int courseId, PenaltyReason reason) 
        {
            int id = GenerateId(studentId, courseId, reason);
            if (PenaltyPointExists(id))
            {
                throw new PenaltyPointException("Penalty point already exists !", PenaltyPointExceptionType.PenaltyPointExists);
            }

            PenaltyPoint penaltyPoint = new PenaltyPoint(id, courseId, reason, DateTime.Now);
            penaltyPointsRepository.CreatePenaltyPoint(penaltyPoint);
            studentService.AssignStudentPenaltyPoint(studentId, id);
        }

        public void DeletePenaltyPoint(string studentId, int id) 
        {
            if (!PenaltyPointExists(id)) 
            { 
                throw new PenaltyPointException("Penalty point not found !", PenaltyPointExceptionType.PenaltyPointNotFound);
            }

            PenaltyPoint penaltyPoint = GetPenaltyPoint(id);
            penaltyPoint.Deleted = true;
            penaltyPointsRepository.UpdatePenaltyPoint(penaltyPoint);
            studentService.DeleteStudentPenaltyPoint(studentId, id);
        }

        public int GenerateId(string studentId, int courseId, PenaltyReason reason) 
        { 
            string combination = studentId + courseId.ToString() + DateTime.Now.ToString("ddMMyyyy") + reason.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combination));
                int hashCode = BitConverter.ToInt32(hashBytes, 0);
                return hashCode == int.MinValue ? 0 : Math.Abs(hashCode);
            }
        }

        public Dictionary<string, double> PenaltyPointsCourseReport()
        {
            Dictionary<int, int> reportResults = penaltyPointsRepository.PenaltyPointsReport();
            List<Course> courses = courseService.GetAllCoursesById(reportResults.Keys.ToList());
            Dictionary<string, double> results = new Dictionary<string, double>();
            foreach (var item in reportResults)
            {
                Course course = courses.First(course => course.Id == item.Key);
                string course_name = course.LanguageProficiency.LanguageName + " " + course.LanguageProficiency.LanguageLevel.ToString() + " " + course.Id;
                results.Add(course_name, item.Value);
            }
            return results;
        }
    }
}
