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

        public void CreateExam(string languageName, string languageLevelStr, string examDateStr, int maxParticipants, Teacher teacher) 
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
        }

        public void UpdateExam(int examId, string languageName, string languageLevelStr, string examDateStr, int maxParticipants, Teacher teacher) 
        {
            Validations.ValidateExam(languageName, languageLevelStr, examDateStr, maxParticipants);
            LanguageLevel languageLevel = Enum.Parse<LanguageLevel>(languageLevelStr);
            LanguageProficiency languageProficiency = new LanguageProficiency(languageName, languageLevel);
            DateTime examDate = DateTime.Parse(examDateStr);

            if (CheckIfExamsMatch(examId, teacher.MyExamsIds, examDate)) 
            {
                throw new ExamException("Tacher is busy than", ExamExceptionType.TeacherIsBusy);
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
    }
}
