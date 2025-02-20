using LanguageSchoolApp.model.Courses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.repository.Users
{
    public class DirectorRepository
    {
        public static readonly string filename = Path.Combine("..", "..", "..", "data", "Director.json");

        public static void WriteToFile(Dictionary<int, Director> allDirectors)
        {
            try
            {
                string serializedDirectors = JsonConvert.SerializeObject(allDirectors, Formatting.Indented);
                File.WriteAllText(filename, serializedDirectors);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Dictionary<int, Director> ReadFromFile()
        {
            Dictionary<int, Director> allDirectors = new Dictionary<int, Director>();
            try
            {
                string data = File.ReadAllText(filename);
                allDirectors = JsonConvert.DeserializeObject<Dictionary<int, Director>>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return allDirectors;
        }
    }
}
