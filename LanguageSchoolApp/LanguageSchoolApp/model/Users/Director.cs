using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Users
{
    public class Director : User
    {
        public Director() { }
        public Director(string _name, string _surname, Gender _gender, DateTime _birthday, int _phoneNumber, string _email, string _password)
        {
            Name = _name;
            Surname = _surname;
            Gender = _gender;
            Birthday = _birthday;
            PhoneNumber = _phoneNumber;
            Email = _email;
            Password = _password;
        }
    }
}
