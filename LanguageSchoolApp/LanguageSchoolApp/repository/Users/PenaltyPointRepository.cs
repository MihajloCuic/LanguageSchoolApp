using LanguageSchoolApp.model.Courses;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.repository.Users
{
    public class PenaltyPointRepository
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "PenaltyPoints.json");

        public static void WriteToFile(Dictionary<int, PenaltyPoint> allPenaltyPoints)
        {
            try
            {
                string serializedPenaltyPoints = JsonConvert.SerializeObject(allPenaltyPoints, Formatting.Indented);
                File.WriteAllText(filename, serializedPenaltyPoints);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<int, PenaltyPoint> ReadFromFile()
        {
            Dictionary<int, PenaltyPoint> allPenaltyPoints = new Dictionary<int, PenaltyPoint>();
            try
            {
                string data = File.ReadAllText(filename);
                allPenaltyPoints = JsonConvert.DeserializeObject<Dictionary<int, PenaltyPoint>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return allPenaltyPoints;
        }
    }
}
