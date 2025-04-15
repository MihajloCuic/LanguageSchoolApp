using LanguageSchoolApp.model.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.service.Exams
{
    public interface IExamApplicationService
    {
        Dictionary<int, ExamApplication> GetAllExamApplications();
        ExamApplication GetExamApplication(int id);
        List<ExamApplication> GetAllExamApplicationsByStudentId(string studentId);
        List<ExamApplication> GetAllExamApplicationsByExamId(int examId);
        bool ExamApplicationExists(int id);
        void CreateExamApplication(string studentId, int examId);
        void AcceptExamApplication(int id);
        void DenyExamApplication(int id);
        void DeleteExamApplication(int id);
        int GenerateId(string studentId, int examId);
    }
}
