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
        void DeleteExamApplication(int id);
        void DeleteAllExamApplicationsByIds(List<int> ids);
        int GenerateId(string studentId, int examId);
    }
}
