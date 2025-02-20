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
    public class ExamRepository
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "Exams.json");

        public static void WriteToFile(Dictionary<int, Exam> allExams)
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
            Dictionary<int, Exam> allExams = new Dictionary<int, Exam>();
            try
            {
                string data = File.ReadAllText(filename);
                allExams = JsonConvert.DeserializeObject<Dictionary<int, Exam>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return allExams;
        }
    }
}
