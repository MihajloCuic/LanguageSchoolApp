using LanguageSchoolApp.exceptions.Exams;
using LanguageSchoolApp.model.Exams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
