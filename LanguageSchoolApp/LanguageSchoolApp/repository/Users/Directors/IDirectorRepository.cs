using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchoolApp.model;
using LanguageSchoolApp.model.Users;

namespace LanguageSchoolApp.repository.Users.Directors
{
    public interface IDirectorRepository
    {
        Director GetDirector();
        void UpdateDirector(string name, string surname, Gender gender, DateTime birthday, string phoneNumber, string email, string password);
        void WriteToFile();
    }
}
