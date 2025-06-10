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
        public List<int> FinishedCoursesIds { get; set; }
        public List<int> FinishedExamsIds { get; set; }

        public Director() 
        {
            FinishedCoursesIds = new List<int>();
            FinishedExamsIds = new List<int>();
        }

        public Director(string _name, string _surname, Gender _gender, DateTime _birthday, string _phoneNumber, string _email, string _password)
        :base(_name, _surname, _gender, _birthday, _phoneNumber, _email, _password) 
        {
            FinishedCoursesIds = new List<int>();
            FinishedExamsIds = new List<int>();
        }

        public Director(string _name, string _surname, Gender _gender, DateTime _birthday, string _phoneNumber, string _email, string _password, 
            List<int> _finishedCoursesIds, List<int> _finishedExamsIds)
        : base(_name, _surname, _gender, _birthday, _phoneNumber, _email, _password)
        {
            FinishedCoursesIds = _finishedCoursesIds;
            FinishedExamsIds = _finishedExamsIds;
        }
    }
}
