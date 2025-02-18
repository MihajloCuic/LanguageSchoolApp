using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Courses
{
    public class FinishedCourse
    {
        public Course Course { get; set; }
        public int Grade { get; set; }

        public FinishedCourse() { }

        public FinishedCourse(Course _course, int _grade) { 
            Course = _course;
            Grade = _grade;
        }
    }
}
