using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.service.Exams
{
    public interface IExamService
    {
        Dictionary<int, Exam> GetAllExams();
        List<Exam> GetAllExamsById(List<int> examId);
        bool ExamExists(int examId);
        Exam GetExam(int examId);
        bool CheckIfExamsMatch(int exam, List<int> teachersExams, DateTime examDate);
        List<Exam> GetAvailableExams(List<Course> finishedCourses);
        List<Exam> GetAllFilteredExams(List<Exam> exams, string languageName, string languageLevelStr);
        List<Exam> SortExams(List<Exam> exams, string examDateSortingStr);
        void CreateExam(string languageName, string languageLevelStr, string examDateStr, int maxParticipants, Teacher teacher);
        void UpdateExam(int examId, string languageName, string languageLevelStr, string examDateStr, int maxParticipants, Teacher teacher);
        void DeleteExam(int examId);
        int GenerateId(LanguageProficiency languageProficiency, DateTime examDate, string teacherId);
        List<ExamResultsDTO> GetExamResultsDTO(List<ExamResults> examResults);
    }
}
