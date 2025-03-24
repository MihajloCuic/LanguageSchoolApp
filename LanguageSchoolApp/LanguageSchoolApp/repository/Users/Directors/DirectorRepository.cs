using LanguageSchoolApp.model.Courses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model.Users;
using LanguageSchoolApp.model;

namespace LanguageSchoolApp.repository.Users.Directors
{
    public class DirectorRepository : IDirectorRepository
    {
        private static readonly string filename = Path.Combine("..", "..", "..", "data", "Director.json");
        private readonly Director director;

        public DirectorRepository()
        {
            director = ReadFromFile();
        }

        public Director GetDirector() 
        { 
            return director;
        }

        public void UpdateDirector(string name, string surname, Gender gender, DateTime birthday, string phoneNumber, string email, string password)
        {
            director.Name = name;
            director.Surname = surname;
            director.Gender = gender;
            director.Birthday = birthday;
            director.PhoneNumber = phoneNumber;
            director.Email = email;
            director.Password = password;
            WriteToFile();
        }

        public void WriteToFile()
        {
            try
            {
                string serializedDirectors = JsonConvert.SerializeObject(director, Formatting.Indented);
                File.WriteAllText(filename, serializedDirectors);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Director ReadFromFile()
        {
            Director dir = new Director();
            try
            {
                string data = File.ReadAllText(filename);
                dir = JsonConvert.DeserializeObject<Director>(data);
            }
            catch (IOException e)
            {
                throw new Exception(e.Message);
            }
            return dir;
        }
    }
}
