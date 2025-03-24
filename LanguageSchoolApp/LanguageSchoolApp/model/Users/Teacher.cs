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
        public double Grade { get; set; }
        public List<Course> MyCourses {  get; set; }
        public List<Exam> MyExams { get; set; }
        public List<LanguageProficiency> LanguageProficiencies { get; set; }

        public Teacher() { }

        public Teacher(string _name, string _surname, Gender _gender,
                    DateTime _birthday, string _phoneNumber, string _email,
                    string _password, double _grade, List<Course> _myCourses, List<Exam> _myExams,
                    List<LanguageProficiency> _languageProficiencies)
        :base(_name, _surname, _gender, _birthday, _phoneNumber, _email, _password) { 
            Grade = _grade;
            MyCourses = _myCourses;
            MyExams = _myExams;
            LanguageProficiencies = _languageProficiencies;
        }

        public Teacher(string _name, string _surname, Gender _gender,
                    DateTime _birthday, string _phoneNumber, string _email,
                    string _password, List<LanguageProficiency> _languageProficiencies) 
        :base(_name, _surname, _gender, _birthday, _phoneNumber, _email, _password) { 
            Grade = 0.0;
            MyCourses = new List<Course>();
            MyExams = new List<Exam>();
            LanguageProficiencies = _languageProficiencies;
        }
    }
}
