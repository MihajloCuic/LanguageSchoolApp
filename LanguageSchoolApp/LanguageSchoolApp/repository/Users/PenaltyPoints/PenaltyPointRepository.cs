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
using LanguageSchoolApp.service.Courses;

namespace LanguageSchoolApp.repository.Users.PenaltyPoints
{
    public class PenaltyPointRepository : IPenaltyPointsRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "PenaltyPoints.json");
        private readonly Dictionary<int, PenaltyPoint> allPenaltyPoints;

        public PenaltyPointRepository() 
        {
            allPenaltyPoints = ReadFromFile();
        }

        public Dictionary<int, PenaltyPoint> GetAllPenaltyPoints()
        { 
            return allPenaltyPoints; 
        }

        public List<PenaltyPoint> GetAllPenaltyPointsByIds(List<int> penaltyPointsIds)
        { 
            return allPenaltyPoints.Where(penaltyPoint => penaltyPointsIds.Contains(penaltyPoint.Value.Id)).Select(penaltyPoint => penaltyPoint.Value).ToList();
        }

        public PenaltyPoint GetPenaltyPoint(int id)
        {
            return allPenaltyPoints[id];
        }

        public bool PenaltyPointExists(int id)
        { 
            return allPenaltyPoints.ContainsKey(id);
        }

        public void CreatePenaltyPoint(PenaltyPoint penaltyPoint)
        {
            allPenaltyPoints.Add(penaltyPoint.Id, penaltyPoint);
            WriteToFile();
        }

        public void UpdatePenaltyPoint(PenaltyPoint penaltyPoint) 
        {
            allPenaltyPoints[penaltyPoint.Id] = penaltyPoint;
            WriteToFile();
        }

        public Dictionary<int, int> PenaltyPointsReport()
        {
            Dictionary<int, int> reportResults = new Dictionary<int, int>();
            foreach (PenaltyPoint penaltyPoint in allPenaltyPoints.Values)
            {
                if (!reportResults.ContainsKey(penaltyPoint.CourseId))
                {
                    reportResults[penaltyPoint.CourseId] = 0;
                }

                reportResults[penaltyPoint.CourseId] += 1;
            }

            return reportResults;
        }

        public  void WriteToFile()
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
            Dictionary<int, PenaltyPoint> penaltyPoints = new Dictionary<int, PenaltyPoint>();
            try
            {
                string data = File.ReadAllText(filename);
                penaltyPoints = JsonConvert.DeserializeObject<Dictionary<int, PenaltyPoint>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return penaltyPoints;
        }
    }
}
