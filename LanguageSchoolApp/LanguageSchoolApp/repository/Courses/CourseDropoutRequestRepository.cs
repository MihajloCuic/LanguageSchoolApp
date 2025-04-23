using System;
using System.Collections.Generic;
using System.IO;
using LanguageSchoolApp.model.Courses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LanguageSchoolApp.repository.Courses
{
    public class CourseDropoutRequestRepository : ICourseDropoutRequestRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "CourseDropoutRequests.json");
        private readonly Dictionary<int, CourseDropoutRequest> allDropoutRequests;

        public CourseDropoutRequestRepository() 
        {
            allDropoutRequests = ReadFromFile();
        }

        public Dictionary<int, CourseDropoutRequest> GetAllDropoutRequests()
        { 
            return allDropoutRequests;
        }

        public List<CourseDropoutRequest> GetAllPendingRequestsByCourseId(int courseId)
        { 
            return allDropoutRequests.Where(req => req.Value.CourseId == courseId && req.Value.Pending == true).Select(req => req.Value).ToList();
        }

        public List<CourseDropoutRequest> GetAllDropoutRequestsByStudentId(string studentId)
        {
            return allDropoutRequests.Where(req => req.Value.StudentId == studentId).Select(req => req.Value).ToList();
        }

        public CourseDropoutRequest GetRequest(int id)
        { 
            return allDropoutRequests[id];
        }

        public bool DropoutRequestExists(int id) 
        { 
            return allDropoutRequests.ContainsKey(id);
        }

        public void CreateDropoutRequest(CourseDropoutRequest courseDropoutRequest)
        {
            allDropoutRequests.Add(courseDropoutRequest.Id, courseDropoutRequest);
            WriteToFile();
        }

        public void EditDropoutRequest(CourseDropoutRequest courseDropoutRequest)
        {
            allDropoutRequests[courseDropoutRequest.Id] = courseDropoutRequest;
            WriteToFile();
        }

        public void WriteToFile()
        {
            try
            {
                string serializedRequests = JsonConvert.SerializeObject(allDropoutRequests, Formatting.Indented);
                File.WriteAllText(filename, serializedRequests);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<int, CourseDropoutRequest> ReadFromFile()
        {
            Dictionary<int, CourseDropoutRequest> requests = new Dictionary<int, CourseDropoutRequest>();
            try
            {
                string data = File.ReadAllText(filename);
                requests = JsonConvert.DeserializeObject<Dictionary<int, CourseDropoutRequest>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return requests;
        }
    }
}
