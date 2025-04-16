using LanguageSchoolApp.exceptions.Exams;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Exams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LanguageSchoolApp.repository.Exams
{
    public class ExamRepository : IExamRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "Exams.json");
        private readonly Dictionary<int, Exam> allExams;

        public ExamRepository() 
        {
            allExams = ReadFromFile();
        }

        public Dictionary<int, Exam> GetAllExams() 
        { 
            return allExams;
        }

        public List<Exam> GetAllExamsById(List<int> examsIds) 
        { 
            return examsIds.Where(key => allExams.ContainsKey(key))
                           .Select(key => allExams[key])
                           .ToList();
        }

        public bool ExamExists(int examId) 
        { 
            return allExams.ContainsKey(examId);
        }

        public Exam GetExam(int examId) 
        {
            if (!allExams.ContainsKey(examId)) 
            { 
                throw new ExamException("Exam not found", ExamExceptionType.ExamNotFound);
            }
            return allExams[examId];
        }

        public List<Exam> GetAvailableExams(List<Course> finishedCourses)
        { 
            List<Exam> availableExams = new List<Exam>();
            foreach (Exam exam in allExams.Values) 
            {
                if (!finishedCourses.Any(course => course.LanguageProficiency.Equals(exam.LanguageProficiency))) 
                { 
                    continue;
                }
                if (exam.Participants.Count >= exam.MaxParticipants) 
                { 
                    continue;
                }
                if ((exam.ExamDate - DateTime.Now).TotalDays <= 30) 
                {
                    continue;
                }
                availableExams.Add(exam);
            }
            return availableExams;
        }

        public List<Exam> GetAllFilteredExams(List<Exam> exams, string languageName, LanguageLevel? languageLevel) 
        {
            List<Exam> foundExams = new List<Exam>();
            foreach (Exam exam in exams) 
            {
                if (!string.IsNullOrEmpty(languageName) && exam.LanguageProficiency.LanguageName.ToLower() != languageName.ToLower()) 
                {
                    continue;
                }
                if (languageLevel != null && !exam.LanguageProficiency.LanguageLevel.Equals(languageLevel)) 
                {
                    continue;
                }

                foundExams.Add(exam);
            }
            return foundExams;
        }

        public List<Exam> SortExams(List<Exam> exams, SortingDirection examDateSorting) 
        {
            if (examDateSorting.Equals(SortingDirection.None))
            {
                return exams;
            }
            if (examDateSorting.Equals(SortingDirection.Ascending))
            {
                return exams.OrderBy(exam => exam.ExamDate).ToList();
            }
            if (examDateSorting.Equals(SortingDirection.Descending))
            {
                return exams.OrderByDescending(exam => exam.ExamDate).ToList();
            }
            return new List<Exam>();
        }

        public void CreateExam(Exam exam) 
        { 
            allExams.Add(exam.Id, exam);
            WriteToFile();
        }

        public void UpdateExam(int examId, Exam exam)
        {
            allExams[examId] = exam;
            WriteToFile();
        }

        public void DeleteExam(int examId) 
        { 
            allExams.Remove(examId);
            WriteToFile();
        }

        public void DeleteAllExamsByIds(List<int> examIds)
        {
            foreach (int id in examIds)
            {
                allExams.Remove(id);
            }
            WriteToFile();
        }

        public void WriteToFile()
        {
            try
            {
                string serializedExams = JsonConvert.SerializeObject(allExams, Formatting.Indented);
                File.WriteAllText(filename, serializedExams);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<int, Exam> ReadFromFile()
        {
            Dictionary<int, Exam> exams = new Dictionary<int, Exam>();
            try
            {
                string data = File.ReadAllText(filename);
                exams = JsonConvert.DeserializeObject<Dictionary<int, Exam>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return exams;
        }
    }
}
