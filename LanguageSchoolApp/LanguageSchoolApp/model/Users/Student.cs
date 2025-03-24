using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LanguageSchoolApp.model.Exams;
using LanguageSchoolApp.model.Courses;

namespace LanguageSchoolApp.model.Users
{
    public class Student : User
    {
        public ProfessionalDegree ProfessionalDegree { get; set; }
        public List<FinishedCourse> FinishedCourses { get; set; }
        public List<ExamResults> FinishedExamResults { get; set; }
        public Course EnrolledCourse { get; set; }
        public List<int> PenaltyPoints { get; set; }

        public Student() { }

        public Student(string _name, string _surname, Gender _gender,
                    DateTime _birthday, string _phoneNumber, string _email,
                    string _password, ProfessionalDegree _professionalDegree,
                    List<FinishedCourse> _finishedCourses, Course _enrolledCourse,
                    List<ExamResults> _finishedExamResults,
                    List<int> _penaltyPoints)
        :base(_name, _surname, _gender, _birthday, _phoneNumber, _email, _password) {
            ProfessionalDegree = _professionalDegree;
            FinishedCourses = _finishedCourses;
            FinishedExamResults = _finishedExamResults;
            EnrolledCourse = _enrolledCourse;
            PenaltyPoints = _penaltyPoints;
        }

        public Student(string _name, string _surname, Gender _gender,
                    DateTime _birthday, string _phoneNumber, string _email,
                    string _password, ProfessionalDegree _professionalDegree)
        :base(_name, _surname, _gender, _birthday, _phoneNumber, _email, _password) {
            ProfessionalDegree = _professionalDegree;
            FinishedCourses = new List<FinishedCourse>();
            FinishedExamResults = new List<ExamResults>();
            EnrolledCourse = null;
            PenaltyPoints = new List<int>();

        }
    }
}
