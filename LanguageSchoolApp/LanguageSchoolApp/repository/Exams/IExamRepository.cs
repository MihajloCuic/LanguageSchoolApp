using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Exams;

namespace LanguageSchoolApp.repository.Exams
{
    public interface IExamRepository
    {
        Dictionary<int, Exam> GetAllExams();
        Exam GetExam(int examId);
        bool ExamExists(int examId);
        List<Exam> GetAllExamsById(List<int> examsIds);
        void CreateExam(Exam exam);
        void UpdateExam(int examId, Exam exam);
        void DeleteExam(int examId);
        void WriteToFile();
    }
}
