using LanguageSchoolApp.repository.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.service.Validation;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.exceptions.Exams;
using System.Security.Cryptography;
using LanguageSchoolApp.exceptions.Courses;
using LanguageSchoolApp.repository.Courses;

namespace LanguageSchoolApp.service.Exams
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository examRepository;

        public ExamService(IExamRepository _examRepository)
        { 
            examRepository = _examRepository;
        }

        public Dictionary<int, Exam> GetAllExams() 
        { 
            return examRepository.GetAllExams();
        }

        public List<Exam> GetAllExamsById(List<int> examsIds) 
        {
            return examRepository.GetAllExamsById(examsIds);
        }

        public bool ExamExists(int examId) 
        { 
            return examRepository.ExamExists(examId);
        }

        public Exam GetExam(int examId)
        { 
            return examRepository.GetExam(examId);
        }

        public bool CheckIfExamsMatch(int examId, List<int> teachersExams, DateTime examDate) 
        {
            List<Exam> exams = GetAllExamsById(teachersExams);
            foreach (Exam exam in exams) 
            {
                if (exam.Id == examId) 
                {
                    continue;
                }
                if (exam.ExamDate.Equals(examDate)) 
                {
                    return true;
                }
            }
            return false;
        }

        public List<Exam> GetAvailableExams(List<Course> finishedCourses)
        { 
            return examRepository.GetAvailableExams(finishedCourses);
        }

        public List<Exam> GetAllFilteredExams(List<Exam> exams, string languageName, string languageLevelStr) 
        { 
            LanguageLevel? languageLevel = null;
            if (!string.IsNullOrEmpty(languageLevelStr)) 
            {
                languageLevel = Enum.Parse<LanguageLevel>(languageLevelStr);
            }
            return examRepository.GetAllFilteredExams(exams, languageName, languageLevel);
        }
        public List<Exam> SortExams(List<Exam> exams, string examDateSortingStr) 
        { 
            SortingDirection examDateSorting = Enum.Parse<SortingDirection>(examDateSortingStr);
            return examRepository.SortExams(exams, examDateSorting);
        }

        public Exam CreateExam(string languageName, string languageLevelStr, string examDateStr, int maxParticipants, Teacher teacher) 
        { 
            Validations.ValidateExam(languageName, languageLevelStr, examDateStr, maxParticipants);
            LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(languageLevelStr);
            LanguageProficiency languageProficiency = new LanguageProficiency(languageName, languageLevel);
            DateTime examDate = DateTime.Parse(examDateStr);

            if (CheckIfExamsMatch(-1, teacher.MyExamsIds, examDate)) 
            {
                throw new ExamException("Teacher is busy than", ExamExceptionType.TeacherIsBusy);
            }

            int examId = GenerateId(languageProficiency, examDate, teacher.Email);
            if (ExamExists(examId)) 
            {
                throw new ExamException("Same exam already exists", ExamExceptionType.ExamAlreadyExists);
            }

            Exam newExam = new Exam(examId, languageProficiency, examDate, maxParticipants);
            examRepository.CreateExam(newExam);
            return newExam;
        }

        public void UpdateExam(int examId, string languageName, string languageLevelStr, string examDateStr, int maxParticipants, Teacher teacher) 
        {
            Validations.ValidateExam(languageName, languageLevelStr, examDateStr, maxParticipants);
            LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(languageLevelStr);
            LanguageProficiency languageProficiency = new LanguageProficiency(languageName, languageLevel);
            DateTime examDate = DateTime.Parse(examDateStr);

            if (CheckIfExamsMatch(examId, teacher.MyExamsIds, examDate)) 
            {
                throw new ExamException("Teacher is busy than", ExamExceptionType.TeacherIsBusy);
            }
            Exam exam = GetExam(examId);
            exam.LanguageProficiency = languageProficiency;
            exam.ExamDate = examDate;
            exam.MaxParticipants = maxParticipants;
            examRepository.UpdateExam(examId, exam);
        }

        public void DeleteExam(int examId) 
        {
            if (!ExamExists(examId))
            {
                throw new ExamException("Exam not found", ExamExceptionType.ExamNotFound);
            }
            examRepository.DeleteExam(examId);
        }

        public void DeleteAllExamsByIds(List<int> examIds)
        {
            foreach (int id in examIds)
            {
                if (!ExamExists(id))
                {
                    throw new ExamException($" Exam with id {id} not found !", ExamExceptionType.ExamNotFound);
                }
               DeleteExam(id);
            }
        }

        public int GenerateId(LanguageProficiency languageProficiency, DateTime examDate, string teacherId)
        {
            string combination = teacherId + languageProficiency.LanguageName + languageProficiency.LanguageLevel.ToString() + examDate.ToString("ddMMyyyyHHmm");
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combination));
                int hashCode = BitConverter.ToInt32(hashBytes, 0);
                return hashCode == int.MinValue ? 0 : Math.Abs(hashCode);
            }
        }

        private ExamResultsDTO ExamToExamResultsDTO(Exam exam, ExamResults examResults) 
        {
            return new ExamResultsDTO(exam.Id, exam.LanguageProficiency, examResults.TotalScore, examResults.PartialScores, examResults.Passed);
        }

        public List<ExamResultsDTO> GetExamResultsDTO(List<ExamResults> examResults)
        { 
            List<int> examsIds = examResults.Select(er => er.ExamId).ToList();
            List<Exam> exams = GetAllExamsById(examsIds);
            var examsDict = exams.ToDictionary(exam => exam.Id);

            return examResults.Where(er => examsDict.ContainsKey(er.ExamId))
                              .Select(er => ExamToExamResultsDTO(examsDict[er.ExamId], er))
                              .ToList();
        }

        public void SignupStudentToExam(int examId, string studentId)
        {
            if (!ExamExists(examId))
            { 
                throw new ExamException("Exam not found !", ExamExceptionType.ExamNotFound);
            }

            Exam exam = GetExam(examId);
            exam.Participants.Add(studentId);
            examRepository.UpdateExam(examId, exam);
        }

        public void WithdrawStudentFromExam(int examId, string studentId) 
        {
            if (!ExamExists(examId))
            {
                throw new ExamException("Exam not found !", ExamExceptionType.ExamNotFound);
            }

            Exam exam = GetExam(examId);
            exam.Participants.Remove(studentId);
            examRepository.UpdateExam(examId, exam);
        }

        public void FinishExam(int examId) 
        {
            if (!ExamExists(examId))
            {
                throw new ExamException("Exam not found !", ExamExceptionType.ExamNotFound);
            }

            Exam exam = GetExam(examId);
            exam.IsFinished = true;
            examRepository.UpdateExam(examId , exam);
        }
    }
}
