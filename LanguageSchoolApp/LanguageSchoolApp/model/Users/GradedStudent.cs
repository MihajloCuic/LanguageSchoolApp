using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Users
{
    public class GradedStudent
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }

        public GradedStudent(string _studentId, string _name, int _grade) 
        { 
            StudentId = _studentId;
            Name = _name;
            Grade = _grade;
        }
    }
}
