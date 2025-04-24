using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.repository.Exams;
using LanguageSchoolApp.exceptions.Exams;
using System.Security.Cryptography;
using System.Text;
using LanguageSchoolApp.model;

namespace LanguageSchoolApp.service.Exams
{
    public class ExamApplicationService : IExamApplicationService
    {
        private readonly IExamApplicationRepository examApplicationRepository;
        private readonly IExamService examService;

        public ExamApplicationService(IExamApplicationRepository _examApplicationRepository, IExamService _examService)
        { 
            examApplicationRepository = _examApplicationRepository;
            examService = _examService;
        }

        public Dictionary<int, ExamApplication> GetAllExamApplications() 
        { 
            return examApplicationRepository.GetAllExamApplications();
        }

        public ExamApplication GetExamApplication(int id) 
        { 
            return examApplicationRepository.GetExamApplication(id);
        }

        public List<ExamApplication> GetAllExamApplicationsByStudentId(string studentId) 
        { 
            return examApplicationRepository.GetAllExamApplicationsByStudentId(studentId);
        }

        public List<ExamApplication> GetAllExamApplicationsByExamId(int examId) 
        { 
            return examApplicationRepository.GetAllExamApplicationsByExamId(examId);
        }

        public bool ExamApplicationExists(int id) 
        { 
            return examApplicationRepository.ExamApplicationExists(id);
        }

        public void CreateExamApplication(string studentId, int examId) 
        { 
            int examApplicationId = GenerateId(studentId, examId);
            if (ExamApplicationExists(examId)) 
            { 
                throw new ExamApplicationException("Exam application already exists", ExamApplicationExceptionType.ExamApplicationAlreadyExists);
            }
            ExamApplication examApplication = new ExamApplication(examApplicationId, studentId, examId, AcceptationType.Accepted);
            examApplicationRepository.CreateExamApplication(examApplication);
            examService.SignupStudentToExam(examId, studentId);
        }

        public void DeleteExamApplication(int id) 
        {
            if (!ExamApplicationExists(id)) 
            { 
                throw new ExamApplicationException("Exam application not found", ExamApplicationExceptionType.ExamApplicationNotFound);
            }
            ExamApplication application = GetExamApplication(id);
            examService.WithdrawStudentFromExam(application.ExamId, application.StudentId);
            examApplicationRepository.DeleteExamApplication(id);
        }

        public void DeleteAllExamApplicationsByIds(List<int> ids)
        {
            foreach (int id in ids) 
            {
                if (!ExamApplicationExists(id))
                {
                    throw new ExamApplicationException($"Exam application with id {id} not found !", ExamApplicationExceptionType.ExamApplicationNotFound);
                }
                DeleteExamApplication(id);
            }
        }

        public int GenerateId(string studentId, int examId) 
        { 
            string combination = studentId + examId.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combination));
                int hashCode = BitConverter.ToInt32(hashBytes, 0);

                return hashCode == int.MinValue ? 0 : Math.Abs(hashCode);
            }
        }
    }
}
