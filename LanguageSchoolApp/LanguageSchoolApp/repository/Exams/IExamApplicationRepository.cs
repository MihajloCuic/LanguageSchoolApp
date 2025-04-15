using LanguageSchoolApp.model.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.repository.Exams
{
    public interface IExamApplicationRepository
    {
        Dictionary<int, ExamApplication> GetAllExamApplications();
        ExamApplication GetExamApplication(int id);
        List<ExamApplication> GetAllExamApplicationsByStudentId(string studentId);
        List<ExamApplication> GetAllExamApplicationsByExamId(int examId);
        bool ExamApplicationExists(int id);
        void CreateExamApplication(ExamApplication examApplication);
        void UpdateExamApplication(ExamApplication examApplication);
        void DeleteExamApplication(int id);
        void WriteToFile();
    }
}
