using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace LanguageSchoolApp.model.Users
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User() { }

        public User(string _name, string _surname, Gender _gender, DateTime _birthday, int _phoneNumber, string _email, string _password)
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
