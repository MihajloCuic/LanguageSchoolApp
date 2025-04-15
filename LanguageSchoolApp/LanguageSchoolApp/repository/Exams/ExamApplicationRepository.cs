using LanguageSchoolApp.model.Exams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.repository.Exams
{
    public class ExamApplicationRepository : IExamApplicationRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "ExamApplications.json");
        private readonly Dictionary<int, ExamApplication> allExamApplications;

        public ExamApplicationRepository()
        {
            allExamApplications = ReadFromFile();
        }

        public Dictionary<int, ExamApplication> GetAllExamApplications()
        { 
            return allExamApplications;
        }

        public ExamApplication GetExamApplication(int id)
        {
            return allExamApplications[id];
        }

        public List<ExamApplication> GetAllExamApplicationsByStudentId(string studentId)
        { 
            return allExamApplications.Where(examApplication => examApplication.Value.StudentId == studentId).Select(examApplication => examApplication.Value).ToList();
        }

        public List<ExamApplication> GetAllExamApplicationsByExamId(int examId) 
        { 
            return allExamApplications.Where(examApplication => examApplication.Value.ExamId == examId).Select(examApplication => examApplication.Value).ToList();
        }

        public bool ExamApplicationExists(int id) 
        { 
            return allExamApplications.ContainsKey(id);
        }

        public void CreateExamApplication(ExamApplication examApplication)
        {
            allExamApplications[examApplication.Id] = examApplication;
            WriteToFile();
        }

        public void UpdateExamApplication(ExamApplication examApplication)
        {
            allExamApplications[examApplication.Id] = examApplication;
            WriteToFile();
        }

        public void DeleteExamApplication(int id)
        {
            allExamApplications.Remove(id);
            WriteToFile();
        }

        public void WriteToFile()
        {
            try
            {
                string serializedExamApplications = JsonConvert.SerializeObject(allExamApplications, Formatting.Indented);
                File.WriteAllText(filename, serializedExamApplications);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<int, ExamApplication> ReadFromFile()
        {
            Dictionary<int, ExamApplication> examApplications = new Dictionary<int, ExamApplication>();
            try
            {
                string data = File.ReadAllText(filename);
                examApplications = JsonConvert.DeserializeObject<Dictionary<int, ExamApplication>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return examApplications;
        }
    }
}
