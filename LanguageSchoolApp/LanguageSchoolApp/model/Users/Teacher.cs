using LanguageSchoolApp.model.Courses;
using LanguageSchoolApp.model.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Users
{
    public class Teacher : User
    {
        public List<int> MyGrades { get; set; }
        public List<int> MyCoursesIds {  get; set; }
        public List<int> MyExamsIds { get; set; }
        public List<LanguageProficiency> LanguageProficiencies { get; set; }

        public Teacher() 
        { 
            MyGrades = new List<int>();
            MyCoursesIds = new List<int>();
            MyExamsIds = new List<int>();
            LanguageProficiencies = new List<LanguageProficiency>();
        }

        public Teacher(string _name, string _surname, Gender _gender,
                    DateTime _birthday, string _phoneNumber, string _email,
                    string _password, List<int> _myGrades, List<int> _myCoursesIds, List<int> _myExamsIds,
                    List<LanguageProficiency> _languageProficiencies)
        :base(_name, _surname, _gender, _birthday, _phoneNumber, _email, _password) { 
            MyGrades = _myGrades;
            MyCoursesIds = _myCoursesIds;
            MyExamsIds = _myExamsIds;
            LanguageProficiencies = _languageProficiencies;
        }

        public Teacher(string _name, string _surname, Gender _gender,
                    DateTime _birthday, string _phoneNumber, string _email,
                    string _password, List<LanguageProficiency> _languageProficiencies) 
        :base(_name, _surname, _gender, _birthday, _phoneNumber, _email, _password) { 
            MyGrades = new List<int>();
            MyCoursesIds = new List<int>();
            MyExamsIds = new List<int>();
            LanguageProficiencies = _languageProficiencies;
        }

        public double CalculateAverageGrade()
        {
            int sum = 0;
            foreach (int grade in MyGrades) 
            { 
                sum += grade;
            }
            if (MyGrades.Count == 0)
            {
                return 0.0;
            }
            return Math.Round((double)sum / MyGrades.Count, 2);
        }
    }
}
