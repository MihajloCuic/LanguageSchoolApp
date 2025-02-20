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
    public class ExamApplicationRepository
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "ExamApplications.json");

        public static void WriteToFile(Dictionary<int, ExamApplication> allExamApplications)
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
            Dictionary<int, ExamApplication> allExamApplications = new Dictionary<int, ExamApplication>();
            try
            {
                string data = File.ReadAllText(filename);
                allExamApplications = JsonConvert.DeserializeObject<Dictionary<int, ExamApplication>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return allExamApplications;
        }
    }
}
